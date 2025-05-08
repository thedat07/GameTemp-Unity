using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.View;


public static class StaticData
{
    public static float GetSizeAds(bool back = false)
    {
        if (Gley.MobileAds.API.CanShowAds())
        {
            return back == false ? 160 : 0;
        }
        else
        {
            return back == false ? 0 : 160;
        }
    }

    public static int InterTimestep
    {
        get
        {
            return RemoteConfigController.GetIntConfig("inter_capping", 60);
        }
    }

    public static int LevelStartShowingInter
    {
        get
        {
            return RemoteConfigController.GetIntConfig("inter_start_level", 5);
        }
    }

    public static float RateRev
    {
        get
        {
            return RemoteConfigController.GetFloatConfig("af_purchase_manual", 0.7f);
        }
    }
}
