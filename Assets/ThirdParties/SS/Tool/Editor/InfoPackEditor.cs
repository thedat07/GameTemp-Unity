
using System.Linq;
using UnityEditor;
using UnityEngine;
using System;

public class InfoPackEditor : EditorWindow
{
    private static int m_Index;
    Vector2 m_Scroll;
    private static ItemShop m_ItemShop;

    private void ClearMemory()
    {
        m_ItemShop = null;
        System.GC.Collect();
    }

    public static void ShowWindow(int index)
    {
        m_Index = index;
        m_ItemShop = ShopShowEditor.soShop.purchaseConfig[index];
        GetWindow<InfoPackEditor>("Info Pack");
    }

    private void OnGUI()
    {
        if (Application.isPlaying == false)
        {
            if (ShopShowEditor.soShop != null && m_Index >= 0 && m_Index < ShopShowEditor.soShop.purchaseConfig.Count)
                OnGUIInfo();
        }
    }

    void OnGUIInfo()
    {
        if (GUILayout.Button("Add", GUILayout.Width(100)))
        {
            ShopShowEditor.soShop.purchaseConfig[m_Index].data.Add(new ItemShopData());
            if (GUI.changed)
            {
                ShopShowEditor.SaveData(false);
            }
        }

        if (ShopShowEditor.soShop == null ||
            ShopShowEditor.soShop.purchaseConfig == null ||
            m_Index < 0 ||
            m_Index >= ShopShowEditor.soShop.purchaseConfig.Count ||
            m_ItemShop == null)
        {
            EditorGUILayout.LabelField("Invalid data or index out of bounds.");
            return; // Ngừng thực thi ngay lập tức
        }

        // Dữ liệu hợp lệ, tiếp tục với ScrollView
        m_Scroll = EditorGUILayout.BeginScrollView(m_Scroll);

        foreach (var item in m_ItemShop.data.ToList())
        {
            EditorGUILayout.BeginHorizontal("box");
            OnShowItemShopData(item);
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView(); // Kết thúc ScrollView
    }

    private void OnShowItemShopData(ItemShopData data)
    {
        MasterDataType curType = data.type;

        curType = (MasterDataType)EditorGUILayout.EnumPopup(new GUIContent("Select Option"), curType);

        if (curType != data.type)
        {
            data.type = curType;
            if (GUI.changed)
                ShopShowEditor.SaveData(m_ItemShop, m_Index);
        }

        if (data.type == MasterDataType.NoAds || data.type == MasterDataType.None || data.type == MasterDataType.Stage) { }
        else
        {
            if (data.type == MasterDataType.LivesInfinity)
            {
                int curVaule = data.vaule;
                curVaule = EditorGUILayout.IntField("Vaule", curVaule);
                if (curVaule != data.vaule)
                {
                    data.vaule = curVaule;
                    if (GUI.changed)
                        ShopShowEditor.SaveData(m_ItemShop, m_Index);
                }
                GUILayout.Label("[Seconds]");
            }
            else
            {
                int curVaule = data.vaule;
                curVaule = EditorGUILayout.IntField("Vaule", curVaule);
                if (curVaule != data.vaule)
                {
                    data.vaule = curVaule;
                    if (GUI.changed)
                        ShopShowEditor.SaveData(m_ItemShop, m_Index);
                }
                if (data.vaule <= 0)
                {
                    GUIStyle warningStyle = new GUIStyle(EditorStyles.label);
                    warningStyle.normal.textColor = Color.yellow;
                    GUILayout.Label("⚠️", warningStyle);
                }
            }
        }

        if (GUILayout.Button("Remove", GUILayout.Width(100)))
        {
            ShopShowEditor.soShop.purchaseConfig[m_Index].data.Remove(data);
            if (GUI.changed)
                ShopShowEditor.SaveData(m_ItemShop, m_Index);
        }
    }

    private void OnDestroy()
    {
        ClearMemory();
    }
}
