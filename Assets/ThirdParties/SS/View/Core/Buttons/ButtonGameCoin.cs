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
    public UnityEvent OnCompleted;

    protected CointShop m_Data;

    protected override void StartButton()
    {
        m_Data = GameManager.Instance.GetShopPresenter().soDataRewards.GetItemCoin(pack);
        textPrice.text = string.Format("{0}", m_Data.vaule);
        infoViewRoot.Init(m_Data);
    }

    protected override void OnClickEvent()
    {
        GameManager.Instance.GetMasterPresenter().AddMoney(m_Data.vaule, pack.ToString(), OnSucccessMoney, OnFailMoney, OnCompletedMoney);

        void OnSucccessMoney()
        {
            m_Data.SetData();
            OnSucccess?.Invoke();
        }

        void OnFailMoney()
        {
            OnFail?.Invoke();
        }

        void OnCompletedMoney()
        {
            OnCompleted?.Invoke();
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

            Draw("pack", "Gói bán coin được liên kết với nút.");

            Draw("textPrice", "Giá bán coin");

            Draw("infoViewRoot", "UI hiển thị thông tin phần thưởng.");
        }

        protected override void DrawSelectableEvents(bool showUnusedEvents)
        {
            TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

            base.DrawSelectableEvents(showUnusedEvents);

            if (showUnusedEvents == true || Any(tgts, t => t.OnDown.GetPersistentEventCount() > 0))
            {
                Draw("OnSuccess", "Sự kiện được gọi khi người chơi mua thành công.");
            }

            if (showUnusedEvents == true || Any(tgts, t => t.OnClick.GetPersistentEventCount() > 0))
            {
                Draw("OnFail", "Sự kiện được gọi khi mua bị hủy hoặc lỗi.");
            }

            if (showUnusedEvents == true || Any(tgts, t => t.OnClick.GetPersistentEventCount() > 0))
            {
                Draw("OnCompleted", "Sự kiện được gọi khi mua kết thúc (bất kể thành công hay thất bại).");
            }
        }
    }
}
#endif