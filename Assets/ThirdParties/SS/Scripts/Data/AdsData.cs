using System;
using UnityEngine;
using DesignPatterns;
using LibraryGame;
using UnityEngine.Events;

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

    /// <summary>
    /// Kiểm tra xem có thể hiện quảng cáo interstitial không.
    /// </summary>
    public bool CanShowInterstitialAd()
    {
        int currentLevel = GameManager.Instance.GetMasterData().GetData(MasterDataType.Stage);
        float adInterval = StaticData.InterTimestep;
        int minLevelForAds = StaticData.LevelStartShowingInter;

        bool hasReachedMinLevel = currentLevel >= minLevelForAds;
        bool isAdCooldownOver = Time.time >= lastAdTime + adInterval;
        bool canAdBeShown = Gley.MobileAds.API.CanShowAds();

        return hasReachedMinLevel && isAdCooldownOver && canAdBeShown;
    }

    /// <summary>
    /// Cập nhật thời gian hiển thị quảng cáo gần nhất.
    /// </summary>
    public void UpdateLastAdTime()
    {
        lastAdTime = Time.time;
    }
}