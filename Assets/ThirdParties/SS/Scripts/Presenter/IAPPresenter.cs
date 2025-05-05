using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using AppsFlyerSDK;
using UnityEngine.Purchasing.Security;
using UnityEngine.Purchasing.Extension;

public class IAPPresenter : MonoBehaviour, IDetailedStoreListener
{
    public SoShop soShop;

    public string environment = "production";

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    private (UnityAction pass, UnityAction error) ActionsPurchase;

    public bool isRestore = false;

    public Product GetProduct(string productId)
    {
        if (m_StoreController == null)
        {
            return null;
        }
        else
        {
            return m_StoreController.products.WithID(productId);
        }
    }

    async void Start()
    {
        if (m_StoreController == null)
        {
            if (IsInitialized())
            {
                // ... we are done here.
                return;
            }


            try
            {
                var options = new InitializationOptions()
                    .SetEnvironmentName(environment);

                await UnityServices.InitializeAsync(options);


                var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

                soShop.AddProduct(builder);

                UnityPurchasing.Initialize(this, builder);
            }
            catch (Exception exception)
            {
                UnityEngine.Console.LogError("IAP", "UnityPurchasing: " + exception);
            }
        }
    }

    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        UnityEngine.Console.LogError("IAP", "OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        UnityEngine.Console.LogError("IAP", "OnInitializeFailed InitializationFailureReason:" + error + " n/Mesage:" + message);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        bool validPurchase = true; // Presume valid for platforms with no R.V.

        // Unity IAP's validation logic is only included on these platforms.
#if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX
        // Prepare the validator with the secrets we prepared in the Editor
        // obfuscation window.
        var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
            AppleTangle.Data(), Application.identifier);

        try
        {
            // On Google Play, result has a single product ID.
            // On Apple stores, receipts contain multiple products.
            var result = validator.Validate(args.purchasedProduct.receipt);
            // For informational purposes, we list the receipt(s)
            UnityEngine.Console.Log("IAP", "Receipt is valid. Contents: ");
            foreach (IPurchaseReceipt productReceipt in result)
            {
                UnityEngine.Console.Log("IAP", productReceipt.productID);
                UnityEngine.Console.Log("IAP", productReceipt.purchaseDate);
                UnityEngine.Console.Log("IAP", productReceipt.transactionID);
            }
        }
        catch (IAPSecurityException)
        {
            UnityEngine.Console.LogError("IAP", "Invalid receipt, not unlocking content");
            validPurchase = false;
        }
#endif
        if (validPurchase)
        {
            Pass(args.purchasedProduct.definition.id, args.purchasedProduct);
        }
        else
        {
            ActionsPurchase.error?.Invoke();
            DisplayLoading(false);
        }

        return PurchaseProcessingResult.Complete;
    }

    private void Pass(string id, Product product = null)
    {
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
               {
                   for (int i = 0; i < soShop.purchaseConfig.Count; i++)
                   {
                       if (String.Equals(id, soShop.purchaseConfig[i].GetIDStore()))
                       {
                           GameManager.Instance.GetShopPresenter().AddDataRestore(id);
                           ActionsPurchase.pass?.Invoke();
                           if (!isRestore && product != null) AppsFlyerPurchaseEvent(product);
                       }
                   }

                   UnityEngine.Console.LogSuccess("IAP", string.Format("=>>ProcessPurchase: Product: '{0}'", id));

                   DisplayLoading(false);

                   isRestore = false;
               });
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        UnityEngine.Console.LogError("IAP", string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
    {
        ActionsPurchase.error?.Invoke();
        DisplayLoading(false);
        UnityEngine.Console.LogError("IAP", $"Purchase failed: {product.definition.id}, Reason: {failureDescription.reason}, Details: {failureDescription.message}");
    }

    public void BuyItem(ItemShop item, (UnityAction pass, UnityAction error) completionHandler)
    {
        isRestore = false;
        DisplayLoading(true);
        BuyProductID(item.GetIDStore(), completionHandler);
    }


    public void AddEventBuyProductID((UnityAction pass, UnityAction error) completionHandler)
    {
        ActionsPurchase.pass = null;
        ActionsPurchase.error = null;

        ActionsPurchase.pass += completionHandler.pass;
        ActionsPurchase.error += completionHandler.error;
    }

    void BuyProductID(string productId, (UnityAction pass, UnityAction error) completionHandler)
    {
        try
        {
#if UNITY_EDITOR
            AddEventBuyProductID(completionHandler);
            Pass(productId);
#else
                        if (IsInitialized())
                        {
                            Product product = m_StoreController.products.WithID(productId);
                            if (product != null && product.availableToPurchase)
                            {
                                AddEventBuyProductID(completionHandler);
                                DisplayLoading(true);
                                m_StoreController.InitiatePurchase(product);

                                UnityEngine.Console.Log("IAP",string.Format("Purchasing product asychronously: '{0}'", product.definition.id));

                            }
                            else
                            {
                               UnityEngine.Console.LogError("IAP","BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
                            }
                        }
                        else
                        {
                              UnityEngine.Console.LogError("IAP","BuyProductID FAIL. Not initialized.");
                        }
#endif
        }
        catch (Exception e)
        {
            UnityEngine.Console.LogError("IAP", "Error: " + e);
        }
    }

    public void AppsFlyerPurchaseEvent(Product product)
    {
        Dictionary<string, string> eventValue = new Dictionary<string, string>();
        eventValue.Add("af_content_id", product.definition.id);
        eventValue.Add("af_currency", product.metadata.isoCurrencyCode);

        float localizedPrice = (float)product.metadata.localizedPrice * StaticData.RateRev;

        string price = localizedPrice.ToString("0.#####", System.Globalization.CultureInfo.InvariantCulture);

        eventValue.Add("af_revenue", price);

        AppsFlyer.sendEvent("af_iap", eventValue);

        DebugProduct(product.definition.id, product.metadata.isoCurrencyCode, price, product.metadata.localizedPrice.ToString(),

        StaticData.RateRev.ToString(), ((float)product.metadata.localizedPrice).ToString());
    }

    public void DebugProduct(string text1, string text2, string text3, string text4, string text5, string text6)
    {
        UnityEngine.Console.Log("IAP", string.Format("Product: {0}\n{1}\n{2}\n{3}\n{4}\n{5}", text1, text2, text3, text4, text5, text6));
    }

    public void DisplayLoading(bool isShowed)
    {
        if (LoadingController.instance != null)
        {
            if (isShowed) LoadingController.instance.OnShow();
            else LoadingController.instance.OnHide();
        }
    }


    public void OnRestorePurchase()
    {
        if (!IsInitialized())
        {
            isRestore = false;
            UnityEngine.Console.LogError("IAP", "RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            UnityEngine.Console.Log("IAP", "RestorePurchases started ...");

            isRestore = true;

            ActionsPurchase.pass = null;

            ActionsPurchase.error = null;

            DisplayLoading(true);

            IAppleExtensions appleExtensions = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();

            appleExtensions.RestoreTransactions((success, message) =>
            {
                DisplayLoading(false);
                if (success)
                {
                    UnityEngine.Console.LogSuccess("IAP", "Transactions restored successfully.");
                }
                else
                {
                    UnityEngine.Console.LogError("IAP", $"Failed to restore transactions. Error: {message}");
                }
            });
        }
        else
        {
            UnityEngine.Console.Log("IAP", "RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }
}
