using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;
using LibraryGame;
using UnityEngine.UI;

public class ButtonGame : ButtonBase, IPointerClickHandler
{
    [Header("Audio")]
    public TypeAudio typeAudio = TypeAudio.ButtonClick;

    [Header("Setting")]
    public bool isLock;
    public UnityEvent onClick = new UnityEvent();
    public UnityEvent callback = new UnityEvent();

    private bool isAnimating = false;

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.HasMultipleTouches()) return;
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (!IsActive() || !IsInteractable()) return;
        if (isLock || isAnimating) return;

        isAnimating = true;
        interactable = false;

        OnClick();
        PlayAudio();
        PlayEffect();
    }

    protected virtual void OnClick()
    {
        onClick?.Invoke();
    }

    protected virtual void PlayEffect()
    {
        transform.DoResetDefault();
        transform.DOPunchScale(m_Scale * 0.05f, 0.25f)
            .OnComplete(OnEffectComplete)
            .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    protected virtual void OnEffectComplete()
    {
        isAnimating = false;
        interactable = true;
        callback?.Invoke();
    }

    protected virtual void PlayAudio()
    {
        GameManager.Instance.GetSettingPresenter().PlaySound(typeAudio);
    }
}