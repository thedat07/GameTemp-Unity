using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Gley.MobileAds;

public class AdsPresenter : MonoBehaviour, IInitializable
{
    public AdsDataNotSave adsDataNotSave;

    AdsData m_AdsData;

    public string placementInter = "";
    public string placementReward = "";

    public void Initialize()
    {
        m_AdsData = GameManager.Instance.GetAdsData();
        Gley.MobileAds.API.Initialize(OnInitialized);
        void OnInitialized()
        {
            //Show ads only after this method is called
            //This callback is not mandatory if you do not want to show banners as soon as your app starts.
            ShawBanner();
        }
    }

    public void ShowMediationDebugger()
    {
        MaxSdk.ShowMediationDebugger();
    }

    public int UpdateAdShowedCount()
    {
        adsDataNotSave.adShowedCount++;
        return adsDataNotSave.adShowedCount;
    }

    public int UpdateAdInterCount()
    {
        adsDataNotSave.adInterCount++;
        return adsDataNotSave.adInterCount;
    }

    [ContextMenu("Remove Ads")]
    public void OnRemoveAds()
    {
        Gley.MobileAds.API.RemoveAds(true);
        TigerForge.EventManager.EmitEvent(AdsData.Key);
    }

    public void ShawBanner()
    {
        Gley.MobileAds.API.ShowBanner(BannerPosition.Bottom, BannerType.Banner);
    }

    public void HideBanner()
    {
        Gley.MobileAds.API.HideBanner();
    }

    public void ShowInterstitial(string placement)
    {
        if (adsDataNotSave.CanShowInterstitialAd())
        {
            placementInter = placement;
            Gley.MobileAds.API.ShowInterstitial();
        }
    }

    public void ShowRewardedVideo(string placement, UnityAction onSuccess = null, UnityAction onFail = null, UnityAction onCompleted = null)
    {
        placementReward = placement;
        Gley.MobileAds.API.ShowRewardedVideo(CompleteMethod);

        void CompleteMethod(bool completed)
        {
            if (completed)
            {
                onSuccess?.Invoke();
                adsDataNotSave.UpdateLastAdTime();
            }
            else
            {
                onFail?.Invoke();
            }

            onCompleted?.Invoke();
        }
    }

    public void ShowRewardedInterstitial(string placement, UnityAction onSuccess = null, UnityAction onFail = null, UnityAction onCompleted = null)
    {
        placementReward = placement;
        Gley.MobileAds.API.ShowRewardedInterstitial(VideoComplete);

        void VideoComplete(bool watched)
        {
            if (watched)
            {
                onSuccess?.Invoke();
                adsDataNotSave.UpdateLastAdTime();
            }
            else
            {
                onFail?.Invoke();
            }

            onCompleted?.Invoke();
        }
    }
}
