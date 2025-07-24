using UnityEngine;
using Creator;
using UnityEngine.UI;
using System.Linq;

public class PopupToolController : Controller
{
    public const string POPUPTOOL_SCENE_NAME = "PopupTool";

    public override string SceneName()
    {
        return POPUPTOOL_SCENE_NAME;
    }

    public InputField inputField;
    public InputField inputFieldPass;
    public GameObject pass;
    public GameObject content;

    [Header("Buttons")]
    public Button btnLog;
    public Button btnExit1;
    public Button btnExit2;
    public Button btnHideUI;
    public Button btnLevel;
    public Button btnAds;
    public Button btnRemove;
    public Button btnLive;
    public Button btnMoney;
    public Button btnNextLevel;
    public Button btnDebug;

    void Start()
    {

#if UNITY_EDITOR
        GameManager.Instance.GetMasterModelView().IsTest = true;
#endif

        if (GameManager.Instance.GetMasterModelView().IsTest)
        {
            pass.SetActive(false);
            content.SetActive(true);
        }
        else
        {
            pass.SetActive(true);
            content.SetActive(false);
        }
        SettingButton();
    }

    void SettingButton()
    {
        btnLog.onClick.AddListener(() => { Log(); });
        btnExit1.onClick.AddListener(() => { OnKeyBack(); });
        btnExit2.onClick.AddListener(() => { OnKeyBack(); });
        btnHideUI.onClick.AddListener(() => { BtnHideUI(); });
        btnLevel.onClick.AddListener(() => { BtnSetLevel(); });
        btnAds.onClick.AddListener(() => { ShowAds(); });
        btnRemove.onClick.AddListener(() => { RemoveAds(); });
        btnMoney.onClick.AddListener(() => { AddMoney(); });
        btnLive.onClick.AddListener(() => { AddTimeInfinity(); });
        btnNextLevel.onClick.AddListener(() => { NextLevel(); });
        btnDebug.onClick.AddListener(() => { AddDebug(); });
    }

    public void Log()
    {
        if (inputFieldPass.text == "AdOne_MM")
        {
            pass.SetActive(false);
            content.SetActive(true);
            GameManager.Instance.GetMasterModelView().IsTest = true;
        }
    }

    public void AddMoney()
    {
        GameManager.Instance.GetMasterModelView().Post(999999, MasterDataType.Money, SceneName());
    }

    public void AddStar()
    {
        //GameManager.Instance.GetQuestData().infoQuestSeassonPass.SetDataStar(10);
    }

    public void NextLevel()
    {
        GameManager.Instance.GetMasterModelView().Post(1, MasterDataType.Stage);
        //
    }

    public void BtnSetLevel()
    {
        if (inputField.text != "")
        {
            int number;
            if (int.TryParse(inputField.text, out number))
            {
                GameManager.Instance.GetMasterModelView().Put(int.Parse(inputField.text), MasterDataType.Stage);
                //  SS.View.Manager.RunScene(GamePlayController.GAMEPLAY_SCENE_NAME);
            }
        }
    }

    public void ShowAds()
    {
        GameManager.Instance.GetAdsModelView().ShowMediationDebugger();
    }

    public void RemoveAds()
    {
        GameManager.Instance.GetAdsModelView().OnRemoveAds();
    }


    public void AddTimeInfinity()
    {
        //    GameManager.Instance.GetMasterModelView().SetData((int)System.TimeSpan.FromMinutes(15).TotalSeconds, MasterDataType.LivesInfinity, SceneName());
    }

    public void AddDebug()
    {
        if (!GameManager.Instance.GetMasterModelView().IsDebug)
        {
            GameManager.Instance.GetMasterModelView().IsDebug = true;
        }
        //    GameManager.Instance.GetMasterModelView().SetData((int)System.TimeSpan.FromMinutes(15).TotalSeconds, MasterDataType.LivesInfinity, SceneName());
    }

    public void BtnHideUI()
    {
        string[] scenesName = new string[] { };
        GameManager.Instance.hideUI = !GameManager.Instance.hideUI;
        var lst = FindObjectsOfType<Controller>();
        foreach (var item in lst)
        {
            if (item != this || scenesName.Any(x => x == item.SceneName()))
            {
                item.HideUI();
            }
        }
    }
}