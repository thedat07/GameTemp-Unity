using UnityEngine;
using UnityUtilities;
using System;

public partial class FeatureTheStickyCollector : FeatureData

{
    DateTime lastResetTime;

    public FeatureTheStickyCollector(TypeFeature type, int levelUnlock = 0) : base(type, levelUnlock)
    {
        LoadData();
        CheckTimeReset();
    }

    void ResetFeature()
    {

    }
}