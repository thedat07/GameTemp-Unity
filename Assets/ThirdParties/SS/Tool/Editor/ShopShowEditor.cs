
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Purchasing;
using System.Collections.Generic;

public class ShopShowEditor : EditorWindow
{
    public static SoDataRewards soShop;

    Vector2 m_Scroll2;
    Vector2 m_Scroll3;

    private bool m_CointView;
    private bool m_AdsView;

    private void ClearMemory()
    {
        soShop = null;
        System.GC.Collect();
    }

    private void OnDestroy()
    {
        ClearMemory();
    }

    public static void ShowWindow(SoDataRewards so)
    {
        soShop = so;
        GetWindow<ShopShowEditor>("Shop Data");
    }

    private void OnGUI()
    {
        if (Application.isPlaying == false)
        {
            soShop = (SoDataRewards)EditorGUILayout.ObjectField("Shop", soShop, typeof(SoDataRewards), true);

            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal("box");
            m_CointView = EditorGUILayout.ToggleLeft("Coint View", m_CointView);
            m_AdsView = EditorGUILayout.ToggleLeft("Ads View", m_AdsView);
            EditorGUILayout.EndHorizontal();
            OnShow();
        }
    }

    void OnShow()
    {
        if (soShop)
        {
            if (m_CointView)
            {
                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(5));
                OnGUICoint();
            }

            if (m_AdsView)
            {
                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(5));
                OnGUIAds();
            }
        }
    }

    void OnGUICoint()
    {
        if (GUILayout.Button("Add", GUILayout.Width(100)))
        {
            soShop.cointConfig.Add(new CointInfoPack());
            SaveData();
        }

        if (soShop.cointConfig == null || soShop.cointConfig.Count == 0)
        {
            EditorGUILayout.LabelField("No purchase configurations available.");
            return;
        }

        m_Scroll2 = EditorGUILayout.BeginScrollView(m_Scroll2);
        int index = 0;
        foreach (var item in soShop.cointConfig.ToList())
        {
            EditorGUILayout.BeginHorizontal("box");

            ECointPack shopPack = item.pack;

            shopPack = (ECointPack)EditorGUILayout.EnumPopup("Pack:", shopPack);
            if (shopPack != item.pack)
            {
                item.pack = shopPack;
                SaveData();
            }

            int curVaule = item.price;

            curVaule = EditorGUILayout.IntField("Vaule", curVaule);

            if (curVaule != item.price)
            {
                item.price = curVaule;
                SaveData();
            }

            if (GUILayout.Button("View", GUILayout.Width(100)))
            {
                InfoCointPackEditor.ShowWindow(index);
            }

            index++;

            if (GUILayout.Button("Remove", GUILayout.Width(100)))
            {
                soShop.cointConfig.Remove(item);
                SaveData();
            }

            EditorGUILayout.EndHorizontal(); // Kết thúc Horizontal này là đủ
        }
        EditorGUILayout.EndScrollView(); // Đúng: Kết thúc ScrollView
    }

    void OnGUIAds()
    {
        if (GUILayout.Button("Add", GUILayout.Width(100)))
        {
            soShop.adsConfig.Add(new AdsInfoPack());
            SaveData();
        }

        if (soShop.adsConfig == null || soShop.adsConfig.Count == 0)
        {
            EditorGUILayout.LabelField("No purchase configurations available.");
            return;
        }

        m_Scroll3 = EditorGUILayout.BeginScrollView(m_Scroll3);
        int index = 0;
        foreach (var item in soShop.adsConfig.ToList())
        {
            EditorGUILayout.BeginHorizontal("box");

            EAdsPack shopPack = item.pack;

            shopPack = (EAdsPack)EditorGUILayout.EnumPopup("Pack:", shopPack);
            if (shopPack != item.pack)
            {
                item.pack = shopPack;
                SaveData();
            }

            if (GUILayout.Button("View", GUILayout.Width(100)))
            {
                InfoAdsPackEditor.ShowWindow(index);
            }

            index++;

            if (GUILayout.Button("Remove", GUILayout.Width(100)))
            {
                soShop.adsConfig.Remove(item);
                SaveData();
            }

            EditorGUILayout.EndHorizontal(); // Kết thúc Horizontal này là đủ
        }
        EditorGUILayout.EndScrollView(); // Đúng: Kết thúc ScrollView
    }

    public static void SaveData(bool changed = true)
    {
        if (soShop)
        {
            if (changed)
            {
                if (GUI.changed)
                {
                    EditorUtility.SetDirty(soShop);
                }
            }
            else
            {
                EditorUtility.SetDirty(soShop);
            }
        }
    }

    public static void SaveData(CointInfoPack data, int index)
    {
        if (soShop)
        {
            List<CointInfoPack> purchaseConfig = new List<CointInfoPack>();
            purchaseConfig.AddRange(soShop.cointConfig);
            purchaseConfig[index] = data;
            soShop.cointConfig = purchaseConfig;
            EditorUtility.SetDirty(soShop);
        }
    }

    public static void SaveData(AdsInfoPack data, int index)
    {
        if (soShop)
        {
            List<AdsInfoPack> purchaseConfig = new List<AdsInfoPack>();
            purchaseConfig.AddRange(soShop.adsConfig);
            purchaseConfig[index] = data;
            soShop.adsConfig = purchaseConfig;
            EditorUtility.SetDirty(soShop);
        }
    }
}
