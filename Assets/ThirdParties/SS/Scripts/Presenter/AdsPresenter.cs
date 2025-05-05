using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class AdsPresenter : MonoBehaviour
{
    // public MaxManager maxAds;

    public AdsDataNotSave adsDataNotSave;

    AdsData m_AdsData;

    public void Init()
    {
        m_AdsData = GameManager.Instance.GetAdsData();
        // maxAds.Init();
        TigerForge.EventManager.StartListening(AdsData.Key, HideBanner);
    }

    public void ShowMediationDebugger()
    {
        //  MaxSdk.ShowMediationDebugger();
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
        if (m_AdsData.isRemoveAds == false)
        {
            m_AdsData.isRemoveAds = true;
            TigerForge.EventManager.EmitEvent(AdsData.Key);
        }
    }

    public void ShowBanner()
    {
        if (m_AdsData.isRemoveAds == false)
        {
            // maxAds.ShowBanner();
        }
    }

    public void HideBanner()
    {
        //   maxAds.HideBanner();
    }

    public void ShowInterstitial(UnityEvent onClose, string placement)
    {
        onClose?.Invoke();
        //  maxAds.ShowInterAds(placement, onClose);
    }



    public void ShowRewarded(string placement, UnityEvent onCompleted, UnityEvent onFailed)
    {
        // UnityEvent onCompletedAds = new UnityEvent();
        // onCompletedAds.AddListener(() =>
        // {
        //     StartCoroutine(Wait());
        // });

        // maxAds.ShowRewardAds(placement, onCompletedAds, onFailed);

        // IEnumerator Wait()
        // {
        //     yield return new WaitForEndOfFrame();
        //     onCompleted?.Invoke();
        //     adsDataNotSave.UpdateLastAdTime();
        // }
        onCompleted?.Invoke();
    }

    void OnDestroy()
    {
        TigerForge.EventManager.StopListening(AdsData.Key, HideBanner);
    }
}
