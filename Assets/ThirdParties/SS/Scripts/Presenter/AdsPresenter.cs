using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using Gley.MobileAds;

public class AdsPresenter : MonoBehaviour, IInitializable
{
    public AdsInfoData adsInfoData;

    AdsData m_AdsData;

    public string placementInter = "";
    public string placementReward = "";

    [SerializeField] GameObject m_ShieldAds;

    public void UpdateLastAdTime() => adsInfoData.UpdateLastAdTime();

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
        adsInfoData.adShowedCount++;
        return adsInfoData.adShowedCount;
    }

    public int UpdateAdInterCount()
    {
        adsInfoData.adInterCount++;
        return adsInfoData.adInterCount;
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
        if (adsInfoData.CanShowInterstitialAd())
        {
            SetActiveShield(true);
            placementInter = placement;
            this.SetDelay(0.25f, () =>
            {
                Gley.MobileAds.API.ShowInterstitial(UpdateLastAdTime);
                SetActiveShield(false);
            });
        }
    }

    public void ShowRewardedVideo(string placement, UnityAction onSuccess = null, UnityAction onFail = null, UnityAction onCompleted = null)
    {
        SetActiveShield(true);
        placementReward = placement;
        Gley.MobileAds.API.ShowRewardedVideo(CompleteMethod);

        void CompleteMethod(bool completed)
        {
            if (completed)
            {
                onSuccess?.Invoke();
                UpdateLastAdTime();
            }
            else
            {
                onFail?.Invoke();
            }

            onCompleted?.Invoke();
            SetActiveShield(false);
        }
    }

    public void ShowRewardedInterstitial(string placement, UnityAction onSuccess = null, UnityAction onFail = null, UnityAction onCompleted = null)
    {
        SetActiveShield(true);
        placementReward = placement;
        Gley.MobileAds.API.ShowRewardedInterstitial(VideoComplete);

        void VideoComplete(bool watched)
        {
            if (watched)
            {
                onSuccess?.Invoke();
                UpdateLastAdTime();
            }
            else
            {
                onFail?.Invoke();
            }

            onCompleted?.Invoke();
            SetActiveShield(false);
        }
    }

    void SetActiveShield(bool active)
    {
        if (active)
        {
            m_ShieldAds.gameObject.SetActive(true);
        }
        else
        {
            this.SetDelayNextFrame(() =>
            {
                m_ShieldAds.gameObject.SetActive(false);
            });
        }
    }
}
