using Director;

public static class StaticData
{
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

    public static int RemoveAdFrequency
    {
        get
        {
            return RemoteConfigController.GetIntConfig("remove_ad_frequency", 10);
        }
    }

    public static int RemoveAdFirst
    {
        get
        {
            return RemoteConfigController.GetIntConfig("remove_ad_first", 10);
        }
    }

    public static bool RwBooster
    {
        get
        {
            return RemoteConfigController.GetBoolConfig("rw_booster", true);
        }
    }

    public static bool RwBoosterHide
    {
        get
        {
            return RemoteConfigController.GetBoolConfig("rw_booster_hide", true);
        }
    }

    public static bool LoseInterAd
    {
        get
        {
            return RemoteConfigController.GetBoolConfig("lose_interAd", true);
        }
    }

    public static int BoosterAvailable
    {
        get
        {
            return RemoteConfigController.GetIntConfig("booster_available", 3);
        }
    }

    public static int AmountRevive
    {
        get
        {
            return RemoteConfigController.GetIntConfig("amount_revive", 8);
        }
    }

}
