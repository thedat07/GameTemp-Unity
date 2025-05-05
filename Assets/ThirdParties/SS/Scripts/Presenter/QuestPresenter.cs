using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class QuestPresenter : MonoBehaviour
{
    public const string KeyUpdate = "QuestUpdate";

    private QuestData m_Data;

    public void Init()
    {
        m_Data = GameManager.Instance.GetQuestData();
        InvokeRepeating(nameof(UpdateQuest), 1, 1);
    }

    private void UpdateQuest()
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