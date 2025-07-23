using Creator;
using DG.Tweening;
using UnityUtilities;

public interface ILoading
{
    void ShowLoading();
    void HideLoading();
}

public class PopupLoadingController : Controller, ILoading
{
    public const string SCENE_NAME = "PopupLoading";

    public override string SceneName()
    {
        return SCENE_NAME;
    }

    public override void CreateShield() { }

    public override void HideUI() { }

    void Start()
    {
        GetCanvasScaler().EditCanvasScaler();
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