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

    private SpinData spinData;

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
            return !spinData.freeSpinUsed;
        }
        else
        {
            // ads spin
            return spinData.adsSpinsUsed < m_MaxDailySpins;
        }
    }

    // Thực hiện spin
    public void DoSpin(bool isAdsSpin)
    {
        if (!isAdsSpin)
        {
            spinData.freeSpinUsed = true;
        }
        else
        {
            spinData.adsSpinsUsed++;
        }
        SaveData();
    }

    // Lấy số lượt còn lại
    public (int freeLeft, int adsLeft) GetRemainingSpins()
    {
        CheckDailyReset();

        int freeLeft = spinData.freeSpinUsed ? 0 : 1;
        int adsLeft = m_LevelUnlock - spinData.adsSpinsUsed;

        return (freeLeft, adsLeft);
    }

    // Reset theo ngày user
    private void CheckDailyReset()
    {
        string today = NetworkTime.GetDateTimeUtc().ToString("yyyy-MM-dd");

        if (spinData.lastSpinDate != today)
        {
            spinData.freeSpinUsed = false;
            spinData.adsSpinsUsed = 0;
            spinData.lastSpinDate = today;
            SaveData();
        }
    }

    private void LoadData()
    {
        if (ES3.KeyExists("SpinData"))
        {
            spinData = ES3.Load<SpinData>("SpinData");
        }
        else
        {
            spinData = new SpinData
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
        ES3.Save("SpinData", spinData);
    }
}