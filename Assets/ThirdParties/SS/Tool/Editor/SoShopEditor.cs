using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoDataRewards))]
public class SoShopEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var script = (SoDataRewards)target;

        if (GUILayout.Button("View", GUILayout.Width(60)))
        {
            ShopShowEditor.ShowWindow(script);
        }
        base.OnInspectorGUI();
    }
}