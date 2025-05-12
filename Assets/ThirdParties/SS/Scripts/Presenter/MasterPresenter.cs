using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Directory;
using System.Collections;

public class MasterPresenter : MonoBehaviour, IInitializable
{
    private MasterData m_Data;

    public SoTextGame soTextGame;

    public bool IsTest = false;

    public string GetText(int index) => soTextGame.GetText(index);

    public void Initialize()
    {
        IsTest = false;
        m_Data = GameManager.Instance.GetMasterData();
    }

    void Start()
    {
        GameManager.Instance.checkInternet.CustomUpdate();
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

    public void AddMoney(int vaule, string log, UnityAction onSucccess, UnityAction onFail, UnityAction onCompleted)
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

        onCompleted?.Invoke();
    }
}
