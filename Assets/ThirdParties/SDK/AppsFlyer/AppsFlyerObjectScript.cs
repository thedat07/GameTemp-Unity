using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppsFlyerSDK;
using AppsFlyerConnector;

using UnityEngine;
using System.Runtime.InteropServices;

#if UNITY_IOS

namespace AudienceNetwork
{
    public static class AdSettings
    {
        [DllImport("__Internal")]
        private static extern void FBAdSettingsBridgeSetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled);

        public static void SetAdvertiserTrackingEnabled(bool advertiserTrackingEnabled)
        {
            FBAdSettingsBridgeSetAdvertiserTrackingEnabled(advertiserTrackingEnabled);
        }
    }
}

#endif
// This class is intended to be used the the AppsFlyerObject.prefab

public class AppsFlyerObjectScript : MonoBehaviour, IAppsFlyerConversionData
{

    // These fields are set from the editor so do not modify!
    //******************************//
    public string devKey;
    public string appID;
    public string UWPAppID;
    public string macOSAppID;
    public bool isDebug;
    public bool getConversionData;
    //******************************//


    void Start()
    {
        // These fields are set from the editor so do not modify!
        //******************************//
        AppsFlyer.setIsDebug(isDebug);
#if UNITY_WSA_10_0 && !UNITY_EDITOR
        AppsFlyer.initSDK(devKey, UWPAppID, getConversionData ? this : null);
#elif UNITY_STANDALONE_OSX && !UNITY_EDITOR
    AppsFlyer.initSDK(devKey, macOSAppID, getConversionData ? this : null);
#else
        AppsFlyer.initSDK(devKey, appID, getConversionData ? this : null);
#endif
        //******************************/

#if UNITY_IOS && !UNITY_EDITOR
        AppsFlyer.waitForATTUserAuthorizationWithTimeoutInterval(60);
#endif

        AppsFlyerPurchaseConnector.init(this, AppsFlyerConnector.Store.GOOGLE);
        AppsFlyerPurchaseConnector.setAutoLogPurchaseRevenue(AppsFlyerAutoLogPurchaseRevenueOptions.AppsFlyerAutoLogPurchaseRevenueOptionsAutoRenewableSubscriptions, AppsFlyerAutoLogPurchaseRevenueOptions.AppsFlyerAutoLogPurchaseRevenueOptionsInAppPurchases);
        AppsFlyerPurchaseConnector.setPurchaseRevenueValidationListeners(true);
        AppsFlyerPurchaseConnector.build();
        AppsFlyerPurchaseConnector.startObservingTransactions();
        AppsFlyer.startSDK();

        AppsFlyerAdRevenue.start();

#if UNITY_IOS

        AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(true);
#endif
    }
    public void didReceivePurchaseRevenueValidationInfo(string validationInfo)
    {
        AppsFlyer.AFLog("didReceivePurchaseRevenueValidationInfo", validationInfo);
        // deserialize the string as a dictionnary, easy to manipulate
        Dictionary<string, object> dictionary = AFMiniJSON.Json.Deserialize(validationInfo) as Dictionary<string, object>;

        // if the platform is Android, you can create an object from the dictionnary 
#if UNITY_ANDROID
        if (dictionary.ContainsKey("productPurchase") && dictionary["productPurchase"] != null)
        {
            // Create an object from the JSON string.
            InAppPurchaseValidationResult iapObject = JsonUtility.FromJson<InAppPurchaseValidationResult>(validationInfo);
        }
        else if (dictionary.ContainsKey("subscriptionPurchase") && dictionary["subscriptionPurchase"] != null)
        {
            SubscriptionValidationResult iapObject = JsonUtility.FromJson<SubscriptionValidationResult>(validationInfo);

        }
#endif
    }

    // Mark AppsFlyer CallBacks
    public void onConversionDataSuccess(string conversionData)
    {
        AppsFlyer.AFLog("didReceiveConversionData", conversionData);
        Dictionary<string, object> conversionDataDictionary = AppsFlyer.CallbackStringToDictionary(conversionData);
        // add deferred deeplink logic here
    }

    public void onConversionDataFail(string error)
    {
        AppsFlyer.AFLog("didReceiveConversionDataWithError", error);
    }

    public void onAppOpenAttribution(string attributionData)
    {
        AppsFlyer.AFLog("onAppOpenAttribution", attributionData);
        Dictionary<string, object> attributionDataDictionary = AppsFlyer.CallbackStringToDictionary(attributionData);
        // add direct deeplink logic here
    }

    public void onAppOpenAttributionFailure(string error)
    {
        AppsFlyer.AFLog("onAppOpenAttributionFailure", error);
    }

}
