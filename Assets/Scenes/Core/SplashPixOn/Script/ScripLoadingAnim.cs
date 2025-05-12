using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScripLoadingAnim : MonoBehaviour
{
    bool isLoadedSceneOfGame = false;
    [SerializeField] GameObject objectScence;
    [SerializeField] SkeletonGraphic skelGraphic;
    [SerializeField, SpineAnimation(dataField = "skelGraphic")] string animIdleStart = "Idle";
    [SerializeField, SpineAnimation(dataField = "skelGraphic")] string animIdleEnd = "Idle";
    [SerializeField, SpineAnimation(dataField = "skelGraphic")] string animAction = "Action";
    [SerializeField] float timeFadedIn = 2f;
    public string SceneStartOfGame = "MainGame";
    public Animation animationHide;

    private void Awake()
    {
        isLoadedSceneOfGame = false;
        DontDestroyOnLoad(gameObject);
        objectScence.gameObject.SetActive(true);
        FadedShowAnim();
    }

    void FadedShowAnim()
    {
        // fade dan len
        skelGraphic.color = new Color(1f, 1f, 1f, 0f);

        DOTween.To(
           () => skelGraphic.color.a,
           a =>
           {
               Color currentColor = skelGraphic.color;
               skelGraphic.color = new Color(currentColor.r, currentColor.g, currentColor.b, a);
           }, 1f, timeFadedIn).OnComplete(() =>
           {
               PlayAnimationAction();
           })
            .SetLink(gameObject, LinkBehaviour.KillOnDisable).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    public void PlayAnimationIdle()
    {
        if (isLoadedSceneOfGame) return;
        if (skelGraphic == null) return;
        var trackEntry = skelGraphic.AnimationState.SetAnimation(0, animIdleStart, false);
        trackEntry.Complete += entry => PlayAnimationIdle();
    }
    public void PlayAnimationIdleEnd()
    {
        if (isLoadedSceneOfGame) return;
        if (skelGraphic == null) return;
        var trackEntry = skelGraphic.AnimationState.SetAnimation(0, animIdleEnd, false);
        trackEntry.Complete += entry => PlayAnimationIdleEnd();
    }
    // dien anim cua logo
    public void PlayAnimationAction()
    {
        if (isLoadedSceneOfGame) return;
        if (skelGraphic == null) return;
        var trackEntry = skelGraphic.AnimationState.SetAnimation(0, animAction, false);
        trackEntry.Complete += entry => LoadSceneIngame();
    }
    void LoadSceneIngame()
    {
        PlayAnimationIdleEnd();
        StartCoroutine(ILoading());
    }
    // load vao scene cua dev bth
    IEnumerator ILoading()
    {
        yield return null;
        var aysnc = SceneManager.LoadSceneAsync(SceneStartOfGame, LoadSceneMode.Single);
        // load data
        while (!aysnc.isDone)
            yield return null;
        isLoadedSceneOfGame = true;
        yield return null;
        skelGraphic.color = new Color(1f, 1f, 1f, 1f);
        animationHide.Play();
    }
}
