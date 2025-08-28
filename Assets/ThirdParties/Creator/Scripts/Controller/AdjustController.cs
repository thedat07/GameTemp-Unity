using UnityEngine;
using AdjustSdk;
using UnityEngine.Purchasing;
using System.Collections.Generic;


public class AdjustController : MonoBehaviour
{
    public string appToken;

    public void Init()
    {

#if UNITY_EDITOR
    AdjustConfig adjustConfig = new AdjustConfig(appToken, AdjustEnvironment.Sandbox);
#else
    AdjustConfig adjustConfig = new AdjustConfig(appToken, AdjustEnvironment.Production);
#endif

        // Log level (dev dùng Verbose, production nên Warning hoặc None)
        adjustConfig.LogLevel = AdjustLogLevel.Verbose;

        // Gửi event khi chạy nền
        adjustConfig.IsSendingInBackgroundEnabled = true;

        // Attribution callback
        adjustConfig.AttributionChangedDelegate = (attribution) =>
        {
            Console.Log("Adjus", "Attribution: " + attribution);
        };

        // Deferred deeplink callback
        adjustConfig.DeferredDeeplinkDelegate = (deeplinkUrl) =>
        {
            Console.Log("Adjus", "Deeplink: " + deeplinkUrl);
        };

        // Init Adjust SDK
        Adjust.InitSdk(adjustConfig);
    }
}

public static class AnalyticsTracker
{
    private const string EVENT_IAP = "af_iap"; // hoặc token bạn setup trên Adjust dashboard

    public static void TrackIAPWithVerify(Product product)
    {
        if (product == null) return;

        double localizedPrice = (double)product.metadata.localizedPrice * StaticData.RateRev;
        string currency = product.metadata.isoCurrencyCode;

        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            AdjustEvent adjustEvent = new AdjustEvent(EVENT_IAP);
            adjustEvent.SetRevenue(localizedPrice, currency);
            adjustEvent.TransactionId = product.transactionID;
            adjustEvent.ProductId = product.definition.id;

            Adjust.VerifyAndTrackAppStorePurchase(adjustEvent, verificationResult =>
            {
                Console.Log("Adjus", "iOS Verify status: " + verificationResult.VerificationStatus);
                Console.Log("Adjus", "Code: " + verificationResult.Code);
                Console.Log("Adjus", "Message: " + verificationResult.Message);
            });
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            string purchaseToken = ExtractPurchaseToken(product);

            AdjustEvent adjustEvent = new AdjustEvent(EVENT_IAP);
            adjustEvent.SetRevenue(localizedPrice, currency);
            adjustEvent.ProductId = product.definition.id;

            if (!string.IsNullOrEmpty(purchaseToken))
                adjustEvent.PurchaseToken = purchaseToken;

            Adjust.VerifyAndTrackPlayStorePurchase(adjustEvent, verificationResult =>
            {
                Console.Log("Adjus", "Android Verify status: " + verificationResult.VerificationStatus);
                Console.Log("Adjus", "Code: " + verificationResult.Code);
                Console.Log("Adjus", "Message: " + verificationResult.Message);
            });
        }
        else
        {
            Console.LogWarning("Adjus", "Platform không hỗ trợ IAP verify");
        }
    }

    private static string ExtractPurchaseToken(Product product)
    {
        try
        {
            var receiptWrapper = MiniJson.JsonDecode(product.receipt) as Dictionary<string, object>;
            if (receiptWrapper != null && receiptWrapper.ContainsKey("Payload"))
            {
                var payload = MiniJson.JsonDecode(receiptWrapper["Payload"].ToString()) as Dictionary<string, object>;
                if (payload != null && payload.ContainsKey("json"))
                {
                    var originalJson = MiniJson.JsonDecode(payload["json"].ToString()) as Dictionary<string, object>;
                    if (originalJson != null && originalJson.ContainsKey("purchaseToken"))
                    {
                        return originalJson["purchaseToken"].ToString();
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("ExtractPurchaseToken error: " + e.Message);
        }
        return null;
    }
}
