using DG.Tweening;
using Lean.Gui;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonHomeBar : LeanToggle
{
    public GameObject txtInfo;

    public RectTransform icon;

    public Transform active;

    private (Vector3, Vector3) m_Action = new(new Vector3(1.26f, 1.26f, 1.26f), new Vector3(0, 92.3f, 0));

    private (Vector3, Vector3) m_Deactivate = new(Vector3.one, Vector3.zero);

    private float m_TimeEffect = 0.25f;

    private bool m_IsActive;

    DG.Tweening.Sequence m_Sequence;

    void Start()
    {
        OnOn.AddListener(OnActive);
        OnOff.AddListener(OnDeactivate);
        if (On)
        {
            UnityTimer.Timer.Register(0.2f, ViewInit);
        }
    }

    private void ViewInit()
    {
        icon.localScale = m_Action.Item1;
        icon.anchoredPosition = m_Action.Item2;
        txtInfo.SetActive(true);
        m_IsActive = true;
        Vector3 point = transform.position;
        point.y = active.transform.position.y;
        active.transform.position = point;
    }

    private void OnActive()
    {
        if (!m_IsActive)
        {
            KillSequence();
            m_Sequence.Append(icon.DOScale(m_Action.Item1, m_TimeEffect));
            m_Sequence.Join(icon.DOAnchorPos(m_Action.Item2, m_TimeEffect));
            txtInfo.SetActive(true);
            MoveActive();
            m_IsActive = true;
        }
    }

    private void OnDeactivate()
    {
        KillSequence();
        m_Sequence.Append(icon.DOScale(m_Deactivate.Item1, m_TimeEffect));
        m_Sequence.Join(icon.DOAnchorPos(m_Deactivate.Item2, m_TimeEffect));
        txtInfo.SetActive(false);
        m_IsActive = false;
    }

    private void KillSequence()
    {
        if (m_Sequence != null && m_Sequence.IsActive())
        {
            m_Sequence.Kill();
        }

        m_Sequence = DOTween.Sequence();
        m_Sequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    public void MoveActive()
    {
        active.DOKill();
        Vector3 point = transform.position;
        point.y = active.transform.position.y;
        active.DOMove(point, m_TimeEffect / 2).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }
}
