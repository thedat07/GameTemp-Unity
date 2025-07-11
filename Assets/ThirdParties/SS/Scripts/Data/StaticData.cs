using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Directory;


public static class StaticData
{
    public static int InterTimestep
    {
        get
        {
            return RemoteConfigController.GetIntConfig("inter_capping", 75);
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
}
