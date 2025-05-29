public enum MasterDataType
{
    Stage = 0,
    Money = 1,
    NoAds = 12,
    None = 13,
    LivesInfinity = 2,
}

public class MasterData
{
    public const string Key = "keyMasterData";

    public MasterDataBase dataStage;

    public MasterDataBase dataMoney;

    public MasterData()
    {
        dataStage = new MasterDataBase(1, MasterDataType.Stage);
        dataMoney = new MasterDataBase(0, MasterDataType.Money);
    }

    public int GetData(MasterDataType type)
    {
        int result = type switch
        {
            MasterDataType.Stage => dataStage.Get(),
            MasterDataType.Money => dataMoney.Get(),
            _ => 0
        };
        return result;
    }
}