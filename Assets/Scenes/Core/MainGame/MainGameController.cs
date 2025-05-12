using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Directory;

public class MainGameController : Controller
{
    public const string MAIN_GAME = "MainGame";

    public override string SceneName()
    {
        return MAIN_GAME;
    }

    void Awake()
    {
        Manager manager = new Manager();

#if DEBUG
        Debug.unityLogger.logEnabled = true;
#else
        Debug.unityLogger.logEnabled = false;
#endif
    }

    IEnumerator Start()
    {
        Manager.SceneAnimationDuration = 0.15f;

        Manager.LoadingSceneName = PopupLoadingController.SCENE_NAME;

        Manager.MaskSceneName = PopupMaskController.SCENE_NAME;

        Manager.NoInternetSceneName = PopupNoInternetController.SCENE_NAME;

        yield return new WaitForEndOfFrame();

        GameManager.Instance.GetSettingPresenter().PlayMusic();

        yield return new WaitForEndOfFrame();

        Manager.RunScene(DGameController.SCENE_NAME);
    }
}
