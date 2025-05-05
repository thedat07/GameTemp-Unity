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
    public SoShop soShop;

    [Header("Ref")]
    public InfoViewDataIAP infoViewData;

    [Header("Setting")]
    public Text textPrice;

    protected CointShop m_Data;

    protected override void StartButton()
    {
        m_Data = soShop.GetItemCoin(pack);
        textPrice.text = string.Format("{0}", m_Data.vaule);
        infoViewData.Init(m_Data);
    }

    protected override void OnClick()
    {
        GameManager.Instance.GetMasterPresenter().EditMoney(m_Data.vaule, () =>
        {
            m_Data.SetData();
            onClick?.Invoke();
        }, () =>
        {
            //  SS.View.Manager.Add(PopupInfoController.POPUPINFO_SCENE_NAME, new PopupInfoData("Not enough gold", null));
        }, pack.ToString());
    }
}
