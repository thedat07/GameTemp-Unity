using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using LibraryGame;
using TMPro;

public class ItemView : MasterDataSubject
{
    [Header("Setting")]
    public MasterDataType type;
    public bool IsAbbreviateNumber;
    public TextMeshProUGUI text;
    protected int m_Current;

    protected override void Init()
    {
        m_Current = m_MasterData.GetData(type);
        if (IsAbbreviateNumber)
        {
            text.text = string.Format("{0}", AbbrevationUtility.AbbreviateNumber(m_Current));
        }
        else
        {
            text.text = string.Format("{0}", m_Current);
        }
    }

    public override void UpdateView()
    {
        if (m_Current != m_MasterData.GetData(type))
        {
            UpdateText();
        }
        m_Current = m_MasterData.GetData(type);
    }

    protected virtual void UpdateText()
    {
        transform.DoResetDefault();

        transform.DOPunchScale(Vector3.one * 0.05f, 0.25f).SetLink(gameObject, LinkBehaviour.KillOnDestroy);

        if (IsAbbreviateNumber)
        {
            text.text = string.Format("{0}", AbbrevationUtility.AbbreviateNumber(m_MasterData.GetData(type)));
        }
        else
        {
            text.text = m_MasterData.GetData(type).ToString();
        }
    }
}
