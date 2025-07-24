using UnityEngine;
using UnityEngine.Events;
using ExaGames.Common.TimeBasedLifeSystem;
using NaughtyAttributes;

public class MasterModelView : MonoBehaviour, IInitializable
{
    private MasterModel m_Model;

    public LivesManager livesManager;

    public bool IsTest = false;

    public bool IsDebug = false;

    public bool CanPlay() => livesManager.CanPlay;

    [Button]
    public void ConsumeLife() => livesManager.ConsumeLife();

    public string GetText(Gley.Localization.WordIDs wordID) => Gley.Localization.API.GetText(wordID);

    public void Initialize()
    {
        IsTest = false;
        m_Model = GameManager.Instance.GetMasterData();
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
                    m_Model.dataStage.Post(vaule);
                }
                break;
            case MasterDataType.Money:
                {
                    m_Model.dataMoney.Post(vaule);
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
                    m_Model.dataStage.Put(vaule);
                }
                break;

            case MasterDataType.Money:
                {
                    m_Model.dataMoney.Post(vaule);
                }
                break;

            case MasterDataType.Booster1:
                {
                    m_Model.dataBooster1.Post(vaule);
                }
                break;
            case MasterDataType.Booster2:
                {
                    m_Model.dataBooster2.Post(vaule);
                }
                break;
            case MasterDataType.Booster3:
                {
                    m_Model.dataBooster3.Post(vaule);
                }
                break;
            case MasterDataType.Booster4:
                {
                    m_Model.dataBooster4.Post(vaule);
                }
                break;
            case MasterDataType.LivesInfinity:
                {
                    livesManager.GiveInifinite(vaule);
                }
                break;
            case MasterDataType.Lives:
                {
                    if (vaule < 5)
                    {
                        for (int i = 0; i < vaule; i++)
                        {
                            livesManager.GiveOneLife();
                        }
                    }

                    else
                        livesManager.FillLives();
                }
                break;
            default:
                break;

        }
    }

    public void PostMoney(int vaule, string log, UnityAction onSucccess, UnityAction onFail, UnityAction onCompleted)
    {
        int newMoney = m_Model.GetData(MasterDataType.Money) - vaule;
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

    public void PlayGame()
    {
        // if (GameManager.Instance.GetMasterData().dataStage.Get() <= StaticData.GoGamePlay)
        //     Manager.RunScene(GamePlayController.GAMEPLAY_SCENE_NAME);
        // else
        //     Manager.RunScene(HomeController.HOME_SCENE_NAME);

        Creator.Director.RunScene(DGameController.SCENE_NAME);
    }

}
