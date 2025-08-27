using Creator;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnLockNewBoosterData
{
    public MasterDataType type;

    public UnLockNewBoosterData(MasterDataType type)
    {
        this.type = type;
    }
}

public class UnLockNewBoosterController : Controller
{
    public const string UNLOCKNEWBOOSTER_SCENE_NAME = "UnLockNewBooster";

    public override string SceneName()
    {
        return UNLOCKNEWBOOSTER_SCENE_NAME;
    }

    [Header("Ref")]
    public UnLockSO unLockSO;

    [Header("View")]
    public Image icon;
    public TextMeshProUGUI[] txtTile;
    public TextMeshProUGUI[] txtTut;

    private UnLockNewBoosterData m_Data;
    public UnLockSO.Data m_DataType;

    public override void OnActive(object data)
    {
        if (data != null)
        {
            m_Data = data as UnLockNewBoosterData;
            m_DataType = unLockSO.GetData(m_Data.type);
            InitView();
            View();
        }
    }

    void InitView()
    {
        foreach (var item in txtTile)
        {
            item.text = string.Format("{0}", m_DataType.txtTile);
        }

        foreach (var item in txtTut)
        {
            item.text = string.Format("{0}", m_DataType.txtTut);
        }

        if (icon)
        {
            icon.sprite = m_DataType.icon;
        }
    }

    void View()
    {
        switch (m_Data.type)
        {
            case MasterDataType.Booster1:
                ViewBooster1();
                break;
            case MasterDataType.Booster2:
                ViewBooster2();
                break;
            case MasterDataType.Booster3:
                ViewBooster3();
                break;
        }
    }

    void ViewBooster1()
    {

    }

    void ViewBooster2()
    {

    }

    void ViewBooster3()
    {

    }
}