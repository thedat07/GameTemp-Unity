using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using SS.View;


public class MasterPresenter : MonoBehaviour
{
    private MasterData m_Data;

    public bool IsTest = false;

    public void Init()
    {
        IsTest = false;
        m_Data = GameManager.Instance.GetMasterData();
    }

    public void AddData(int vaule, MasterDataType type, string log = "")
    {
        switch (type)
        {
            case MasterDataType.Stage:
                {
                    m_Data.dataStage.AddValue(vaule);
                }
                break;
            case MasterDataType.Money:
                {
                    m_Data.dataMoney.AddValue(vaule);
                }
                break;
            default:
                break;
        };

        FirebaseEvent.LogEvent("rw_data_game",
        "level", GameManager.Instance.GetMasterData().GetData(MasterDataType.Stage).ToString(),
        "type_data", type.ToString(),
        "vaule", vaule.ToString(),
        "log", log.ToString()
        );
        TigerForge.EventManager.EmitEvent(MasterData.Key);
    }

    public void SetValue(int vaule, MasterDataType type)
    {
        switch (type)
        {
            case MasterDataType.Stage:
                {
                    m_Data.dataStage.SetValue(vaule);
                }
                break;
        }
        TigerForge.EventManager.EmitEvent(MasterData.Key);
    }

    public void AddMoney(int vaule, UnityAction onSucccess, UnityAction onFail, string log)
    {
        int newMoney = m_Data.GetData(MasterDataType.Money) - vaule;
        if (newMoney < 0)
        {
            onFail?.Invoke();
            FirebaseEvent.LogEvent("money_spend_fail",
             "level", GameManager.Instance.GetMasterData().GetData(MasterDataType.Stage).ToString(),
             "vaule", vaule.ToString(),
             "log", log);

        }
        else
        {
            onSucccess?.Invoke();
            AddData(-vaule, MasterDataType.Money);
            FirebaseEvent.LogEvent("money_spend_success",
            "level", GameManager.Instance.GetMasterData().GetData(MasterDataType.Stage).ToString(),
            "vaule", vaule.ToString(),
            "log", log);
        }
    }
}
