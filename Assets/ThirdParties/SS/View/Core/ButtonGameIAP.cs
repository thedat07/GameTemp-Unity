using UnityEngine.Events;
using UnityEngine;
using TMPro;
using Gley.EasyIAP;
using System.Collections.Generic;

[System.Serializable]

public class InfoViewData
{
    public MasterDataType type;
    public GameObject view;
    public InfoShopTextView textView;

    public void Show(ItemShopData data)
    {
        if (this.type == data.type)
        {
            view.SetActive(true);
            textView.View(data);
        }
    }
}

[System.Serializable]
public class InfoViewDataIAP
{
    public InfoViewData[] textView;

    public void Init(List<ItemShopData> data)
    {
        foreach (var item in textView)
        {
            item.view.SetActive(false);
        }

        foreach (var item in textView)
        {
            var getData = data.Find(x => x.type == item.type);
            if (getData != null)
            {
                item.Show(getData);
            }
        }
    }

    public void Init(CointShop itemShop)
    {
        foreach (var item in textView)
        {
            item.view.SetActive(false);
        }

        foreach (var item in textView)
        {
            var getData = itemShop.data.Find(x => x.type == item.type);
            if (getData != null)
            {
                item.Show(getData);
            }
        }
    }

    public void Init(AdsShop itemShop)
    {
        foreach (var item in textView)
        {
            item.view.SetActive(false);
        }

        foreach (var item in textView)
        {
            var getData = itemShop.data.Find(x => x.type == item.type);
            if (getData != null)
            {
                item.Show(getData);
            }
        }
    }
}

public class ButtonGameIAP : ButtonGame
{
    [Header("Setting IAP")]
    public ShopProductNames yourPorduct;

    [Header("Textprice")]
    public TextMeshProUGUI textPrice;

    [Header("View")]
    public InfoViewDataIAP infoViewDataIAP;

    [Header("Event IAP")]
    public UnityEvent OnSuccess; UnityEvent OnFail; UnityEvent OnCompleted;

    protected List<ItemShopData> m_Data;

    protected override void StartButton()
    {
        Init();

        Event();

        void Init()
        {
            m_Data = Gley.EasyIAP.API.GetValue(yourPorduct);
            infoViewDataIAP.Init(m_Data);
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

    protected override void OnClick()
    {
        onClick?.Invoke();

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
