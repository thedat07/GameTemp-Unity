using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;
using LibraryGame;
using UnityEngine.UI;

public class ButtonSound : Selectable, IPointerClickHandler
{
    public ObjectOnOff icon;

    [Header("Setting")]
    public TypeAudio typeAudio = TypeAudio.ButtonClick;
    protected Vector3 m_Scale;

    protected override void Start()
    {
        base.Start();
        if (Application.isPlaying)
        {
            m_Scale = transform.localScale;
            UpdateView();
        }
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (GameManager.Instance.HasMultipleTouches()) return;

        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (!IsActive() || !IsInteractable())
            return;

        GameManager.Instance.GetSettingPresenter().SetSound();

        UpdateView();

        interactable = false;
        PlayAudio();
        transform.DoResetDefault();
        transform.DOPunchScale(m_Scale * 0.05f, 0.25f).OnComplete(() => { interactable = true; }).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    public void PlayAudio()
    {
        GameManager.Instance.GetSettingPresenter().PlaySound(typeAudio);
    }

    private void UpdateView()
    {
        icon.Set(GameManager.Instance.GetSettingData().sound);
    }
}
