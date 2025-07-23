using UnityEngine;
using UnityEngine.Events;

public class MasterPresenter : MonoBehaviour, IInitializable
{
    private MasterData m_Data;

    public bool IsTest = false;
    
    public bool IsDebug = false;

    public string GetText(Gley.Localization.WordIDs wordID) => Gley.Localization.API.GetText(wordID);

    public void Initialize()
    {
        IsTest = false;
        m_Data = GameManager.Instance.GetMasterData();
    }

    void Start()
    {
        GameManager.Instance.checkInternet.CustomUpdate();
    }

    public void Post(int vaule, MasterDataType type, string log = "")
    {
        switch (type)
        {
            case MasterDataType.Stage:
                {
                    m_Data.dataStage.Post(vaule);
                }
                break;
            case MasterDataType.Money:
                {
                    m_Data.dataMoney.Post(vaule);
                }
                break;
            default:
                break;
        };

        Log();

        void Log()
        {
            UnityEngine.Console.Log("AddData", string.Format("{0}: {1}", type.ToString(), vaule));

            FirebaseEvent.LogEvent("rw_data_game",
            "level", GameManager.Instance.GetMasterData().GetData(MasterDataType.Stage).ToString(),
            "type_data", type.ToString(),
            "vaule", vaule.ToString(),
            "log", log.ToString()
            );
        }
    }

    public void Put(int vaule, MasterDataType type)
    {
        switch (type)
        {
            case MasterDataType.Stage:
                {
                    m_Data.dataStage.Put(vaule);
                }
                break;
        }
    }

    public void PostMoney(int vaule, string log, UnityAction onSucccess, UnityAction onFail, UnityAction onCompleted)
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
            Post(-vaule, MasterDataType.Money);
            FirebaseEvent.LogEvent("money_spend_success",
            "level", GameManager.Instance.GetMasterData().GetData(MasterDataType.Stage).ToString(),
            "vaule", vaule.ToString(),
            "log", log);
        }

        onCompleted?.Invoke();
    }
}
