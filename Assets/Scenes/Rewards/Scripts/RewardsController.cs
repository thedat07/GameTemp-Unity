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

    public override void OnActive(object data)
    {
        if (data != null)
        {
            m_CanCheckTab = false;
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
        if (m_CanCheckTab)
        {
            OnKeyBack();
        }
    }
}