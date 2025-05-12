using System;
using UnityEngine;
using DesignPatterns;
using LibraryGame;
using UnityEngine.Events;

public class AdsData
{
    public const string Key = "keyAdsData";
    private const string KeyRemoveAds = "remove";
    private const bool DefaultRemoveAds = false;

    public AdsData() { }

    /// <summary>
    /// GET/PUT: Trạng thái đã mua gỡ quảng cáo
    /// </summary>
    public bool IsRemoveShowAds
    {
        get => LibraryGameSave.GetAds(KeyRemoveAds, DefaultRemoveAds);
        set => LibraryGameSave.PutAds(KeyRemoveAds, value);
    }

    /// <summary>
    /// DELETE: Đặt lại trạng thái quảng cáo
    /// </summary>
    public void Reset()
    {
        LibraryGameSave.PutAds(KeyRemoveAds, DefaultRemoveAds);
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