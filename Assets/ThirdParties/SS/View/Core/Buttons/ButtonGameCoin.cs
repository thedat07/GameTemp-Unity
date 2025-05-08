using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGameCoin : ButtonGame
{
    [Header("Setting")]
    public CointPack pack;
    public InfoViewRoot infoViewRoot;
    public Text textPrice;

    [Header("Event")]
    public UnityEvent OnSucccess;
    public UnityEvent OnFail;

    protected CointShop m_Data;

    protected override void StartButton()
    {
        m_Data = GameManager.Instance.GetShopPresenter().soDataRewards.GetItemCoin(pack);
        textPrice.text = string.Format("{0}", m_Data.vaule);
        infoViewRoot.Init(m_Data);
    }

    protected override void OnClickEvent()
    {
        GameManager.Instance.GetMasterPresenter().AddMoney(m_Data.vaule, OnSucccessMoney, OnFailMoney, pack.ToString());

        void OnSucccessMoney()
        {
            m_Data.SetData();
            OnSucccess?.Invoke();
        }

        void OnFailMoney()
        {
            OnFail?.Invoke();
        }
    }
}

#if UNITY_EDITOR
namespace Lean.Gui.Editor
{
    using UnityEditor;
    using TARGET = ButtonGameCoin;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(TARGET))]
    public class ButtonGameCoin_Editor : ButtonGame_Editor
    {
        protected override void DrawSelectableSettings()
        {
            base.DrawSelectableSettings();

            Draw("pack", "");

            Draw("textPrice", "");

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
        }
    }
}
#endif