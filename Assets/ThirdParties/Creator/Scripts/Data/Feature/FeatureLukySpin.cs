using UnityEngine;
using UnityUtilities;

[System.Serializable]
public class FeatureLukySpin : FeatureData
{
    [System.Serializable]
    public class SpinData
    {
        public bool freeSpinUsed;   // đã dùng free chưa
        public int adsSpinsUsed;    // số lượt ads đã dùng
        public string lastSpinDate; // ngày lưu
    }

    private int m_MaxDailySpins = StaticData.MaxAdsSpins;

    private SpinData m_SpinData;

    public FeatureLukySpin(TypeFeature type, int levelUnlock = 0) : base(type, levelUnlock)
    {
        LoadData();
        CheckDailyReset();
    }

    // Kiểm tra có được spin không
    public bool CanSpin(bool isAdsSpin)
    {
        if (!IsUnlock())
            return false;

        CheckDailyReset();

        if (!isAdsSpin)
        {
            // free spin
            return !m_SpinData.freeSpinUsed;
        }
        else
        {
            // ads spin
            return m_SpinData.adsSpinsUsed < m_MaxDailySpins;
        }
    }

    // Thực hiện spin
    public void DoSpin(bool isAdsSpin)
    {
        if (!isAdsSpin)
        {
            m_SpinData.freeSpinUsed = true;
        }
        else
        {
            m_SpinData.adsSpinsUsed++;
        }
        SaveData();
    }

    // Lấy số lượt còn lại
    public (int freeLeft, int adsLeft) GetRemainingSpins()
    {
        CheckDailyReset();

        int freeLeft = m_SpinData.freeSpinUsed ? 0 : 1;
        int adsLeft = m_LevelUnlock - m_SpinData.adsSpinsUsed;

        return (freeLeft, adsLeft);
    }

    // Reset theo ngày user
    private void CheckDailyReset()
    {
        string today = NetworkTime.GetDateTimeUtc().ToString("yyyy-MM-dd");

        if (m_SpinData.lastSpinDate != today)
        {
            m_SpinData.freeSpinUsed = false;
            m_SpinData.adsSpinsUsed = 0;
            m_SpinData.lastSpinDate = today;
            SaveData();
        }
    }

    private void LoadData()
    {
        if (ES3.KeyExists("SpinData"))
        {
            m_SpinData = ES3.Load<SpinData>("SpinData");
        }
        else
        {
            m_SpinData = new SpinData
            {
                freeSpinUsed = false,
                adsSpinsUsed = 0,
                lastSpinDate = NetworkTime.GetDateTimeUtc().ToString("yyyy-MM-dd")
            };
            SaveData();
        }
    }

    private void SaveData()
    {
        ES3.Save("SpinData", m_SpinData);
    }
}