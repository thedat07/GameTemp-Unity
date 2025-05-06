using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.View;

public class MainGameController : Controller
{
    public const string MAIN_GAME = "MainGame";

    public override string SceneName()
    {
        return MAIN_GAME;
    }

    void Awake()
    {
#if DEBUG
        Debug.unityLogger.logEnabled = true;
#else
        Debug.unityLogger.logEnabled = false;
#endif
    }

    IEnumerator Start()
    {
        Manager.LoadingSceneName = DLoadingController.SCENE_NAME;

        Manager.MaskSceneName = PopupMaskController.POPUPTUTOR_SCENE_NAME;

        Manager.NoInternetSceneName = PopupNoInternetController.POPUPNOINTERNET_SCENE_NAME;
     
        yield return new WaitForEndOfFrame();

        GameManager.Instance.GetSettingPresenter().PlayMusic();

        yield return new WaitForEndOfFrame();
    }
}
