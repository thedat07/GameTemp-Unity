using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.View;


// https://grok.com/share/bGVnYWN5_8e46029c-dfbd-4f15-bda8-93197b607856

public static class StaticData
{
    public static readonly Vector2 ScreenGame = new Vector2(1080f, 2160f);

    /// </summary>
    public static readonly Rect[] NSA_iPhoneX = new Rect[]
     {
            new Rect (0f, 102f / 2436f, 1f, 2202f / 2436f),  // Portrait
            new Rect (132f / 2436f, 63f / 1125f, 2172f / 2436f, 1062f / 1125f)  // Landscape
     };

    public static readonly Rect[] NSA_iPhoneXsMax = new Rect[]
      {
            new Rect (0f, 102f / 2688f, 1f, 2454f / 2688f),  // Portrait
            new Rect (132f / 2688f, 63f / 1242f, 2424f / 2688f, 1179f / 1242f)  // Landscape
      };

    public static readonly Rect[] NSA_Pixel3XL_LSL = new Rect[]
     {
            new Rect (0f, 0f, 1f, 2789f / 2960f),  // Portrait
            new Rect (0f, 0f, 2789f / 2960f, 1f)  // Landscape
     };

    public static readonly Rect[] NSA_Pixel3XL_LSR = new Rect[]
     {
            new Rect (0f, 0f, 1f, 2789f / 2960f),  // Portrait
            new Rect (171f / 2960f, 0f, 2789f / 2960f, 1f)  // Landscape
     };

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

    public static int RemoveAdFrequency
    {
        get
        {
            return RemoteConfigController.GetIntConfig("RemoveAd_Frequency", 10);
        }
    }

    public static int RemoveAdFirst
    {
        get
        {
            return RemoteConfigController.GetIntConfig("RemoveAdFirst", 2);
        }
    }

    // public static int[] SwapLv
    // {
    //     get
    //     {
    //         int[] arrayLevel = GameManager.Instance.GetStagePresenter().GetLevel();
    //         string json = RemoteConfigController.GetStringConfig("Swap_lv", Helper.ObjectToJson<int[]>(arrayLevel));
    //         return Helper.JsonToObject<int[]>(json);
    //     }
    // }

    public static int CointMoney
    {
        get
        {
            return 10;
        }
    }

    public static int CointPiggyBank
    {
        get
        {
            return 100;
        }
    }
}
