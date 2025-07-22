using UnityEngine;
using UnityUtilities;

public class AdsData
{
    public const string Key = "keyAdsData";
    private const string KeyRemoveAds = "keyRemoveAds";
    private const bool DefaultRemoveAds = false;

    public AdsData() { }

    /// <summary>
    /// GET/PUT: Trạng thái đã mua gỡ quảng cáo
    /// </summary>
    public bool IsRemoveShowAds
    {
        get => SaveExtensions.GetAds(KeyRemoveAds, DefaultRemoveAds);
        set => SaveExtensions.PutAds(KeyRemoveAds, value);
    }

    /// <summary>
    /// DELETE: Đặt lại trạng thái quảng cáo
    /// </summary>
    public void Reset()
    {
        SaveExtensions.PutAds(KeyRemoveAds, DefaultRemoveAds);
    }
}

[System.Serializable]
public class AdsInfoData
{
    public int adInterAds;

    public int adShowedCount
    {
        set => PlayerPrefs.SetInt("AdShowedCount", value);
        get => PlayerPrefs.GetInt("AdShowedCount", 0);
    }

    public int adInterCount
    {
        set => PlayerPrefs.SetInt("AdInterCount", value);
        get => PlayerPrefs.GetInt("AdInterCount", 0);
    }

    private float lastAdTime = -999f; // Khởi tạo ban đầu để chắc chắn ads có thể hiển thị từ đầu

    /// <summary>
    /// Kiểm tra xem có thể hiện quảng cáo interstitial không.
    /// </summary>
    public bool CanShowInterstitialAd()
    {
        int currentLevel = GameManager.Instance.GetMasterData().GetData(MasterDataType.Stage);
        int minLevelForAds = StaticData.LevelStartShowingInter;

        bool hasReachedMinLevel = currentLevel >= minLevelForAds;
        bool isAdCooldownOver = Time.time >= lastAdTime;
        bool canAdBeShown = Gley.MobileAds.API.CanShowAds();

        return hasReachedMinLevel && isAdCooldownOver && canAdBeShown;
    }

    /// <summary>
    /// Cập nhật thời gian hiển thị quảng cáo gần nhất.
    /// </summary>
    /// <param name="isRewardedAd">True nếu là reward ad, false nếu là interstitial</param>
    public void UpdateLastAdTime(bool isRewardedAd)
    {
        float cooldown = isRewardedAd ? StaticData.InterTimestepRw : StaticData.InterTimestep;
        lastAdTime = Time.time + cooldown;
    }
}