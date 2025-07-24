using UnityEngine;
using UnityEngine.Events;
using Gley.MobileAds;

public class AdsModelView : MonoBehaviour, IInitializable
{
    public AdsInfo adsInfoData;

    AdsModel m_Model;

    public string placementInter = "";
    public string placementReward = "";

    [SerializeField] GameObject m_ShieldAds;

    public void UpdateLastAdTime(bool isRewardedAd) => adsInfoData.UpdateLastAdTime(isRewardedAd);

    public void Initialize()
    {
        m_Model = GameManager.Instance.GetAdsData();
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
        m_Model.adShowedCount++;
        return m_Model.adShowedCount;
    }

    public int UpdateAdInterCount()
    {
        adsInfoData.adInterAds++;
        
        m_Model.adInterCount++;
        return m_Model.adInterCount;
    }

    [ContextMenu("Remove Ads")]
    public void OnRemoveAds()
    {
        Gley.MobileAds.API.RemoveAds(true);
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

            this.SetDelay(0.5f, () =>
            {
                SetActiveShield(false);
                Gley.MobileAds.API.ShowInterstitial(() => { UpdateLastAdTime(false); });
            });
        }
    }

    public void ShowInterstitial(string placement, UnityAction interstitialClosed)
    {
        if (adsInfoData.CanShowInterstitialAd())
        {
            SetActiveShield(true);

            placementInter = placement;

            this.SetDelay(0.5f, () =>
            {
                SetActiveShield(false);
                Gley.MobileAds.API.ShowInterstitial(() =>
                {
                    UpdateLastAdTime(false);
                    interstitialClosed?.Invoke();
                });
            });
        }
    }

    public void ShowRewardedVideo(string placement, UnityAction onSuccess = null, UnityAction onFail = null, UnityAction onCompleted = null)
    {
        SetActiveShield(true);

        placementReward = placement;

        this.SetDelay(0.25f, () =>
        {
            SetActiveShield(false);
            Gley.MobileAds.API.ShowRewardedVideo(CompleteMethod);
        });

        void CompleteMethod(bool completed)
        {
            this.SetDelay(0.2f, () =>
            {
                if (completed)
                {
                    onSuccess?.Invoke();
                    UpdateLastAdTime(true);
                }
                else
                {
                    onFail?.Invoke();
                }

                onCompleted?.Invoke();
            });
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
            m_ShieldAds.gameObject.SetActive(false);
        }
    }
}
