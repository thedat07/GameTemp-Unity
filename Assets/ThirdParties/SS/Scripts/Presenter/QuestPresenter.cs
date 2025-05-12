using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class QuestPresenter : MonoBehaviour, IInitializable, IUpdatable
{
    public const string KeyUpdate = "QuestUpdate";

    private QuestData m_Data;

    public void Initialize()
    {
        m_Data = GameManager.Instance.GetQuestData();
        InvokeRepeating(nameof(CustomUpdate), 1, 1);
    }

    public void CustomUpdate()
    {
        TigerForge.EventManager.EmitEvent(KeyUpdate);
    }

    public void SetData(int vaule, TypeQuest type)
    {
        switch (type)
        {
            default:
                break;
        }
        TigerForge.EventManager.EmitEvent(QuestData.Key);
    }
}