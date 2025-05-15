using UnityEngine.Events;
using UnityEngine;
using TMPro;
using Gley.EasyIAP;
using System.Collections.Generic;

public class ButtonGameIAP : ButtonGame
{
    [Header("Setting")]
    public ShopProductNames yourPorduct;
    public TextMeshProUGUI textPrice;
    public InfoRewardViewRoot infoViewRoot;

    [Header("Event")]
    public UnityEvent OnSuccess;
    public UnityEvent OnFail;
    public UnityEvent OnCompleted;

    protected List<InventoryItem> m_Data;

    protected override void StartButton()
    {
        Init();

        Event();

        void Init()
        {
            m_Data = Gley.EasyIAP.API.GetValue(yourPorduct);
            infoViewRoot.Initialize(new InfoRewardData(m_Data));
            if (textPrice)
                textPrice.text = Gley.EasyIAP.API.GetPrice(yourPorduct).ToString();

            this.interactable = Gley.EasyIAP.API.IsActive(yourPorduct);
        }

        void Event()
        {
            UpdateView();
            GetType();

            void GetType()
            {
                ProductType productType = Gley.EasyIAP.API.GetProductType(yourPorduct);
                switch (productType)
                {
                    case ProductType.Consumable:
                        //do something for consumable
                        break;

                    case ProductType.NonConsumable:
                        TigerForge.EventManager.StartListening(ShopPresenter.Key, UpdateView);
                        //do something for non-consumable
                        break;

                    case ProductType.Subscription:
                        TigerForge.EventManager.StartListening(ShopPresenter.Key, UpdateView);
                        //do something for subscription
                        break;
                }
            }
        }
    }

    protected override void DestroyButton()
    {
        GetType();

        void GetType()
        {
            ProductType productType = Gley.EasyIAP.API.GetProductType(yourPorduct);
            switch (productType)
            {
                case ProductType.Consumable:
                    //do something for consumable
                    break;

                case ProductType.NonConsumable:
                    TigerForge.EventManager.StopListening(ShopPresenter.Key, UpdateView);
                    //do something for non-consumable
                    break;

                case ProductType.Subscription:
                    TigerForge.EventManager.StopListening(ShopPresenter.Key, UpdateView);
                    //do something for subscription
                    break;
            }
        }
    }

    protected override void OnClickEvent()
    {
        GameManager.Instance.GetShopPresenter().BuyProduct(yourPorduct, OnSuccessIAP, OnFailIAP, OnCompletedIAP);

        void OnCompletedIAP()
        {
            OnCompleted?.Invoke();
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
        this.interactable = Gley.EasyIAP.API.IsActive(yourPorduct);
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