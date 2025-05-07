using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SS.View;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class DLoadingController : Controller
{
    public const string SCENE_NAME = "DLoading";

    public override string SceneName()
    {
        return SCENE_NAME;
    }

    public override void CreateShield() { }

    public override void HideUI() { }

    [Header("Ref")]
    public string sceneName;

    void Start()
    {
        LibraryGame.Game.GetScale(GetCanvasScaler());
        Canvas.ForceUpdateCanvases();
    }

    public void OnShow()
    {
        DOTween.KillAll();

        gameObject.SetActive(true);
    }

    public void OnHide()
    {
        gameObject.SetActive(false);
    }
}