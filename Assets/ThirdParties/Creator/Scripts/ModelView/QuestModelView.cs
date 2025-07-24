using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class QuestModelView : MonoBehaviour, IInitializable, IUpdatable
{
    public const string KeyUpdate = "QuestUpdate";

    private QuestModel m_Model;

    public void Initialize()
    {
        m_Model = GameManager.Instance.GetQuestData();
        InvokeRepeating(nameof(CustomUpdate), 1, 1);
    }

    public void CustomUpdate()
    {

    }
}