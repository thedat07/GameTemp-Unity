using System;
using UnityUtilities;

public class FeaturePiggyBank : FeatureData
{
    const string Key = "PiggyBankData";

    [System.Serializable]
    public class PiggyBankData
    {
        public int exp;               // điểm hiện tại trong Pig
        public bool isFull;           // Pig đã full chưa
        public string fullTimeStamp;  // thời gian đạt full (string lưu DateTime)

        public PiggyBankData()
        {
            exp = 0;
            isFull = false;
            fullTimeStamp = "";
        }
    }

    private int m_ExpPerWin = 100;
    private int m_MaxExp = 1000;
    private int m_CountdownHours = 48;

    private PiggyBankData m_PiggyData;

    public FeaturePiggyBank(TypeFeature type, int levelUnlock = 0) : base(type, levelUnlock)
    {
        LoadData();
        CheckExpire();
    }

    // Cộng exp khi win level
    public void AddExpOnWin()
    {
        if (m_PiggyData.isFull) return; // đang full thì không cộng thêm

        m_PiggyData.exp += m_ExpPerWin;
        if (m_PiggyData.exp >= m_MaxExp)
        {
            m_PiggyData.exp = m_MaxExp;
            m_PiggyData.isFull = true;
            m_PiggyData.fullTimeStamp = NetworkTime.GetDateTimeUtc().ToString("O");
        }
        SaveData();
    }

    // Người chơi mua PiggyBank
    public bool BuyPiggy()
    {
        CheckExpire();

        if (!m_PiggyData.isFull) return false; // chưa full thì không mua được

        ResetPiggy();
        return true;
    }

    // Kiểm tra hết hạn
    private void CheckExpire()
    {
        if (m_PiggyData.isFull && !string.IsNullOrEmpty(m_PiggyData.fullTimeStamp))
        {
            DateTime fullTime = DateTime.Parse(m_PiggyData.fullTimeStamp);
            DateTime expireTime = fullTime.AddHours(m_CountdownHours);

            if (DateTime.Now >= expireTime)
            {
                ResetPiggy();
            }
        }
    }

    // Lấy thời gian còn lại để mua
    public TimeSpan GetRemainingTime()
    {
        if (!m_PiggyData.isFull || string.IsNullOrEmpty(m_PiggyData.fullTimeStamp))
            return TimeSpan.Zero;

        DateTime fullTime = DateTime.Parse(m_PiggyData.fullTimeStamp);
        DateTime expireTime = fullTime.AddHours(m_CountdownHours);

        TimeSpan remain = expireTime - DateTime.Now;
        return remain.TotalSeconds > 0 ? remain : TimeSpan.Zero;
    }

    // Reset Piggy về 0
    private void ResetPiggy()
    {
        m_PiggyData.exp = 0;
        m_PiggyData.isFull = false;
        m_PiggyData.fullTimeStamp = "";
        SaveData();
    }

    // Get Exp hiện tại
    public int GetCurrentExp() => m_PiggyData.exp;

    // Get Max Exp
    public int GetMaxExp() => m_MaxExp;

    // Kiểm tra đã full chưa
    public bool IsFull() => m_PiggyData.isFull;

    // Data save/load
    private void LoadData()
    {
        if (SaveExtensions.KeyExists(m_Type, Key))
            m_PiggyData = SaveExtensions.GetFeature<PiggyBankData>(m_Type, Key, new PiggyBankData());
        else
        {
            m_PiggyData = new PiggyBankData();
            SaveData();
        }
    }

    private void SaveData()
    {
        SaveExtensions.PutFeature(m_Type, Key, m_PiggyData);
    }
}