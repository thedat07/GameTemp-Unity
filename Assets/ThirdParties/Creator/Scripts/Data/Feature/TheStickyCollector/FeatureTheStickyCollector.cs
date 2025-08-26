using UnityEngine;
using UnityUtilities;
using System;

public partial class FeatureTheStickyCollector : FeatureData
{
    const string Key = "TheStickyCollectorData";

    public class TheStickyCollectorData
    {
        public int claimIndex;
        public int amount;

        public TheStickyCollectorData()
        {
            claimIndex = 0;
            amount = 0;
        }
    }

    DateTime m_LastResetTime;

    TheStickyCollectorData m_Data;

    FeatureTSCSO m_So;

    public int GetIndexClaim() => m_Data.claimIndex;

    public FeatureTheStickyCollector(TypeFeature type, FeatureTSCSO so, int levelUnlock = 0) : base(type, levelUnlock)
    {
        m_So = so;
        LoadData();
        CheckTimeReset();
    }

    void ResetFeature()
    {
        DeleteData();
    }
}