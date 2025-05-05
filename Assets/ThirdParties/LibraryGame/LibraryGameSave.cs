using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LibraryGame
{
    public class LibraryGameSave
    {
        public const string keyFileMasterData = "FileMasterData";
        public const string keyFileSettingData = "FileSettingData";
        public const string keyFileAdsData = "FileAdsData";
        public const string keyFileQuestData = "FileQuestData";
        public const string keyFileShopData = "FileShopData";
        public const string keyFileHomeData = "FileHomeData";

        public static T Load<T>(string key, T defaultValue, string filePath = "FileGame")
        {
            if (!ES3.KeyExists(key, filePath))
                ES3.Save(key, defaultValue, filePath);

            return ES3.Load(key, filePath, defaultValue);
        }

        public static void Save<T>(string key, T value, string filePath = "FileGame")
        {
            ES3.Save(key, value, filePath);
        }

        /// <summary>Save & Load Master Data</summary>
        public static T LoadMasterData<T>(MasterDataType type, string key, T defaultValue)
        {
            return Load<T>(string.Format("{0}_{1}", type, key), defaultValue, keyFileMasterData);
        }

        public static void SaveMasterData<T>(MasterDataType type, string key, T value)
        {
            Save<T>(string.Format("{0}_{1}", type, key), value, keyFileMasterData);
        }

        /// <summary>Save & Load Setting Data</summary>
        public static T LoadSettingData<T>(string key, T defaultValue)
        {
            return Load<T>(key, defaultValue, keyFileSettingData);
        }

        public static void SaveSettingData<T>(string key, T value)
        {
            Save<T>(key, value, keyFileSettingData);
        }

        /// <summary>Save & Load Ads Data</summary>
        public static T LoadAdsData<T>(string key, T defaultValue)
        {
            return Load<T>(key, defaultValue, keyFileAdsData);
        }

        public static void SaveAdsData<T>(string key, T value)
        {
            Save<T>(key, value, keyFileAdsData);
        }

        /// <summary>Save & Load Quest Data</summary>
        public static T LoadQuestData<T>(TypeQuest type, string key, T defaultValue)
        {
            return Load<T>(string.Format("{0}_{1}", type, key), defaultValue, keyFileQuestData);
        }

        public static ListES3<T> LoadQuestDataList<T>(TypeQuest type, string key, int levelUnlock = 0)
        {
            var info = GetInfoQuest(type, key);
            var listES3 = new ListES3<T>(info, levelUnlock); // internally loads from ES3 if key exists
            return listES3;

            (string, string) GetInfoQuest(TypeQuest t, string k)
            {
                return ($"{t}_{k}", keyFileQuestData);
            }
        }

        public static void SaveQuestData<T>(TypeQuest type, string key, T value)
        {
            Save<T>(string.Format("{0}_{1}", type, key), value, keyFileQuestData);
        }

        /// <summary>Save & Load Shop Data</summary>
        public static T LoadShopData<T>(string key, T defaultValue)
        {
            return Load<T>(key, defaultValue, keyFileShopData);
        }

        public static void SaveShopData<T>(string key, T value)
        {
            Save<T>(key, value, keyFileShopData);
        }

        /// <summary>Save & Load Home Data</summary>
        public static T LoadHomeData<T>(string key, T defaultValue)
        {
            return Load<T>(key, defaultValue, keyFileHomeData);
        }

        public static void SaveHomeData<T>(string key, T value)
        {
            Save<T>(key, value, keyFileHomeData);
        }
    }
}

public class ListES3<T> : List<T>
{
    private string key;
    private string filePath;

    private int m_LevelUnlock = 0;

    private bool UnLock()
    {
        return true;
    }


    public ListES3((string, string) info, int levelUnlock)
    {
        this.m_LevelUnlock = levelUnlock;
        this.key = info.Item1;
        this.filePath = info.Item2;

        if (ES3.KeyExists(key, filePath))
        {
            var loaded = ES3.Load<List<T>>(key, filePath);
            this.AddRange(loaded);
        }
    }

    public void AddES3(T item)
    {
        if (UnLock())
        {
            this.Add(item);
            ES3.Save(key, (this as List<T>), filePath);
        }
    }

    public void RemoveES3(T item)
    {
        if (this.Remove(item))
        {
            ES3.Save(key, (this as List<T>), filePath);
        }
    }

    public void ClearES3()
    {
        this.Clear();
        ES3.Save(key, (this as List<T>), filePath);
    }
}