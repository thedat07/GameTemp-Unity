using System.Collections.Generic;
using Creator;
using Lean.Touch;
using UnityEngine;
using UnityTimer;

public class RewardsViewData
{
    public InventoryItem[] rewards;

    public RewardsViewData(InventoryItem[] rewards)
    {
        this.rewards = rewards;
    }
}

public class RewardsController : Controller
{
    public const string REWARDS_SCENE_NAME = "Rewards";

    public override string SceneName()
    {
        return REWARDS_SCENE_NAME;
    }

    private RewardsViewData m_Data;

    private bool m_CanCheckTab;

    private bool m_FingerTap;

    public override void OnActive(object data)
    {
        if (data != null)
        {
            m_CanCheckTab = false;
            m_FingerTap = false;

            m_Data = data as RewardsViewData;

            Init();

            Timer.Register(0.25f, () =>
            {
                m_CanCheckTab = true;
            });
        }
    }

    void Init()
    {
        if (m_Data.rewards.Length == 1)
        {
            View1();
        }
        else if (m_Data.rewards.Length <= 3)
        {
            View2();
        }
        else
        {
            View3();
        }
    }

    void View1()
    {

    }

    void View2()
    {

    }

    void View3()
    {

    }

    void OnEnable()
    {
        LeanTouch.OnFingerTap += HandleFingerTap;
    }

    void OnDisable()
    {
        LeanTouch.OnFingerTap -= HandleFingerTap;
    }

    private void HandleFingerTap(LeanFinger finger)
    {
        if (m_CanCheckTab && !m_FingerTap)
        {
            OnKeyBack();
            m_FingerTap = true;
        }
    }
}