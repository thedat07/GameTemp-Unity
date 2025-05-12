using Firebase.Analytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirebaseEvent
{
    //economy type
    public const string param_coin = "coin";

    //campaign
    public const string campaign_complete_level_ = "campaign_complete_level_";
    public const string campaign_ad_total_view_perday_ = "campaign_ad_total_view_";
    public const string campaign_ad_inter_view_perday_ = "campaign_ad_inter_view";
    public const string campaign_ad_video_view_perday_ = "campaign_ad_video_view";

    //params 
    public const string param_level_name = "level_name";


    public static void LogEvent(string eventType)
    {
        Console.Log("Firebase", "eventname: " + eventType);
        LogAnalytics(eventType);
    }

    public static void LogEvent(string eventType, string paramName, string paramValue)
    {
        if (GameManager.Instance.IsDoneFirebase() == false) return;

        Console.Log("Firebase", "eventname: " + eventType + "\n" + paramName + ": " + paramValue
                     );
        LogAnalytics(eventType, paramName, paramValue);
    }

    public static void LogEvent(string eventType, string paramName, string paramValue, string paramName2, string paramValue2)
    {
        Console.Log("Firebase", "eventname: " + eventType + "\n" + paramName + ": " + paramValue
                     + "\n" + paramName2 + ": " + paramValue2
                     );

        var p = new Parameter[]
        {
            new Parameter(paramName, string.Format("{0}", paramValue)),
            new Parameter(paramName2, string.Format("{0}", paramValue2))
        };
        LogAnalytics(eventType, p);
    }

    public static void LogEvent(string eventType, string paramName, string paramValue, string paramName2, string paramValue2, string paramName3, string paramValue3)
    {
        Console.Log("Firebase", "eventname: " + eventType + "\n" + paramName + ": " + paramValue
                     + "\n" + paramName2 + ": " + paramValue2
                     + "\n" + paramName3 + ": " + paramValue3
                     );


        var p = new Parameter[]
        {
            new Parameter(paramName, paramValue),
            new Parameter(paramName2, paramValue2),
            new Parameter(paramName3, paramValue3)
        };
        LogAnalytics(eventType, p);

    }

    public static void LogFirstTimeLevel(int level)
    {
        string key = $"Level_{level}_FirstTime";
        if (!PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.SetInt(key, 1);
            PlayerPrefs.Save();
            LogEvent(string.Format("pass_level_{0}", level));
        }
    }

    public static void OnLevelPass()
    {
        int level = GameManager.Instance.GetMasterData().dataStage.Get();
        var listCountAvailableForEvent = new List<int> { 2, 3, 4, 5, 8, 10, 15, 20, 25, 30, 35 };
        if (listCountAvailableForEvent.Contains(level))
        {
            LogFirstTimeLevel(level);
        }
    }

    public static void LogEventReward(string paramValue2)
    {
        string eventType = "af_rewarded";
        string paramName = "level";
        string paramValue = GameManager.Instance.GetMasterData().GetData(MasterDataType.Stage).ToString();
        string paramName2 = "reward_type";

        Console.Log("Firebase", "eventname: " + eventType + "\n" + paramName + ": " + paramValue
                     + "\n" + paramName2 + ": " + paramValue2
                     );


        var p = new Parameter[]
        {
            new Parameter(paramName, paramValue),
            new Parameter(paramName2, paramValue2),
        };
        LogAnalytics(eventType, p);

    }

    public static void LogEvent(string eventType, string paramName, string paramValue, string paramName2, string paramValue2, string paramName3, string paramValue3, string paramName4, string paramValue4)
    {
        Console.Log("Firebase", "eventname: " + eventType + "\n" + paramName + ": " + paramValue
                     + "\n" + paramName2 + ": " + paramValue2
                     + "\n" + paramName3 + ": " + paramValue3
                     + "\n" + paramName4 + ": " + paramValue4
                     );

        var p = new Parameter[]
        {
            new Parameter(paramName, paramValue),
            new Parameter(paramName2, paramValue2),
            new Parameter(paramName3, paramValue3),
            new Parameter(paramName4, paramValue4)
        };

        LogAnalytics(eventType, p);
    }

    public static void LogAnalytics(string name, params Parameter[] parameters)
    {
        if (GameManager.Instance.IsDoneFirebase() == false)
        {
            UnityEngine.Console.LogError("Firebase", "Firebase is Error");
        }
        else
        {
            FirebaseAnalytics.LogEvent(name, parameters);
        }
    }

    public static void LogAnalytics(string name, string parameterName, string parameterValue)
    {
        if (GameManager.Instance.IsDoneFirebase() == false)
        {
            UnityEngine.Console.LogError("Firebase", "Firebase is Error");
        }
        else
        {
            FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
        }
    }
}

