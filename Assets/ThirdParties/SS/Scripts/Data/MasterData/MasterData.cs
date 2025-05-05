using System;
using DesignPatterns;
using LibraryGame;
using UnityEngine;

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

    public StageData dataStage;

    public MasterDataBase dataMoney;

    public MasterData()
    {
        dataStage = new StageData(1, MasterDataType.Stage);
        dataMoney = new StageData(0, MasterDataType.Money);
    }

    public int GetData(MasterDataType type)
    {
        int result = type switch
        {
            _ => 0
        };
        return result;
    }
}