using System;
using UnityUtilities;

public class FeaturePiggyBankcs : FeatureData
{
    [System.Serializable]
    public class PiggyBankData
    {
        public int exp;               // điểm hiện tại trong Pig
        public bool isFull;           // Pig đã full chưa
        public string fullTimeStamp;  // thời gian đạt full (string lưu DateTime)
    }

    private int expPerWin = 100;
    private int maxExp = 1000;
    private int countdownHours = 48;

    private PiggyBankData piggyData;

    public FeaturePiggyBankcs(TypeFeature type, int levelUnlock = 0) : base(type, levelUnlock)
    {
        LoadData();
        CheckExpire();
    }

    // Cộng exp khi win level
    public void AddExpOnWin()
    {
        if (piggyData.isFull) return; // đang full thì không cộng thêm

        piggyData.exp += expPerWin;
        if (piggyData.exp >= maxExp)
        {
            piggyData.exp = maxExp;
            piggyData.isFull = true;
            piggyData.fullTimeStamp = NetworkTime.GetDateTimeUtc().ToString("O"); // ISO 8601
        }
        SaveData();
    }

    // Người chơi mua PiggyBank
    public bool BuyPiggy()
    {
        CheckExpire();

        if (!piggyData.isFull) return false; // chưa full thì không mua được

        ResetPiggy();
        return true;
    }

    // Kiểm tra hết hạn
    private void CheckExpire()
    {
        if (piggyData.isFull && !string.IsNullOrEmpty(piggyData.fullTimeStamp))
        {
            DateTime fullTime = DateTime.Parse(piggyData.fullTimeStamp);
            DateTime expireTime = fullTime.AddHours(countdownHours);

            if (DateTime.Now >= expireTime)
            {
                ResetPiggy();
            }
        }
    }

    // Lấy thời gian còn lại để mua
    public TimeSpan GetRemainingTime()
    {
        if (!piggyData.isFull || string.IsNullOrEmpty(piggyData.fullTimeStamp))
            return TimeSpan.Zero;

        DateTime fullTime = DateTime.Parse(piggyData.fullTimeStamp);
        DateTime expireTime = fullTime.AddHours(countdownHours);

        TimeSpan remain = expireTime - DateTime.Now;
        return remain.TotalSeconds > 0 ? remain : TimeSpan.Zero;
    }

    // Reset Piggy về 0
    private void ResetPiggy()
    {
        piggyData.exp = 0;
        piggyData.isFull = false;
        piggyData.fullTimeStamp = "";
        SaveData();
    }

    // Get Exp hiện tại
    public int GetCurrentExp() => piggyData.exp;

    // Get Max Exp
    public int GetMaxExp() => maxExp;

    // Kiểm tra đã full chưa
    public bool IsFull() => piggyData.isFull;

    // Data save/load
    private void LoadData()
    {
        if (ES3.KeyExists("PiggyBankData"))
            piggyData = ES3.Load<PiggyBankData>("PiggyBankData");
        else
        {
            piggyData = new PiggyBankData
            {
                exp = 0,
                isFull = false,
                fullTimeStamp = ""
            };
            SaveData();
        }
    }

    private void SaveData()
    {
        ES3.Save("PiggyBankData", piggyData);
    }
}