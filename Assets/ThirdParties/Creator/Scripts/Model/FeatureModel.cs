using System;
using System.Collections.Generic;
using DesignPatterns;
using UnityEngine;

public enum TypeFeature
{
    LuckySpin = 1,
    PiggyBank = 2,
    TheStickyCollector = 3,
    DailyRewards = 4
}

public class FeatureModel
{
    public FeaturePiggyBank featurePiggyBank;

    public FeatureLukySpin featureLukySpin;

    private void Init()
    {
        featurePiggyBank = new FeaturePiggyBank(TypeFeature.PiggyBank, 4);
        featureLukySpin = new FeatureLukySpin(TypeFeature.LuckySpin, 11);
    }

    public FeatureModel()
    {
        Init();
    }
}