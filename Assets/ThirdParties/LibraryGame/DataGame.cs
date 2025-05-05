using UnityEngine;

public class ServiceKey
{
    public const string keyNoAds = "keyNoAd";
    public const string keyPlayedGame = "keyPlayedGame";
    public const string keyAcceptedGDPR = "keyAcceptedGDPR";
    public const string keyVideoCount = "keyVideoCount";
    public const string keyInterCount = "keyInterCount";
    public const string keyCrossProductIndex = "keyCrossProductIndex";
}


public class ServiceData
{
    public static bool noAds
    {
        get { return PlayerPrefs.GetInt(ServiceKey.keyNoAds, 0) == 1; }
        set { PlayerPrefs.SetInt(ServiceKey.keyNoAds, value ? 1 : 0); }
    }

    public static bool playedGame
    {
        get { return PlayerPrefs.GetInt(ServiceKey.keyPlayedGame, 0) == 1; }
        set { PlayerPrefs.SetInt(ServiceKey.keyPlayedGame, value ? 1 : 0); }
    }

    public static bool acceptedGDPR
    {
        get { return PlayerPrefs.GetInt(ServiceKey.keyAcceptedGDPR, 0) == 1; }
        set { PlayerPrefs.SetInt(ServiceKey.keyAcceptedGDPR, value ? 1 : 0); }
    }


    public static int videoCount
    {
        get { return PlayerPrefs.GetInt(ServiceKey.keyVideoCount, 0); }
        set { PlayerPrefs.SetInt(ServiceKey.keyVideoCount, value); }
    }

    public static int interCount
    {
        get { return PlayerPrefs.GetInt(ServiceKey.keyInterCount, 0); }
        set { PlayerPrefs.SetInt(ServiceKey.keyInterCount, value); }
    }

    public static int crossProductIndex
    {
        get { return PlayerPrefs.GetInt(ServiceKey.keyCrossProductIndex, 0); }
        set { PlayerPrefs.SetInt(ServiceKey.keyCrossProductIndex, value); }
    }
}
