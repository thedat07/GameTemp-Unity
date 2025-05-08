using UnityEngine;
using SS.View;
using DG.Tweening;

public interface ILoading
{
    void ShowLoading();
    void HideLoading();
}

public class DLoadingController : Controller, ILoading
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

    public void ShowLoading()
    {
        DOTween.KillAll();

        gameObject.SetActive(true);
    }

    public void HideLoading()
    {
        gameObject.SetActive(false);
    }
}