using AppsFlyerSDK;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MaxManager : MonoBehaviour
{
//     public AdsConfigData adsConfigData;

//     public AdsConfigData GetAdsConfig()
//     {
// #if HARD
//         return adsConfigDataHard;
// #else
//         return adsConfigData;
// #endif
//     }

//     private UnityEvent callbackVideoEarn = null;
//     private UnityEvent callbackVideoFail = null;

//     private UnityEvent callbackVideoInter = null;


//     // Start is called before the first frame update
//     public void Init()
//     {
//         MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
//         {
//             // AppLovin SDK is initialized, configure and start loading ads.
//             UnityEngine.Console.Log("Max", "MAX SDK Initialized");

//             InitializeBannerAds();
//             InitializeInterstitialAds();
//             InitializeRewardedAds();

//             UnityEngine.Console.Log("Max", "MAX SDK Initialized Done");
//         };
//         MaxSdk.SetSdkKey(GetAdsConfig().sdkKey);
//         MaxSdk.InitializeSdk();
//     }

//     private void LogAdCountEvent(MaxSdkBase.AdInfo impressionData)
//     {
//         var count = GameManager.Instance.GetAdsPresenter().UpdateAdShowedCount();

//         var listCountAvailable = new List<int> { 3, 5, 7, 10, 12, 15, 20, 25, 30 };

//         foreach (var item in listCountAvailable)
//         {
//             if (count % item == 0)
//             {
//                 var impressionParameters = new[]
//                 {
//                     new Firebase.Analytics.Parameter("ad_platform", "AppLovin"),
//                     new Firebase.Analytics.Parameter("ad_source", impressionData.NetworkName),
//                     new Firebase.Analytics.Parameter("ad_unit_name", impressionData.AdUnitIdentifier),
//                     new Firebase.Analytics.Parameter("ad_format", impressionData.AdFormat),
//                     new Firebase.Analytics.Parameter("value", impressionData.Revenue),
//                     new Firebase.Analytics.Parameter("currency", "USD"), // All Applovin revenue is sent in USD
//                 };

//                 var eventName = "watch_ads_" + item;

//                 FirebaseEvent.LogAnalytics(eventName, impressionParameters);

//                 FacebookController.instance.LogEvent(eventName);
//             }
//         }
//     }

//     private void LogAdCampaignInter()
//     {
//         var count = GameManager.Instance.GetAdsPresenter().UpdateAdInterCount();

//         var listCountAvailableForEvent = new List<int> { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70 };

//         if (listCountAvailableForEvent.Contains(count))
//         {
//             var eventName = "Inter_view_" + count;
//             FirebaseEvent.LogEvent(eventName);
//             Dictionary<string, string> eventValue = new Dictionary<string, string>();
//             eventValue.Add(eventName, eventName);
//             AppsFlyer.sendEvent(eventName, eventValue);
//         }
//     }

//     private void LogAdCampaignVideo()
//     {
//         var count = GameManager.Instance.GetAdsPresenter().UpdateAdInterCount();

//         var listCountAvailableForEvent = new List<int> { 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 55, 60, 65, 70 };

//         if (listCountAvailableForEvent.Contains(count))
//         {
//             var eventName = "Rw_view_" + count;
//             FirebaseEvent.LogEvent(eventName);
//             Dictionary<string, string> eventValue = new Dictionary<string, string>();
//             eventValue.Add(eventName, eventName);
//             AppsFlyer.sendEvent(eventName, eventValue);
//         }
//     }


//     #region Banner
//     [SerializeField]
//     string bannerAdUnitId => GetAdsConfig().bannerId.GetID(); // Retrieve the ID from your account


//     public void InitializeBannerAds()
//     {
//         MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
//         MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
//         MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
//         MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
//         MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
//         MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;

//         MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);

//         MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, GetAdsConfig().bannerBackgroundColor);
//         MaxSdk.SetBannerExtraParameter(bannerAdUnitId, "adaptive_banner", "false");
//     }

//     private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//     {

//     }

//     private void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
//     {
//         UnityEngine.Console.Log("Max", "Error: " + errorInfo.AdLoadFailureInfo);
//     }

//     private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

//     private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//     {
//         UnityEngine.Console.Log("Max", "Banner ad revenue paid");

//         TrackAdRevenue(adInfo);
//     }

//     private void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

//     private void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

//     private bool m_IsBannerShowing;

//     public void ShowBanner()
//     {
//         if (m_IsBannerShowing) return;

//         if (GameManager.Instance.GetAdsData().isRemoveAds == false)
//         {
//             m_IsBannerShowing = true;
//             MaxSdk.ShowBanner(bannerAdUnitId);
//         }
//     }

//     public void HideBanner()
//     {
//         MaxSdk.HideBanner(bannerAdUnitId);
//         m_IsBannerShowing = false;
//     }

//     #endregion

//     #region Interials Ads
//     [SerializeField]
//     string adInterUnitId => GetAdsConfig().interId.GetID();
//     int retryAttemptInter;
//     string placementInter = "";

//     public void InitializeInterstitialAds()
//     {
//         // Attach callback
//         MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
//         MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
//         MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialRevenuePaidEvent;
//         MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
//         MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
//         MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;
//         MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += HandleOnInterstitialDisplayedEvent;

//         // Load the first interstitial
//         LoadInterstitial();
//     }

//     private void LoadInterstitial()
//     {
//         if (GameManager.Instance.GetAdsData().isRemoveAds) return;

//         MaxSdk.LoadInterstitial(adInterUnitId);
//     }

//     private void OnInterstitialRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//     {
//         UnityEngine.Console.Log("Max", "Interstitial revenue paid");

//         TrackAdRevenue(adInfo);
//     }

//     private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//     {
//         UnityEngine.Console.Log("Max", "Inter Loaded");

//         retryAttemptInter = 0;
//         FirebaseEvent.LogEvent("ads_inter_load");
//     }

//     private void HandleOnInterstitialDisplayedEvent(string adId, MaxSdkBase.AdInfo adInfo)
//     {
//         FirebaseEvent.LogEvent("ads_inter_show", "placement", this.placementInter);
//         FirebaseEvent.LogEvent("af_inters");
//         AppsFlyer.sendEvent("af_inters", null);
//         LogAdCountEvent(adInfo);

//         LogAdCampaignInter();
//     }

//     private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
//     {
//         retryAttemptInter++;
//         double retryDelay = Math.Pow(2, Math.Min(6, retryAttemptInter));
//         Invoke("LoadInterstitial", (float)retryDelay);
//         FirebaseEvent.LogEvent("ads_inter_fail", "placement", placementInter, "error_message", $"error_code_{errorInfo.Code}");
//     }

//     private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
//     {
//         InvokeInterstitialCallbackOnMainThread();
//         FirebaseEvent.LogEvent("ads_inter_fail", "placement", placementInter, "error_message", $"error_code_{errorInfo.Code}");
//     }

//     private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//     {
//         FirebaseEvent.LogEvent("ads_inter_click", "placement", placementInter);
//     }

//     private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//     {
//         UnityEngine.Console.Log("Max", "Ads Hidden");
//         GameManager.Instance.GetAdsPresenter().adsDataNotSave.UpdateLastAdTime();
//         // Interstitial ad is hidden. Pre-load the next ad.
//         InvokeInterstitialCallbackOnMainThread();
//     }

//     public void ShowInterAds(string placement, UnityEvent callbackInter = null)
//     {
//         FirebaseEvent.LogEvent("inter_attempt");
//         this.placementInter = placement;
//         if (MaxSdk.IsInterstitialReady(adInterUnitId))
//         {
//             UnityEngine.Console.Log("Max", "Show inter");
//             MaxSdk.ShowInterstitial(adInterUnitId, placement);
//             this.callbackVideoInter = callbackInter;
//         }
//         else
//         {
//             LoadInterstitial();
//             if (callbackInter != null)
//             {
//                 callbackInter.Invoke();
//             }
//         }
//     }

//     private void InvokeInterstitialCallbackOnMainThread()
//     {
//         StartCoroutine(InvokeAction());
//     }

//     private IEnumerator InvokeAction()
//     {
//         if (callbackVideoInter != null)
//         {
//             callbackVideoInter?.Invoke();
//             callbackVideoInter = null;
//         }
//         yield return null;

//         LoadInterstitial();
//     }
//     #endregion

//     #region Reward Ads
//     [SerializeField]
//     string adRewardUnitId => GetAdsConfig().rewardedId.GetID();
//     int retryAttemptReward;
//     string placementReward = "";

//     public void InitializeRewardedAds()
//     {
//         // Attach callback
//         MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
//         MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
//         MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
//         MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
//         MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
//         MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
//         MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
//         MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;

//         // Load the first rewarded ad
//         LoadRewardedAd();
//     }

//     private void LoadRewardedAd()
//     {
//         MaxSdk.LoadRewardedAd(adRewardUnitId);
//     }

//     private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//     {
//         UnityEngine.Console.Log("Max", "Rewarded ad loaded");

//         retryAttemptReward = 0;
//         FirebaseEvent.LogEvent("ads_reward_load");
//     }

//     private void OnRewardedAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
//     {
//         // Rewarded ad failed to load 
//         // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

//         retryAttemptReward++;
//         double retryDelay = Math.Pow(2, Math.Min(6, retryAttemptReward));
//         FirebaseEvent.LogEvent("ads_reward_fail", "placement", placementReward, "error_message", $"error_code_{errorInfo.Code}");
//         Invoke("LoadRewardedAd", (float)retryDelay);
//     }

//     private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//     {
//         FirebaseEvent.LogEvent("ads_reward_show", "placement", this.placementReward);
//         Dictionary<string, string> eventValues = new Dictionary<string, string>();
//         eventValues.Add("reward_type", placementReward);
//         AppsFlyer.sendEvent("af_rewarded", eventValues);

//         LogAdCountEvent(adInfo);

//         LogAdCampaignVideo();
//     }

//     private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
//     {
//         LoadRewardedAd();
//         FirebaseEvent.LogEvent("ads_reward_fail", "placement", placementReward, "error_message", $"error_code_{errorInfo.Code}");
//     }

//     private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//     {
//         FirebaseEvent.LogEvent("ads_reward_click", "placement", placementReward);
//     }

//     private void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//     {
//         UnityEngine.Console.Log("Max", "Ads Reward Hidden");
//         // Rewarded ad is hidden. Pre-load the next ad
//         LoadRewardedAd();
//     }

//     private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
//     {
//         UnityEngine.Console.Log("Max", "Ads Reward Earn");
//         FirebaseEvent.LogEventReward(placementReward);
//         // The rewarded ad displayed and the user should receive the reward.
//         InvokeRewardedVideoCallbackOnMainThread();

//         GameManager.Instance.GetQuestPresenter().SetData(1, MissionType.Ads);
//     }

//     private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
//     {
//         TrackAdRevenue(adInfo);
//     }

//     public void ShowRewardAds(string placement, UnityEvent callbackEarn = null, UnityEvent callbackFail = null)
//     {
//         UnityEngine.Console.Log("Max", "Can Show Reward: " + MaxSdk.IsRewardedAdReady(adRewardUnitId));
//         FirebaseEvent.LogEvent("reward_attempt");
//         this.placementReward = placement;
//         if (MaxSdk.IsRewardedAdReady(adRewardUnitId))
//         {
//             UnityEngine.Console.Log("Max", "Show reward");
//             MaxSdk.ShowRewardedAd(adRewardUnitId, placement);
//             this.callbackVideoEarn = callbackEarn;
//             this.callbackVideoFail = callbackFail;
//         }
//         else
//         {
//             LoadRewardedAd();
//         }
//     }


//     private void InvokeRewardedVideoCallbackOnMainThread()
//     {
//         if (callbackVideoEarn != null)
//         {
//             callbackVideoEarn?.Invoke();
//             callbackVideoEarn = null;
//             callbackVideoFail = null;
//         }
//     }

//     #endregion

//     private void TrackAdRevenue(MaxSdkBase.AdInfo impressionData)
//     {
//         var impressionParameters = new[]
//         {
//             new Firebase.Analytics.Parameter("ad_platform", "AppLovin"),
//             new Firebase.Analytics.Parameter("ad_source", impressionData.NetworkName),
//             new Firebase.Analytics.Parameter("ad_unit_name", impressionData.AdUnitIdentifier),
//             new Firebase.Analytics.Parameter("ad_format", impressionData.AdFormat),
//             new Firebase.Analytics.Parameter("value", impressionData.Revenue),
//             new Firebase.Analytics.Parameter("currency", "USD"), // All Applovin revenue is sent in USD
//         };
//         FirebaseEvent.LogAnalytics("ad_impression", impressionParameters);
//         FirebaseEvent.LogAnalytics("ad_cgteam_impression", impressionParameters);
//         var eventParams = new Dictionary<string, string>();
//         eventParams.Add("ad_platform", "AppLovin");
//         eventParams.Add("ad_source", impressionData.NetworkName);
//         eventParams.Add("ad_unit_name", impressionData.AdUnitIdentifier);
//         eventParams.Add("ad_format", impressionData.AdFormat);
//         eventParams.Add("value", impressionData.Revenue.ToString("0.0000"));
//         eventParams.Add("currency", "USD");

//         AppsFlyerAdRevenue.logAdRevenue(impressionData.NetworkName, AppsFlyerAdRevenueMediationNetworkType.AppsFlyerAdRevenueMediationNetworkTypeApplovinMax, impressionData.Revenue, "USD", eventParams);

//         UnityEngine.Console.Log("Max", "Value: " + impressionData.Revenue);
//         double currentImpressionRevenue = impressionData.Revenue;
//         float previousTaichiTroasCache = PlayerPrefs.GetFloat("TaichiTroasCache", 0); //Use App Local storage to store cache of tROAS

//         UnityEngine.Console.Log("Max", "Value previousTaichiTroasCache: " + previousTaichiTroasCache);
//         float currentTaichiTroasCache = (float)(previousTaichiTroasCache + currentImpressionRevenue);
//         UnityEngine.Console.Log("Max", "Value currentTaichiTroasCache: " + currentTaichiTroasCache);

//         if (currentTaichiTroasCache >= 0.02f)
//         { //thresshold
//             LogTaichiTroasFirebaseAdRevenueEvent(currentTaichiTroasCache);
//             PlayerPrefs.SetFloat("TaichiTroasCache", 0);//reset TaichiTroasCache
//         }
//         else
//         {
//             PlayerPrefs.SetFloat("TaichiTroasCache", currentTaichiTroasCache);
//         }
//     }

//     private void LogTaichiTroasFirebaseAdRevenueEvent(float TaichiTroasCache)
//     {
//         UnityEngine.Console.Log("Max", "Value Log Taichi: " + TaichiTroasCache);
//         var impressionParameters = new[]
//         {
//             new Firebase.Analytics.Parameter("value", TaichiTroasCache), //(Required)tROAS event must include Double Value
//             new Firebase.Analytics.Parameter("currency", "USD"), // All Applovin revenue is sent in USD
//         };
//         FirebaseEvent.LogAnalytics("Total_Ads_Revenue", impressionParameters);
//         string threshold = "0.02";
//         threshold = threshold.Replace('.', '_');
//         FirebaseEvent.LogAnalytics("ads_revenue_" + threshold, impressionParameters);
//     }
}

