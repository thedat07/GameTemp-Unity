using System;
using UnityEngine;
using DesignPatterns;
using LibraryGame;
using UnityEngine.Events;

public class AdsData
{
    public const string Key = "keyAdsData";

    public bool IsRemoveShowAds
    {
        get { return LibraryGameSave.LoadAdsData("remove", false); }
        set { LibraryGameSave.SaveAdsData("remove", value); }
    }

    public AdsData()
    {

    }
}

[System.Serializable]
public class AdsDataNotSave
{
    public int adShowedCount = 0;
    public int adInterCount = 0;
    private float lastAdTime = 0f;

    public bool CheckAndShowAd()
    {
        int currentLevel = GameManager.Instance.GetMasterData().GetData(MasterDataType.Stage);
        float adInterval = StaticData.InterTimestep;
        int minLevelForAds = StaticData.LevelStartShowingInter;

        if (currentLevel >= minLevelForAds &&
            Time.time >= lastAdTime + adInterval &&
             Gley.MobileAds.API.CanShowAds())
        {
            return true;
        }

        return false;
    }

    public void UpdateLastAdTime()
    {
        lastAdTime = Time.time;
    }
}