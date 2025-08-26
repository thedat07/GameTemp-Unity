using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class FeatureModelView : MonoBehaviour, IInitializable
{
    public const string KeyUpdate = "QuestUpdate";

    private FeatureModel m_Model;

    public void Initialize()
    {
        m_Model = GameManager.Instance.GetFeatureData();
    }
}