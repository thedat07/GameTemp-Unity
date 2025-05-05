//using System;
//using UnityEngine;
//using GoogleMobileAds.Api;
//using GoogleMobileAds.Common;
//using UnityEngine.Events;
//using AppsFlyerSDK;
//using System.Collections.Generic;
//using Newtonsoft.Json.Linq;

//public class AppOpenAdController : MonoBehaviour
//{
//    public UnityEvent actionStartGame;


//#if UNITY_ANDROID
//    private const string _adUnitId = "ca-app-pub-3592666561057448/6332358106";
//#elif UNITY_IPHONE
//        private const string _adUnitId = "ca-app-pub-3592666561057448/7047578903";
//#else
//        private const string _adUnitId = "unused";
//#endif

//    // App open ads can be preloaded for up to 4 hours.
//    private readonly TimeSpan TIMEOUT = TimeSpan.FromHours(4);
//    private DateTime _expireTime;
//    private AppOpenAd _appOpenAd;

//    private bool showFirstTime = false;

//    private int amountOpen;

//    private void Awake()
//    {
//        // Use the AppStateEventNotifier to listen to application open/close events.
//        // This is used to launch the loaded ad when we open the app.
//        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
//    }

//    private void OnDestroy()
//    {
//        // Always unlisten to events when complete.
//        AppStateEventNotifier.AppStateChanged -= OnAppStateChanged;
//    }

//    public void Init()
//    {
//        amountOpen = PlayerPrefs.GetInt("Amount_Open", 0);
//        amountOpen++;
//        PlayerPrefs.SetInt("Amount_Open", amountOpen);

//        if (ADBridge.instance.isRemoveAds) return;

//        // Initialize the Google Mobile Ads SDK.
//        MobileAds.Initialize((InitializationStatus initStatus) =>
//        {
//            LoadAd();
//        });
//    }
//    /// <summary>
//    /// Loads the ad.
//    /// </summary>
//    public void LoadAd()
//    {
//        // Clean up the old ad before loading a new one.
//        if (_appOpenAd != null)
//        {
//            DestroyAd();
//        }

//        Debug.Log("Loading app open ad.");

//        // Create our request used to load the ad.
//        var adRequest = new AdRequest.Builder().Build();

//        // Send the request to load the ad.
//        AppOpenAd.Load(_adUnitId,adRequest, (AppOpenAd ad, LoadAdError error) =>
//        {
//            // If the operation failed with a reason.
//            if (error != null)
//            {
//                Debug.LogError("App open ad failed to load an ad with error : "
//                                + error.GetMessage());

//                actionStartGame?.Invoke();
//                return;
//            }

//            // If the operation failed for unknown reasons.
//            // This is an unexpected error, please report this bug if it happens.
//            if (ad == null)
//            {
//                Debug.LogError("Unexpected error: App open ad load event fired with " +
//                               " null ad and null error.");

//                actionStartGame?.Invoke();
//                return;
//            }

//            // The operation completed successfully.
//            Debug.Log("App open ad loaded with response : " + ad.GetResponseInfo());
//            _appOpenAd = ad;

//            // App open ads can be preloaded for up to 4 hours.
//            _expireTime = DateTime.Now + TIMEOUT;

//            // Register to ad events to extend functionality.
//            RegisterEventHandlers(ad);

//            if (!showFirstTime)
//            {
//                DoShow();

//                showFirstTime = true;
//            }

//        });
//    }

//    private void DoShow() 
//    {
//        if (ADBridge.instance.isRemoveAds || amountOpen < GameServiceController.instance.configController.aoaStartSessionShow || GameServiceController.instance.configController.isEnableAoa)
//        {

//            actionStartGame?.Invoke();

//            return;
//        }

//        if (_appOpenAd != null && DateTime.Now < _expireTime)
//        {
//            Debug.Log("Showing app open ad.");
//            ADBridge.instance.maxAds.resumeFromAds = true;
//            _appOpenAd.Show();
//        }
//        else
//        {
//            Debug.LogError("App open ad is not ready yet.");

//            actionStartGame?.Invoke();
//        }
//    }

//    /// <summary>
//    /// Shows the ad.
//    /// </summary>
//    public void ShowAd()
//    {
//        if (Time.time - ADBridge.instance.lastTimeShowAd >= ADBridge.instance.maxAds.cappingAoaFromInter)
//        {
//            if (!ADBridge.instance.maxAds.resumeFromAds)
//            {
//                // App open ads can be preloaded for up to 4 hours.
//                DoShow();
//            }
//        }
        

//    }

//    /// <summary>
//    /// Destroys the ad.
//    /// </summary>
//    public void DestroyAd()
//    {
//        if (_appOpenAd != null)
//        {
//            Debug.Log("Destroying app open ad.");
//            _appOpenAd.Destroy();
//            _appOpenAd = null;
//        }

//    }

