
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Purchasing;
using System.Collections.Generic;

public class ShopShowEditor : EditorWindow
{
    public static SoShop soShop;

    Vector2 m_Scroll1;
    Vector2 m_Scroll2;
    Vector2 m_Scroll3;

    private bool m_ShopView;
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

    public static void ShowWindow(SoShop so)
    {
        soShop = so;
        GetWindow<ShopShowEditor>("Shop View");
    }

    private void OnGUI()
    {
        if (Application.isPlaying == false)
        {
            soShop = (SoShop)EditorGUILayout.ObjectField("Shop", soShop, typeof(SoShop), true);

            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal("box");
            m_ShopView = EditorGUILayout.ToggleLeft("Shop View", m_ShopView);
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
            if (m_ShopView)
            {
                GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(5));
                OnGUIShop();
            }

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

    void OnGUIShop()
    {
        if (GUILayout.Button("Add", GUILayout.Width(100)))
        {
            soShop.purchaseConfig.Add(new ItemShop());
            SaveData();
        }

        if (soShop.purchaseConfig == null || soShop.purchaseConfig.Count == 0)
        {
            EditorGUILayout.LabelField("No purchase configurations available.");
            return;
        }

        m_Scroll1 = EditorGUILayout.BeginScrollView(m_Scroll1);
        int index = 0;
        foreach (var item in soShop.purchaseConfig.ToList())
        {
            EditorGUILayout.BeginHorizontal("box");

            ShopPack shopPack = item.pack;
            ProductType productType = item.productType;

            shopPack = (ShopPack)EditorGUILayout.EnumPopup("Pack:", shopPack);
            if (shopPack != item.pack)
            {
                item.pack = shopPack;
                SaveData();
            }

            productType = (ProductType)EditorGUILayout.EnumPopup("Product Type:", productType);
            if (productType != item.productType)
            {
                item.productType = productType;
                SaveData();
            }

            if (GUILayout.Button("View", GUILayout.Width(100)))
            {
                InfoPackEditor.ShowWindow(index);
            }

            index++;

            if (GUILayout.Button("Remove", GUILayout.Width(100)))
            {
                soShop.purchaseConfig.Remove(item);
                SaveData();
            }

            EditorGUILayout.EndHorizontal(); // Kết thúc Horizontal này là đủ
        }
        EditorGUILayout.EndScrollView(); // Đúng: Kết thúc ScrollView
    }


    void OnGUICoint()
    {
        if (GUILayout.Button("Add", GUILayout.Width(100)))
        {
            soShop.cointConfig.Add(new CointShop());
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

            CointPack shopPack = item.pack;

            shopPack = (CointPack)EditorGUILayout.EnumPopup("Pack:", shopPack);
            if (shopPack != item.pack)
            {
                item.pack = shopPack;
                SaveData();
            }

            int curVaule = item.vaule;

            curVaule = EditorGUILayout.IntField("Vaule", curVaule);

            if (curVaule != item.vaule)
            {
                item.vaule = curVaule;
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
            soShop.adsConfig.Add(new AdsShop());
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

            AdsPack shopPack = item.pack;

            shopPack = (AdsPack)EditorGUILayout.EnumPopup("Pack:", shopPack);
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

    public static void SaveData(ItemShop data, int index)
    {
        if (soShop)
        {
            List<ItemShop> purchaseConfig = new List<ItemShop>();
            purchaseConfig.AddRange(soShop.purchaseConfig);
            purchaseConfig[index] = data;
            soShop.purchaseConfig = purchaseConfig;
            EditorUtility.SetDirty(soShop);
        }
    }

    public static void SaveData(CointShop data, int index)
    {
        if (soShop)
        {
            List<CointShop> purchaseConfig = new List<CointShop>();
            purchaseConfig.AddRange(soShop.cointConfig);
            purchaseConfig[index] = data;
            soShop.cointConfig = purchaseConfig;
            EditorUtility.SetDirty(soShop);
        }
    }

    public static void SaveData(AdsShop data, int index)
    {
        if (soShop)
        {
            List<AdsShop> purchaseConfig = new List<AdsShop>();
            purchaseConfig.AddRange(soShop.adsConfig);
            purchaseConfig[index] = data;
            soShop.adsConfig = purchaseConfig;
            EditorUtility.SetDirty(soShop);
        }
    }
}
