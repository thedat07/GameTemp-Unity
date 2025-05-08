using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;
using LibraryGame;
using UnityEngine.UI;

public class ButtonGameGold : ButtonGame
{
    [Header("Setting IAP")]
    public CointPack pack;

    [Header("View")]
    public InfoViewDataIAP infoViewData;
    public Text textPrice;

    [Header("Event")]
    public UnityEvent OnSucccess;
    public UnityEvent OnFail;

    protected CointShop m_Data;

    protected override void StartButton()
    {
        m_Data = GameManager.Instance.GetShopPresenter().soDataRewards.GetItemCoin(pack);
        textPrice.text = string.Format("{0}", m_Data.vaule);
        infoViewData.Init(m_Data);
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
