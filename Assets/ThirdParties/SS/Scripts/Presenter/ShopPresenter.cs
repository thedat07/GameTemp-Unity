using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Directory;
using Gley.EasyIAP;

public class ShopPresenter : MonoBehaviour, IInitializable
{
    public const string Key = "ShopPresenter";

    public ItemDatabase itemDatabase;

    public SoDataRewards soDataRewards;

    public ItemData GetItem(MasterDataType type) => itemDatabase.GetItem(type);

    [SerializeField] GameObject m_ShieldShop;

    public void Initialize()
    {
        Gley.EasyIAP.API.Initialize(InitializationComplete);

        void InitializationComplete(IAPOperationStatus status, string message)
        {
            if (status == IAPOperationStatus.Success)
            {
                // IAP was successfully initialized.
                // For each non-consumable or subscription you should check if it is already bought.
                // If a product is active, means it was bought -> grand the access.
                // If remove ads was bought before, mark it as owned.
                ShopProductNames[] shopProductNames = new ShopProductNames[] { };
                foreach (var item in shopProductNames)
                {
                    if (API.IsActive(item))
                    {
                        if (API.GetValue(item).Any(x => x.GetDataType() == MasterDataType.NoAds))
                        {
                            GameManager.Instance.GetAdsPresenter().OnRemoveAds();
                        }
                    }
                }
            }
            else
            {
                Console.Log("IAP", "Error occurred: " + message);
            }
        }
    }

    public void BuyProduct(ShopProductNames shopProduct, UnityAction onSuccess = null, UnityAction onFail = null, UnityAction onCompleted = null)
    {
        SetActiveShield(true);

        Gley.EasyIAP.API.BuyProduct(shopProduct, ProductBought);

        void ProductBought(IAPOperationStatus status, string message, StoreProduct product)
        {
            if (status == IAPOperationStatus.Success)
            {
                ProductType productType = Gley.EasyIAP.API.GetProductType(shopProduct);
                if (productType == ProductType.Consumable)
                {
                    TypeConsumable();
                }
                else
                {
                    TypeNonConsumable();
                }
            }
            else
            {
                onFail?.Invoke();
            }

            onCompleted?.Invoke();

            SetActiveShield(false);

            void TypeConsumable()
            {
                SetVaule(product.value, "IAP");
                onSuccess?.Invoke();
                TigerForge.EventManager.EmitEvent(Key, 0.1f);
            }

            void TypeNonConsumable()
            {
                string productId = product.productName;
                if (!HasReceivedReward(productId))
                {
                    TypeConsumable();
                    MarkRewarded(productId);
                }
                else
                {
                    onFail?.Invoke();
                }
            }
        }
    }

    public void OnRestore()
    {
        SetActiveShield(true);
        Gley.EasyIAP.API.RestorePurchases(ProductRestoredCallback, RestoreDone);
    }

    // automatically called after one product is restored, is the same as the Buy Product callback
    private void ProductRestoredCallback(IAPOperationStatus status, string message, StoreProduct product)
    {
        if (status == IAPOperationStatus.Success)
        {
            ShopProductNames[] shopProductNames = new ShopProductNames[] { };
            foreach (var item in shopProductNames)
            {
                ShopProductNames productName = API.ConvertNameToShopProduct(product.productName);
                if (item == productName)
                {
                    string productId = product.productName;
                    if (!HasReceivedReward(productId))
                    {
                        SetVaule(product.value, "Restore");
                        MarkRewarded(productId);
                    }
                }
            }
            //consumable products are not restored
        }
        else
        {
            //an error occurred in the buying process, log the message for more details
            Console.Log("IAP", "Restore product failed: " + message);
        }
    }

    private void RestoreDone()
    {
        Console.Log("IAP", "Restore done");
        SetActiveShield(false);
    }

    void SetVaule(List<InventoryItem> data, string log = "")
    {
        DataMethod r = new DataMethod(Helper.Convert(data), log);
        r.Apply();
    }

    private bool HasReceivedReward(string productId)
    {
        return ES3.Load<bool>("IAP_Rewarded_" + productId, "IAPData", defaultValue: false);
    }

    private void MarkRewarded(string productId)
    {
        ES3.Save("IAP_Rewarded_" + productId, true, "IAPData");
    }

    void SetActiveShield(bool active)
    {
        if (active)
        {
            m_ShieldShop.gameObject.SetActive(true);
        }
        else
        {
            this.SetDelayNextFrame(() =>
            {
                TigerForge.EventManager.EmitEvent(Key);
                m_ShieldShop.gameObject.SetActive(false);
            });
        }
    }
}
