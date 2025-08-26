using System;
using UnityUtilities;

public class FeatureDailyRewradsAds : FeatureData
{
    const string Key = "DailyRewardsAdsData";

    [System.Serializable]
    public class DailyRewardsData
    {
        public int claimIndex;
        public string lastSpinDate; // ngày lưu

        public DailyRewardsData()
        {
            claimIndex = 0;
            lastSpinDate = NetworkTime.UTC.ToString("O");
        }
    }

    private int m_MaxDailyRewards = 4;

    private DailyRewardsData m_DailyRewardsData;

    public bool IsNoti() => m_DailyRewardsData.claimIndex < m_MaxDailyRewards;

    public int GetClaimIndex() => m_DailyRewardsData.claimIndex;

    public FeatureDailyRewradsAds(TypeFeature type, int levelUnlock = 0) : base(type, levelUnlock)
    {
        LoadData();
        CheckDailyReset();
    }

    public bool CanRewards()
    {
        if (!IsUnlock())
            return false;

        CheckDailyReset();

        return m_DailyRewardsData.claimIndex < m_MaxDailyRewards;
    }

    public void Claim()
    {
        m_DailyRewardsData.claimIndex++;
        SaveData();
    }

    // Reset theo ngày user
    private void CheckDailyReset()
    {
        string today = NetworkTime.UTC.ToString("O");

        if (m_DailyRewardsData.lastSpinDate != today)
        {
            m_DailyRewardsData.claimIndex = 0;
            m_DailyRewardsData.lastSpinDate = today;
            SaveData();
        }
    }

    private void LoadData()
    {
        if (SaveExtensions.KeyExists(m_Type, Key))
        {
            m_DailyRewardsData = SaveExtensions.GetFeature<DailyRewardsData>(m_Type, Key, new DailyRewardsData());
        }
        else
        {
            m_DailyRewardsData = new DailyRewardsData();
            SaveData();
        }
    }

    private void SaveData()
    {
        SaveExtensions.PutFeature(m_Type, Key, m_DailyRewardsData);
    }
}