
using System.Linq;
using UnityEditor;
using UnityEngine;

public class InfoCointPackEditor : EditorWindow
{
    private static int m_Index;
    Vector2 m_Scroll;
    private static CointInfoPack m_ItemShop;

    private void ClearMemory()
    {
        m_ItemShop = null;
        System.GC.Collect();
    }

    public static void ShowWindow(int index)
    {
        m_Index = index;
        m_ItemShop = ShopShowEditor.soShop.cointConfig[index];
        GetWindow<InfoCointPackEditor>("Info Coint Pack");
    }

    private void OnGUI()
    {
        if (Application.isPlaying == false)
        {
            if (ShopShowEditor.soShop != null && m_Index >= 0 && m_Index < ShopShowEditor.soShop.cointConfig.Count)
                OnGUIInfo();
        }
    }

    void OnGUIInfo()
    {
        if (GUILayout.Button("Add", GUILayout.Width(100)))
        {
            ShopShowEditor.soShop.cointConfig[m_Index].data.Add(new InventoryItem());
            if (GUI.changed)
            {
                ShopShowEditor.SaveData(false);
            }
        }

        if (ShopShowEditor.soShop == null ||
            ShopShowEditor.soShop.cointConfig == null ||
            m_Index < 0 ||
            m_Index >= ShopShowEditor.soShop.cointConfig.Count ||
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
            OnShowInventoryItem(item);
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView(); // Kết thúc ScrollView
    }

    private void OnShowInventoryItem(InventoryItem data)
    {
        MasterDataType curType = data.GetDataType();

        curType = (MasterDataType)EditorGUILayout.EnumPopup(new GUIContent("Select Option"), curType);

        if (curType != data.GetDataType())
        {
            data.SetDataType(curType);
            if (GUI.changed)
                ShopShowEditor.SaveData(m_ItemShop, m_Index);
        }

        if (data.GetDataType() == MasterDataType.NoAds || data.GetDataType() == MasterDataType.None || data.GetDataType() == MasterDataType.Stage) { }
        else
        {
            if (data.GetDataType() == MasterDataType.LivesInfinity)
            {
                int curVaule = data.GetQuantity();
                curVaule = EditorGUILayout.IntField("Time", curVaule);
                if (curVaule != data.GetQuantity())
                {
                    data.SetQuantity(curVaule);
                    if (GUI.changed)
                        ShopShowEditor.SaveData(m_ItemShop, m_Index);
                }
                GUILayout.Label("[Seconds]");
            }
            else
            {
                int curVaule = data.GetQuantity();
                curVaule = EditorGUILayout.IntField("Quantity", curVaule);
                if (curVaule != data.GetQuantity())
                {
                    data.SetQuantity(curVaule);
                    if (GUI.changed)
                        ShopShowEditor.SaveData(m_ItemShop, m_Index);
                }
                if (data.GetQuantity() <= 0)
                {
                    GUIStyle warningStyle = new GUIStyle(EditorStyles.label);
                    warningStyle.normal.textColor = Color.yellow;
                    GUILayout.Label("⚠️", warningStyle);
                }
            }
        }

        if (GUILayout.Button("Remove", GUILayout.Width(100)))
        {
            ShopShowEditor.soShop.cointConfig[m_Index].data.Remove(data);
            if (GUI.changed)
                ShopShowEditor.SaveData(m_ItemShop, m_Index);
        }
    }

    private void OnDestroy()
    {
        ClearMemory();
    }
}
