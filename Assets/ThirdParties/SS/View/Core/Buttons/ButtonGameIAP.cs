using UnityEngine.Events;
using UnityEngine;
using TMPro;
using Gley.EasyIAP;
using System.Collections.Generic;
using Gley.EasyIAP.Internal;

public class ButtonGameIAP : ButtonGame
{
    [Header("Setting")]
    public ShopProductNames yourPorduct;
    public TextMeshProUGUI textPrice;
    public InfoRewardViewRoot infoViewRoot;
    public UnityEvent OnUpdateView;

    [Header("Event")]
    public UnityEvent OnSuccess;
    public UnityEvent OnFail;
    public UnityEvent OnCompleted;

    protected List<InventoryItem> m_Data;

    [SerializeField] ProductType m_ProductType;

    CanvasGroup m_CanvasGroup;

    protected override void StartButton()
    {
        m_CanvasGroup = GetComponent<CanvasGroup>();

        Init();

        UpdateView();

        void Init()
        {
            m_Data = Gley.EasyIAP.API.GetValue(yourPorduct);

            m_ProductType = Gley.EasyIAP.API.GetProductType(yourPorduct);

            InfoRewardData infoReward = new InfoRewardData(m_Data);

            infoViewRoot.Initialize(infoReward);

            if (!IAPManager.Instance.IsInitialized())
            {
                if (textPrice)
                    textPrice.text = "???";
            }
            else
            {
                if (textPrice)
                    textPrice.text = Gley.EasyIAP.API.GetPrice(yourPorduct).ToString();
            }
        }
    }

    protected override void OnClickEvent()
    {
        GameManager.Instance.GetShopPresenter().BuyProduct(yourPorduct, OnSuccessIAP, OnFailIAP, OnCompletedIAP);

        void OnCompletedIAP()
        {
            OnCompleted?.Invoke();
            if (m_ProductType == ProductType.NonConsumable || m_ProductType == ProductType.Subscription)
            {
                UpdateView();
            }
        }

        void OnSuccessIAP()
        {
            OnSuccess?.Invoke();
        }

        void OnFailIAP()
        {
            OnFail?.Invoke();
        }
    }

    public virtual void UpdateView()
    {
        if (m_ProductType == ProductType.NonConsumable || m_ProductType == ProductType.Subscription)
        {
            // if (yourPorduct == ShopProductNames.removeads)
            // {
            //     m_CanvasGroup.interactable = !GameManager.Instance.GetAdsData().IsRemoveShowAds;
            //     m_CanvasGroup.alpha = !GameManager.Instance.GetAdsData().IsRemoveShowAds ? 1 : 0.75f;
            // }
            // else
            // {
            //     m_CanvasGroup.interactable = !Gley.EasyIAP.API.IsActive(yourPorduct);
            //     m_CanvasGroup.alpha = !Gley.EasyIAP.API.IsActive(yourPorduct) ? 1 : 0.75f;
            // }
        }
        OnUpdateView?.Invoke();
    }
}

#if UNITY_EDITOR
namespace Lean.Gui.Editor
{
    using UnityEditor;
    using TARGET = ButtonGameIAP;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(TARGET))]
    public class ButtonGameIAP_Editor : ButtonGame_Editor
    {
        protected override void DrawSelectableSettings()
        {
            base.DrawSelectableSettings();

            Draw("yourPorduct", "Gói bán IAP được liên kết với nút.");

            Draw("textPrice", "Giá bán IAP");

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