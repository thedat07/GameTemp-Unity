using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;
using LibraryGame;
using UnityEngine.UI;

public class ButtonAdsRewarded : ButtonGame
{
    [Header("Setting")]
    public AdsPack pack;

    [Header("View")]
    public InfoViewDataIAP infoViewData;

    [Header("Event")]
    public UnityEvent OnSuccess; UnityEvent OnFail; UnityEvent OnCompleted;

    protected AdsShop m_Data;

    protected override void StartButton()
    {
        m_Data = GameManager.Instance.GetShopPresenter().soDataRewards.GetAdsShop(pack);
        infoViewData.Init(m_Data);
    }

    protected override void OnClick()
    {
        onClick?.Invoke();

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
