using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using LibraryGame;
using TMPro;

public class MasterDataView : MasterDataSubject
{
    [Header("Setting")]
    [SerializeField] MasterDataType m_Type;
    [SerializeField] TextMeshProUGUI m_Text;
    [SerializeField] bool m_IsAbbreviateNumber;
    protected int m_Current;

    protected override void Init()
    {
        m_Current = m_MasterData.GetData(m_Type);
        if (m_IsAbbreviateNumber)
        {
            m_Text.text = string.Format("{0}", AbbrevationUtility.AbbreviateNumber(m_Current));
        }
        else
        {
            m_Text.text = string.Format("{0}", m_Current);
        }
    }

    public override void UpdateView()
    {
        if (m_Current != m_MasterData.GetData(m_Type))
        {
            UpdateText();
        }
        m_Current = m_MasterData.GetData(m_Type);
    }

    protected virtual void UpdateText()
    {
        transform.DoResetDefault();

        transform.DOPunchScale(Vector3.one * 0.05f, 0.25f).SetLink(gameObject, LinkBehaviour.KillOnDestroy);

        if (m_IsAbbreviateNumber)
        {
            m_Text.text = string.Format("{0}", AbbrevationUtility.AbbreviateNumber(m_MasterData.GetData(m_Type)));
        }
        else
        {
            m_Text.text = m_MasterData.GetData(m_Type).ToString();
        }
    }
}
