using System;
using UnityEngine;
using DesignPatterns;
using LibraryGame;
using UnityEngine.Events;

[System.Serializable]
public class AdsData
{
    public const string Key = "keyAdsData";

    public bool IsRemoveShowAds
    {
        get { return LibraryGameSave.LoadAdsData("remove", false); }
        set { LibraryGameSave.SaveAdsData("remove", value); }
    }

    public AdsData()
    {

    }
}

[System.Serializable]
public class AdsDataNotSave
{
    public int adShowedCount = 0;
    public int adInterCount = 0;
    private float lastAdTime = 0f;

    public bool CheckAndShowAd()
    {
        int currentLevel = GameManager.Instance.GetMasterData().GetData(MasterDataType.Stage);
        float adInterval = StaticData.InterTimestep;
        int minLevelForAds = StaticData.LevelStartShowingInter;

        if (currentLevel >= minLevelForAds &&
            Time.time >= lastAdTime + adInterval &&
             Gley.MobileAds.API.CanShowAds())
        {
            return true;
        }

        return false;
    }

    public void UpdateLastAdTime()
    {
        lastAdTime = Time.time;
    }
}

[System.Serializable]
public class AdsConfigData
{
    public Color bannerBackgroundColor = Color.black;

    public string sdkKey = string.Empty;

    public AdsConfigID bannerId = new AdsConfigID();
    public AdsConfigID interId = new AdsConfigID();
    public AdsConfigID rewardedId = new AdsConfigID();

    public void ShowAdIfReady()
    {
        // if (MaxSdk.IsAppOpenAdReady(appOpenAd.GetID()))
        // {
        //     MaxSdk.ShowAppOpenAd(appOpenAd.GetID());
        // }
        // else
        // {
        //     MaxSdk.LoadAppOpenAd(appOpenAd.GetID());
        // }
    }
}

[System.Serializable]
public class AdsConfigID
{
    public string androidID = string.Empty;
    public string iosID = string.Empty;

    public string GetID()
    {
#if UNITY_ANDROID
        return androidID;
#elif UNITY_IOS
        return iosID;
#else
        return string.Empty;
#endif
    }

    public void SetID(string id)
    {
#if UNITY_ANDROID
        androidID = id;
#else
        iosID = id;
#endif
    }

}