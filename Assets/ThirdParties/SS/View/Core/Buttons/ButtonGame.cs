using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;
using LibraryGame;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.Serialization;

public class ButtonGame : ButtonBase, IPointerClickHandler
{
    [Header("Audio")]
    public TypeAudio typeAudio = TypeAudio.ButtonClick;

    [Serializable]
    public class ButtonClickedEvent : UnityEvent { }

    // Event delegates triggered on click.
    [FormerlySerializedAs("onClick")]
    [SerializeField]
    private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();

    protected ButtonGame()
    { }

    public ButtonClickedEvent onClick
    {
        get { return m_OnClick; }
        set { m_OnClick = value; }
    }

    private void Press()
    {
        if (!IsActive() || !IsInteractable())
            return;

        UISystemProfilerApi.AddMarker("Button.onClick", this);
        OnClick();
        PlayEffect();
        PlayAudio();
    }

    protected virtual void OnClick()
    {
        onClick?.Invoke();
    }

    protected virtual void PlayEffect()
    {
        transform.DoResetDefault();
        transform.DOPunchScale(m_Scale * 0.05f, 0.25f).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    protected virtual void PlayAudio()
    {
        GameManager.Instance.GetSettingPresenter().PlaySound(typeAudio);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        Press();
    }

    public virtual void OnSubmit(BaseEventData eventData)
    {
        Press();

        if (!IsActive() || !IsInteractable())
            return;

        DoStateTransition(SelectionState.Pressed, false);
        StartCoroutine(OnFinishSubmit());
    }

    private IEnumerator OnFinishSubmit()
    {
        var fadeTime = colors.fadeDuration;
        var elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        DoStateTransition(currentSelectionState, false);
    }
}