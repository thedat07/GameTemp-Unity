using Director;
using UnityEngine;

public static class StaticData
{
    public static Vector2 ScreenGame = new Vector2(1080f, 1920f);

    public static int InterTimestep
    {
        get
        {
            return RemoteConfigController.GetIntConfig("inter_capping", 75);
        }
    }

    public static int InterTimestepRw
    {
        get
        {
            return RemoteConfigController.GetIntConfig("inter_capping_rw", 75);
        }
    }

    public static int LevelStartShowingInter
    {
        get
        {
            return RemoteConfigController.GetIntConfig("inter_start_level", 10);
        }
    }

    public static float RateRev
    {
        get
        {
            return RemoteConfigController.GetFloatConfig("af_purchase_manual", 0.7f);
        }
    }
    public static int MaxAdsSpins
    {
        get
        {
            return RemoteConfigController.GetIntConfig("max_ads_spins ", 2);
        }
    }

    public static int CoinKeepPlaying = 900;

    public static int CoinAds = 900;
}