//    /// <summary>
//    /// Logs the ResponseInfo.
//    /// </summary>
//    public void LogResponseInfo()
//    {
//        if (_appOpenAd != null)
//        {
//            var responseInfo = _appOpenAd.GetResponseInfo();
//            UnityEngine.Debug.Log(responseInfo);
//        }
//    }

//    private void OnAppStateChanged(AppState state)
//    {
//        Debug.Log("App State changed to : " + state);

//        // If the app is Foregrounded and the ad is available, show it.
//        if (state == AppState.Foreground)
//        {
//            ShowAd();
//        }
//    }

//    private void RegisterEventHandlers(AppOpenAd ad)
//    {
//        // Raised when the ad is estimated to have earned money.
//        ad.OnAdPaid += OnPaidEvent;

//        // Raised when the ad closed full screen content.
//        ad.OnAdFullScreenContentClosed += OnAdDidDismissFullScreenContent;
//        // Raised when the ad failed to open full screen content.
//        ad.OnAdFullScreenContentFailed += OnAdFailedToPresentFullScreenContent;
//    }

//    private void OnAdFailedToPresentFullScreenContent(AdError e) 
//    {

//        actionStartGame?.Invoke();

//        ADBridge.instance.maxAds.resumeFromAds = false;
//    }

//    private void OnPaidEvent(AdValue e)
//    {

//        TrackAdRevenue(e);
//    }

//    private void OnAdDidDismissFullScreenContent()
//    {

//        actionStartGame?.Invoke();

//        LoadAd();

//        ADBridge.instance.maxAds.resumeFromAds = false;

//        ADBridge.instance.lastTimeShowAd = Time.time;
//    }

//    private void TrackAdRevenue(AdValue adValue)
//    {
//        var rev = adValue.Value / 1000000;

//        var impressionParameters = new[]
//        {
//            new Firebase.Analytics.Parameter("ad_platform", "Admob"),
//            new Firebase.Analytics.Parameter("ad_source", "Admob"),
//            new Firebase.Analytics.Parameter("ad_unit_name", "AOA"),
//            new Firebase.Analytics.Parameter("ad_format", "AOA"),
//            new Firebase.Analytics.Parameter("value", rev),
//            new Firebase.Analytics.Parameter("currency", adValue.CurrencyCode), // All Applovin revenue is sent in USD
//        };
//        Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_impression", impressionParameters);
//        Firebase.Analytics.FirebaseAnalytics.LogEvent("ad_cgteam_impression", impressionParameters);

//        //var eventParams = new Dictionary<string, string>();
//        //eventParams.Add("ad_platform", "Admob");
//        //eventParams.Add("ad_source", "Admob");
//        //eventParams.Add("ad_unit_name", "AOA");
//        //eventParams.Add("ad_format", "AOA");
//        //eventParams.Add("value", rev.ToString("0.0000"));
//        //eventParams.Add("currency", adValue.CurrencyCode);
//        //AppsFlyer.sendEvent("af_ad_revenue", eventParams);

//        //AppsFlyerAdRevenue.logAdRevenue("Admob", AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeApplovinMax, rev, "USD", eventParams);

//        Debug.Log("Value: " + rev);
//        double currentImpressionRevenue = rev;
//        float previousTaichiTroasCache = PlayerPrefs.GetFloat("TaichiTroasCache", 0); //Use App Local storage to store cache of tROAS

//        Debug.Log("Value previousTaichiTroasCache: " + previousTaichiTroasCache);
//        float currentTaichiTroasCache = (float)(previousTaichiTroasCache + currentImpressionRevenue);
//        Debug.Log("Value currentTaichiTroasCache: " + currentTaichiTroasCache);

//        if (currentTaichiTroasCache >= 0.02f)
//        { //thresshold
//            LogTaichiTroasFirebaseAdRevenueEvent(currentTaichiTroasCache, adValue.CurrencyCode);
//            PlayerPrefs.SetFloat("TaichiTroasCache", 0);//reset TaichiTroasCache
//        }
//        else
//        {
//            PlayerPrefs.SetFloat("TaichiTroasCache", currentTaichiTroasCache);
//        }
//    }

//    private void LogTaichiTroasFirebaseAdRevenueEvent(float TaichiTroasCache, string currencyCode)
//    {
//        Debug.Log("Value Log Taichi: " + TaichiTroasCache);
//        var impressionParameters = new[]
//{
//            new Firebase.Analytics.Parameter("value", TaichiTroasCache), //(Required)tROAS event must include Double Value
//            new Firebase.Analytics.Parameter("currency", currencyCode), // All Applovin revenue is sent in USD
//        };
//        Firebase.Analytics.FirebaseAnalytics.LogEvent("Total_Ads_Revenue", impressionParameters);
//        string threshold = "0.02";
//        threshold = threshold.Replace('.', '_');
//        Firebase.Analytics.FirebaseAnalytics.LogEvent("ads_revenue_" + threshold, impressionParameters);
//    }
//}