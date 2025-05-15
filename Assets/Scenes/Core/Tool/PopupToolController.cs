using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Directory;
using UnityEngine.UI;

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

    void Start()
    {

#if UNITY_EDITOR
        GameManager.Instance.GetMasterPresenter().IsTest = true;
#endif

        if (GameManager.Instance.GetMasterPresenter().IsTest)
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
    }

    public void Log()
    {
        if (inputFieldPass.text == "AdOne_MM")
        {
            pass.SetActive(false);
            content.SetActive(true);
            GameManager.Instance.GetMasterPresenter().IsTest = true;
        }
    }

    public void AddMoney()
    {
        GameManager.Instance.GetMasterPresenter().Post(999999, MasterDataType.Money, SceneName());
    }

    public void AddStar()
    {
        //GameManager.Instance.GetQuestData().infoQuestSeassonPass.SetDataStar(10);
    }

    public void NextLevel()
    {
        GameManager.Instance.GetMasterPresenter().Post(1, MasterDataType.Stage);
        //
    }

    public void BtnSetLevel()
    {
        if (inputField.text != "")
        {
            int number;
            if (int.TryParse(inputField.text, out number))
            {
                GameManager.Instance.GetMasterPresenter().Put(int.Parse(inputField.text), MasterDataType.Stage);
                //  SS.View.Manager.RunScene(GamePlayController.GAMEPLAY_SCENE_NAME);
            }
        }
    }

    public void ShowAds()
    {
        GameManager.Instance.GetAdsPresenter().ShowMediationDebugger();
    }

    public void RemoveAds()
    {
        GameManager.Instance.GetAdsPresenter().OnRemoveAds();
    }


    public void AddTimeInfinity()
    {
        //    GameManager.Instance.GetMasterPresenter().SetData((int)System.TimeSpan.FromMinutes(15).TotalSeconds, MasterDataType.LivesInfinity, SceneName());
    }

    public void BtnHideUI()
    {
        GameManager.Instance.hideUI = !GameManager.Instance.hideUI;
        var lst = FindObjectsOfType<Controller>();
        foreach (var item in lst)
        {
            if (item != this)
            {
                item.HideUI();
            }
        }
    }
}