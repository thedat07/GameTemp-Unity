using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Creator;

public class MainGameController : MonoBehaviour
{
    public const string MAIN_GAME = "MainGame";

    void Awake()
    {
        Creator.Director director = new Creator.Director();

#if DEBUG
        Debug.unityLogger.logEnabled = true;
#else
        Debug.unityLogger.logEnabled = false;
#endif
    }

    IEnumerator Start()
    {
        Creator.Director.SceneAnimationDuration = 0.15f;

        Creator.Director.LoadingSceneName = PopupLoadingController.SCENE_NAME;

        Creator.Director.MaskSceneName = PopupMaskController.SCENE_NAME;

        Creator.Director.NoInternetSceneName = PopupNoInternetController.SCENE_NAME;

        yield return new WaitForEndOfFrame();

        GameManager.Instance.GetSettingPresenter().PlayMusic();

        yield return new WaitForEndOfFrame();

        Creator.Director.RunScene(DGameController.SCENE_NAME);
    }
}
