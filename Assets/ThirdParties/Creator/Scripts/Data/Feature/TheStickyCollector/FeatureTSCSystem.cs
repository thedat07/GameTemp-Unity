using System;
using System.Collections.Generic;
using UnityEngine;
using UnityUtilities;

public partial class FeatureTheStickyCollector
{
    private void LoadData()
    {
        if (SaveExtensions.KeyExists(m_Type, Key))
        {
            m_Data = SaveExtensions.GetFeature<TheStickyCollectorData>(m_Type, Key, new TheStickyCollectorData());
        }
        else
        {
            m_Data = new TheStickyCollectorData();
            SaveData();
        }
    }

    public void Post(int amount)
    {
        if (!IsUnlock()) return;

        int newValue = Mathf.Clamp(m_Data.amount + amount, 0, Int16.MaxValue);
        Put(newValue);
    }

    private void Put(int value)
    {
        m_Data.amount = Mathf.Clamp(value, 0, Int16.MaxValue);
        SaveData();
    }

    private void DeleteData()
    {
        m_Data = new TheStickyCollectorData();
        SaveData();
    }

    private void SaveData()
    {
        SaveExtensions.PutFeature(m_Type, Key, m_Data);
    }

    public bool CanClaim()
    {
        if (m_Data.amount < m_So.GetAmount(m_Data.claimIndex))
        {
            return false;
        }
        return true;
    }


    public void Claim()
    {
        if (CanClaim())
        {
            m_Data.claimIndex++;
            SaveData();
        }
    }
}