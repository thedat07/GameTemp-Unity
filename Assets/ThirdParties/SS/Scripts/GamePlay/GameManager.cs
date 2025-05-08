using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;
using SS.View;
using UnityEngine.Events;

public class GameManager : SingletonPersistent<GameManager>
{
    public bool hideUI;

    [SerializeField] SettingData m_SettingData;
    [SerializeField] MasterData m_MasterData;
    [SerializeField] QuestData m_QuestData;
    [SerializeField] AdsData m_AdsData;

    [Header("Presenter")]
    [SerializeField] SettingPresenter m_SettingPresenter;
    [SerializeField] MasterPresenter m_MasterPresenter;
    [SerializeField] StagePresenter m_StagePresenter;
    [SerializeField] QuestPresenter m_QuestPresenter;
    [SerializeField] AdsPresenter m_AdsPresenter;
    [SerializeField] ShopPresenter m_ShopPresenter;
    [SerializeField] FirebaseManager m_ConfigController;
    [SerializeField] FacebookController m_FacebookController;

    public bool IsDoneFirebase() => m_ConfigController.IsDone();
    public bool IsDoneFacebook() => m_FacebookController.IsDone();

    public CheckInternet checkInternet;

    public override void Awake()
    {
        Init();
    }

    public void Init()
    {
        m_MasterData = new MasterData();
        m_SettingData = new SettingData();
        m_QuestData = new QuestData();
        m_AdsData = new AdsData();

        m_SettingPresenter.Init();
        m_MasterPresenter.Init();
        m_StagePresenter.Init();
        m_AdsPresenter.Init();
        m_QuestPresenter.Init();

        m_ConfigController.InitInfo(null, null);
        m_FacebookController.Init();

        m_ShopPresenter.Init();
    }

    public SettingPresenter GetSettingPresenter() => m_SettingPresenter;
    public MasterPresenter GetMasterPresenter() => m_MasterPresenter;
    public StagePresenter GetStagePresenter() => m_StagePresenter;
    public QuestPresenter GetQuestPresenter() => m_QuestPresenter;

    public ShopPresenter GetShopPresenter() => m_ShopPresenter;
    public AdsPresenter GetAdsPresenter() => m_AdsPresenter;

    public SettingData GetSettingData() => m_SettingData;
    public MasterData GetMasterData() => m_MasterData;
    public QuestData GetQuestData() => m_QuestData;
    public AdsData GetAdsData() => m_AdsData;

    public bool HasSingleTouch()
    {
        return Input.touchCount == 1;
    }

    public bool HasMultipleTouches()
    {
        return Input.touchCount > 1;
    }
}
