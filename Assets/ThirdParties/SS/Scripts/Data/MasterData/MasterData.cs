using UnityEngine.Events;

public enum MasterDataType
{
    Stage = 0,
    Money = 1,

    Booster1 = 2,
    Booster2 = 3,
    Booster3 = 4,
    Booster4 = 5,

    NoAds = 12,
    None = 13,
    LivesInfinity = 14,
    Lives = 15,

}

public class MasterData
{
    public const string Key = "keyMasterData";

    public MasterDataBase dataStage;

    public MasterDataBase dataMoney;

    public BoosterData dataBooster1;
    public BoosterData dataBooster2;
    public BoosterData dataBooster3;
    public BoosterData dataBooster4;

    public MasterData()
    {
        dataStage = new MasterDataBase(1, MasterDataType.Stage);
        dataMoney = new MasterDataBase(0, MasterDataType.Money);

        dataBooster1 = new BoosterData(0, MasterDataType.Booster1, 6);
        dataBooster2 = new BoosterData(0, MasterDataType.Booster2, 10);
        dataBooster3 = new BoosterData(0, MasterDataType.Booster3, 12);
        dataBooster4 = new BoosterData(0, MasterDataType.Booster4, 13);

    }

    public int GetData(MasterDataType type)
    {
        int result = type switch
        {
            MasterDataType.Stage => dataStage.Get(),
            MasterDataType.Money => dataMoney.Get(),


            MasterDataType.Booster1 => dataBooster1.Get(),
            MasterDataType.Booster2 => dataBooster2.Get(),
            MasterDataType.Booster3 => dataBooster3.Get(),
            MasterDataType.Booster4 => dataBooster4.Get(),
            _ => 0
        };
        return result;
    }

    public int GetLevelUnlock(MasterDataType type)
    {
        int result = type switch
        {
            MasterDataType.Booster1 => dataBooster1.GetLevelUnlock(),
            MasterDataType.Booster2 => dataBooster2.GetLevelUnlock(),
            MasterDataType.Booster3 => dataBooster3.GetLevelUnlock(),
            MasterDataType.Booster4 => dataBooster4.GetLevelUnlock(),
            _ => 0
        };
        return result;
    }

    public bool IsLock(MasterDataType type, out UnityEvent callbackData)
    {
        callbackData = new UnityEvent();
        bool result = type switch
        {
            MasterDataType.Booster1 => dataBooster1.IsLock(),
            MasterDataType.Booster2 => dataBooster2.IsLock(),
            MasterDataType.Booster3 => dataBooster3.IsLock(),
            MasterDataType.Booster4 => dataBooster4.IsLock(),
            _ => true
        };
        callbackData.AddListener(() =>
        {
            switch (type)
            {
                case MasterDataType.Booster1:
                    {
                        GameManager.Instance.GetMasterPresenter().Post(2, MasterDataType.Booster1, "Tutorial");
                        dataBooster1.UnLock();
                    }
                    break;
                case MasterDataType.Booster2:
                    {
                        GameManager.Instance.GetMasterPresenter().Post(2, MasterDataType.Booster2, "Tutorial");
                        dataBooster2.UnLock();
                    }
                    break;
                case MasterDataType.Booster3:
                    {
                        GameManager.Instance.GetMasterPresenter().Post(2, MasterDataType.Booster3, "Tutorial");
                        dataBooster3.UnLock();
                    }
                    break;
                case MasterDataType.Booster4:
                    {
                        GameManager.Instance.GetMasterPresenter().Post(2, MasterDataType.Booster4, "Tutorial");
                        dataBooster4.UnLock();
                    }
                    break;
            }
        });
        return result;
    }

}