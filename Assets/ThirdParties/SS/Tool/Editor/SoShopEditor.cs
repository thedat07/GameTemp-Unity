using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoShop))]
public class SoShopEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var script = (SoShop)target;

        if (GUILayout.Button("View", GUILayout.Width(60)))
        {
            ShopShowEditor.ShowWindow(script);
        }
        base.OnInspectorGUI();
    }
}