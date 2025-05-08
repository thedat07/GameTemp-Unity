using UnityEngine.Events;
using UnityEngine;

public class ButtonAdsRewarded : ButtonGame
{
    [Header("Setting")]
    public AdsPack pack;
    public InfoViewRoot infoViewRoot;

    [Header("Event")]
    public UnityEvent OnSuccess;
    public UnityEvent OnFail;
    public UnityEvent OnCompleted;

    protected AdsShop m_Data;

    protected override void StartButton()
    {
        m_Data = GameManager.Instance.GetShopPresenter().soDataRewards.GetAdsShop(pack);
        infoViewRoot.Init(m_Data);
    }

    protected override void OnClickEvent()
    {
        m_Data.SetData(OnSuccessWatch, OnFailWatch, OnCompletedWatch);

        void OnCompletedWatch()
        {
            OnCompleted?.Invoke();
        }

        void OnSuccessWatch()
        {
            OnSuccess?.Invoke();
        }

        void OnFailWatch()
        {
            OnFail?.Invoke();
        }
    }

}

#if UNITY_EDITOR
namespace Lean.Gui.Editor
{
    using UnityEditor;
    using TARGET = ButtonAdsRewarded;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(TARGET))]
    public class ButtonAdsRewarded_Editor : ButtonGame_Editor
    {
        protected override void DrawSelectableSettings()
        {
            base.DrawSelectableSettings();

            Draw("pack", "");

            Draw("infoViewData", "View Info");
        }

        protected override void DrawSelectableEvents(bool showUnusedEvents)
        {
            TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

            base.DrawSelectableEvents(showUnusedEvents);

            if (showUnusedEvents == true || Any(tgts, t => t.OnDown.GetPersistentEventCount() > 0))
            {
                Draw("OnSuccess");
            }

            if (showUnusedEvents == true || Any(tgts, t => t.OnClick.GetPersistentEventCount() > 0))
            {
                Draw("OnFail");
            }

            if (showUnusedEvents == true || Any(tgts, t => t.OnClick.GetPersistentEventCount() > 0))
            {
                Draw("OnCompleted");
            }
        }
    }
}
#endif