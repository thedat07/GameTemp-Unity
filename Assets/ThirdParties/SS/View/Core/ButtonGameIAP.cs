using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;
using LibraryGame;
using UnityEngine.UI;
using TMPro;
using System.Linq;
// using Castle.Core.Internal;

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

    public void Init(ItemShop itemShop)
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
    public ShopPack pack;
    public SoShop soShop;

    [Header("Ref")]
    public InfoViewDataIAP infoViewDataIAP;

    [Header("Textprice")]
    public TextMeshProUGUI textPrice;

    [Header("Info Button")]
    public UnityEvent onCheck = new UnityEvent();
    public UnityEvent callBackPopup = new UnityEvent();

    protected ItemShop m_Data;

    private bool m_ActiveEvent;

    protected override void StartButton()
    {
        Init();

        Event();

        void Init()
        {
            m_Data = soShop.GetItemShop(pack);
            infoViewDataIAP.Init(m_Data);
            if (textPrice)
                textPrice.text = m_Data.GetPrice();

            this.interactable = m_Data.CanBuy();

            onCheck?.Invoke();
        }

        void Event()
        {
            UpdateView();

            m_ActiveEvent = false;

            if (m_Data.productType != UnityEngine.Purchasing.ProductType.Consumable)
            {
                StartListening();
            }
            else
            {
                if (m_Data.data.Any(x => x.type == MasterDataType.NoAds))
                {
                    StartListening();
                }
            }

            void StartListening()
            {
                TigerForge.EventManager.StartListening(ShopPresenter.Key, UpdateView);
                m_ActiveEvent = true;
            }
        }
    }

    protected override void DestroyButton()
    {
        if (m_ActiveEvent)
        {
            TigerForge.EventManager.StopListening(ShopPresenter.Key, UpdateView);
        }
    }

    protected override void OnClick()
    {
        GameManager.Instance.GetShopPresenter().Buy(m_Data,
        () =>
        {
            onClick?.Invoke();
            this.interactable = m_Data.CanBuy();
        },
         () => { },
         () => { callBackPopup?.Invoke(); });
    }

    public virtual void UpdateView()
    {
        this.interactable = m_Data.CanBuy();
    }
}
