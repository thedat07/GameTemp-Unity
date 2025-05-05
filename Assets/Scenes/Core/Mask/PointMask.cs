using System.Collections;
using System.Collections.Generic;
using LibraryGame;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PointMask : MonoBehaviour
{
    public Transform point;
    public Image image;
    [SerializeField] RectTransform m_RectTransform;

    RectTransform m_RectTransformMasking;

    PopupMaskController m_Popup;

    public RectTransform GetRect()
    {
        if (m_RectTransform == null)
        {
            m_RectTransform = GetComponent<RectTransform>();
        }
        return m_RectTransform;
    }

    public RectTransform GetRectMasking()
    {
        if (m_RectTransformMasking == null)
        {
            m_RectTransformMasking = transform.GetChild(0).GetComponent<RectTransform>();
        }
        return m_RectTransformMasking;
    }

    public void Init(Transform point, PopupMaskController popup, Sprite sprite, float scale)
    {
        this.point = point;
        this.m_Popup = popup;
        image.sprite = sprite;
        image.ReSize();
        Vector3 scaleFrom = Vector3.one * scale * 5;
        Vector3 scaleTo = Vector3.one * scale;
        image.transform.DOScale(scaleTo, 0.5f).From(scaleFrom).SetEase(Ease.InOutSine).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        image.transform.rotation = point.rotation;
        UpdateView();
    }

    void UpdateView()
    {
        if (point != null)
        {
            GetRect().anchoredPosition = LibraryGame.Canvas.WorldToScreenSpace(point.transform.position,
            m_Popup.Canvas.worldCamera, m_Popup.contentPoint, m_Popup.GetSize());
        }
    }
}
