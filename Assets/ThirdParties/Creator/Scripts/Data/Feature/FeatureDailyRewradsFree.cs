using UnityEngine;
using UnityUtilities;
using System;

public class FeatureDailyRewradsFree : FeatureData
{
    const string Key = "DailyRewardsFreeData";

    [System.Serializable]
    public class DailyRewardsData
    {
        public string lastClaimTime;
        public int claimCount;

        public DailyRewardsData()
        {
            lastClaimTime = "";
            claimCount = 0;
        }
    }

    private int m_CooldownHours = 1;
    
    private DailyRewardsData m_Data;

    public FeatureDailyRewradsFree(TypeFeature type, int timeCooldownHours, int levelUnlock = 0) : base(type, levelUnlock)
    {
        m_CooldownHours = timeCooldownHours;
        LoadData();
    }

    // Người chơi claim reward
    public bool Claim()
    {
        if (!CanClaim()) return false;

        m_Data.claimCount++;
        m_Data.lastClaimTime = NetworkTime.UTC.ToString();
        SaveData();
        return true;
    }

    // Kiểm tra có thể claim chưa
    public bool CanClaim()
    {
        if (string.IsNullOrEmpty(m_Data.lastClaimTime))
            return true; // chưa claim lần nào => claim được

        DateTime lastClaim = DateTime.Parse(m_Data.lastClaimTime);
        DateTime nextAvailable = lastClaim.AddHours(m_CooldownHours);

        return DateTime.UtcNow >= nextAvailable;
    }

    // Thời gian còn lại đến lần claim kế tiếp
    public TimeSpan GetRemainingTime()
    {
        if (string.IsNullOrEmpty(m_Data.lastClaimTime))
            return TimeSpan.Zero;

        DateTime lastClaim = DateTime.Parse(m_Data.lastClaimTime);
        DateTime nextAvailable = lastClaim.AddHours(m_CooldownHours);

        TimeSpan remain = nextAvailable - DateTime.UtcNow;
        return remain.TotalSeconds > 0 ? remain : TimeSpan.Zero;
    }

    // Reset (nếu cần)
    public void ResetDaily()
    {
        m_Data.lastClaimTime = "";
        m_Data.claimCount = 0;
        SaveData();
    }

    // Get claim count
    public int GetClaimCount() => m_Data.claimCount;

    // Save/Load
    private void LoadData()
    {
        if (SaveExtensions.KeyExists(m_Type, Key))
            m_Data = SaveExtensions.GetFeature<DailyRewardsData>(m_Type, Key, new DailyRewardsData());
        else
        {
            m_Data = new DailyRewardsData();
            SaveData();
        }
    }

    private void SaveData()
    {
        SaveExtensions.PutFeature(m_Type, Key, m_Data);
    }
}