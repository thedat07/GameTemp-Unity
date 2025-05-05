using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;
using LibraryGame;
using UnityEngine.UI;

public class ButtonGameRemoveAds : ButtonGameIAP
{
    public GameObject[] HideObject;

    public override void UpdateView()
    {
        bool canBuy = m_Data.CanBuy();

        this.interactable = canBuy;
        
        foreach (var item in HideObject)
        {
            item.SetActive(canBuy);
        }
    }
}
