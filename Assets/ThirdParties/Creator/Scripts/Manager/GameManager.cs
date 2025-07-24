using UnityEngine;
using DesignPatterns;

public class GameManager : SingletonPersistent<GameManager>
{
    public bool hideUI;

    [SerializeField] SettingModel m_SettingModel;
    [SerializeField] MasterModel m_MasterModel;
    [SerializeField] QuestModel m_QuestModel;
    [SerializeField] AdsModel m_AdsModel;

    [Header("Presenter")]
    [SerializeField] SettingModelView m_SettingModelView;
    [SerializeField] MasterModelView m_MasterModelView;
    [SerializeField] StageModelView m_StageModelView;
    [SerializeField] QuestModelView m_QuestModelView;
    [SerializeField] AdsModelView m_AdsModelView;
    [SerializeField] ShopModelView m_ShopModelView;
    [SerializeField] FirebaseController m_ConfigController;
    [SerializeField] FacebookController m_FacebookController;

    public bool IsDoneFirebase() => m_ConfigController.IsDone();
    public bool IsDoneFacebook() => m_FacebookController.IsDone();

    public CheckInternet checkInternet;

    public override void Awake()
    {
        checkInternet = new CheckInternet(new CheckInternetData(this));
        Init();
    }

    public void Init()
    {
        m_MasterModel = new MasterModel();
        m_SettingModel = new SettingModel();
        m_QuestModel = new QuestModel();
        m_AdsModel= new AdsModel();

        m_SettingModelView.Initialize();
        m_MasterModelView.Initialize();
        m_StageModelView.Initialize();
        m_AdsModelView.Initialize();
        m_QuestModelView.Initialize();

        m_ConfigController.Initialize();
        m_FacebookController.Initialize();

        m_ShopModelView.Initialize();
    }

    public SettingModelView GetSettingPresenter() => m_SettingModelView;
    public MasterModelView GetMasterModelView() => m_MasterModelView;
    public StageModelView GetStageModelView() => m_StageModelView;
    public QuestModelView GetQuestModelView() => m_QuestModelView;
    public ShopModelView GetShopModelView() => m_ShopModelView;
    public AdsModelView GetAdsModelView() => m_AdsModelView;

    public SettingModel GetSettingData() => m_SettingModel;
    public MasterModel GetMasterData() => m_MasterModel;
    public QuestModel GetQuestData() => m_QuestModel;
    public AdsModel GetAdsData() => m_AdsModel;
}
