using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;
using LibraryGame;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.Serialization;

public class ButtonGame : ButtonBase
{
    [Header("Audio")]
    public TypeAudio typeAudio = TypeAudio.ButtonClick;

    protected override void PlayAudio()
    {
        GameManager.Instance.GetSettingPresenter().PlaySound(typeAudio);
    }

    protected override void PlayEffect()
    {
        transform.DoResetDefault();
        transform.DOPunchScale(m_Scale * 0.05f, 0.25f).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }
}