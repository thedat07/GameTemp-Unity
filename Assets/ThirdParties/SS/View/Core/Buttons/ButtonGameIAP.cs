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
    public InfoViewRoot infoViewRoot;

    [Header("Event")]
    public UnityEvent OnSuccess;
    public UnityEvent OnFail;
    public UnityEvent OnCompleted;

    protected List<ItemShopData> m_Data;

    protected override void StartButton()
    {
        Init();

        Event();

        void Init()
        {
            m_Data = Gley.EasyIAP.API.GetValue(yourPorduct);
            infoViewRoot.Init(m_Data);
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

            Draw("yourPorduct", "");

            Draw("textPrice", "Get decimal product price denominated in the local currency");

            Draw("infoViewRoot", "View Info");
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