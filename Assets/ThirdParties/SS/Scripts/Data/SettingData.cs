using System;
using UnityEngine;
using LibraryGame;

public class SettingData
{
    public const string Key = "keySettingData";

    // Các key lưu trữ
    private const string KeySound = "sound";
    private const string KeyVibration = "vibration";
    private const string KeyMusic = "music";

    // Giá trị mặc định
    private const bool DefaultSound = true;
    private const bool DefaultVibration = true;
    private const bool DefaultMusic = true;

    // Constructor
    public SettingData() { }

    /// <summary>
    /// GET/PUT: Âm thanh
    /// </summary>
    public bool Sound
    {
        get => LibraryGameSave.GetSetting(KeySound, DefaultSound);
        set => LibraryGameSave.PutSetting(KeySound, value);
    }

    /// <summary>
    /// GET/PUT: Rung
    /// </summary>
    public bool Vibration
    {
        get => LibraryGameSave.GetSetting(KeyVibration, DefaultVibration);
        set => LibraryGameSave.PutSetting(KeyVibration, value);
    }

    /// <summary>
    /// GET/PUT: Nhạc nền
    /// </summary>
    public bool Music
    {
        get => LibraryGameSave.GetSetting(KeyMusic, DefaultMusic);
        set => LibraryGameSave.PutSetting(KeyMusic, value);
    }

    /// <summary>
    /// RESET: Trả tất cả về mặc định
    /// </summary>
    public void ResetToDefault()
    {
        Sound = DefaultSound;
        Vibration = DefaultVibration;
        Music = DefaultMusic;
    }
}
