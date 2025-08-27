using UnityUtilities;
using UniRx;

[System.Serializable]
public class FeatureLukySpin : FeatureData
{
    const string Key = "SpinData";

    [System.Serializable]
    public class SpinData
    {
        public BoolReactiveProperty freeSpinUsed;   // đã dùng free chưa
        public IntReactiveProperty adsSpinsUsed;    // số lượt ads đã dùng
        public StringReactiveProperty lastSpinDate; // ngày lưu

        public SpinData()
        {
            freeSpinUsed = new BoolReactiveProperty(false);
            adsSpinsUsed = new IntReactiveProperty(0);
            lastSpinDate = new StringReactiveProperty(NetworkTime.UTC.ToString());
        }
    }

    private int m_MaxDailySpins = StaticData.MaxAdsSpins;

    private SpinData m_SpinData;

    public SpinData GetData() => m_SpinData;

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
            return !m_SpinData.freeSpinUsed.Value;
        }
        else
        {
            // ads spin
            return m_SpinData.adsSpinsUsed.Value < m_MaxDailySpins;
        }
    }

    // Thực hiện spin
    public void DoSpin(bool isAdsSpin)
    {
        if (!isAdsSpin)
        {
            m_SpinData.freeSpinUsed.Value = true;
        }
        else
        {
            m_SpinData.adsSpinsUsed.Value++;
        }
        SaveData();
    }

    // Lấy số lượt còn lại
    public (int freeLeft, int adsLeft) GetRemainingSpins()
    {
        CheckDailyReset();

        int freeLeft = m_SpinData.freeSpinUsed.Value ? 0 : 1;
        int adsLeft = m_MaxDailySpins - m_SpinData.adsSpinsUsed.Value;

        return (freeLeft, adsLeft);
    }

    // Reset theo ngày user
    private void CheckDailyReset()
    {
        string today = NetworkTime.UTC.ToString();

        if (m_SpinData.lastSpinDate.Value != today)
        {
            m_SpinData.freeSpinUsed.Value = false;
            m_SpinData.adsSpinsUsed.Value = 0;
            m_SpinData.lastSpinDate.Value = today;
            SaveData();
        }
    }
    private void LoadData()
    {
        if (SaveExtensions.KeyExists(m_Type, Key))
        {
            m_SpinData = SaveExtensions.GetFeature<SpinData>(m_Type, Key, new SpinData());
        }
        else
        {
            m_SpinData = new SpinData();
            SaveData();
        }
    }

    private void SaveData()
    {
        SaveExtensions.PutFeature(m_Type, Key, m_SpinData);
    }
}