using System;
using DesignPatterns;
using UnityEngine;
using LibraryGame;

[System.Serializable]
public class SettingData
{
    public const string Key = "keySettingData";

    public bool sound
    {
        get { return LibraryGameSave.LoadSettingData("sound", true); }
        set { LibraryGameSave.SaveSettingData("sound", value); }
    }

    public bool vibration
    {
        get { return LibraryGameSave.LoadSettingData("vibration", true); }
        set { LibraryGameSave.SaveSettingData("vibration", value); }
    }

    public bool music
    {
        get { return LibraryGameSave.LoadSettingData("music", true); }
        set { LibraryGameSave.SaveSettingData("music", value); }
    }

    public SettingData()
    {

    }
}