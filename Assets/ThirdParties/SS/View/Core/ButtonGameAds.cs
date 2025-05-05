using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;
using LibraryGame;
using UnityEngine.UI;

public class ButtonGameAds : ButtonGame
{
    [Header("Setting IAP")]
    public AdsPack pack;
    public SoShop soShop;

    [Header("Ref")]
    public InfoViewDataIAP infoViewData;

    [Header("Setting")]
    public UnityEvent onCompletedAds = new UnityEvent();

    protected AdsShop m_Data;

    protected override void StartButton()
    {
        m_Data = soShop.GetAdsShop(pack);
        infoViewData.Init(m_Data);
    }

    protected override void OnClick()
    {
        m_Data.SetData(() =>
        {
            onCompletedAds?.Invoke();
        });
        onClick?.Invoke();
    }
}
