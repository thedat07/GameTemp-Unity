using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;
using LibraryGame;
using UnityEngine.UI;

public class ButtonGameRemoveAds : ButtonGameIAP
{
    public GameObject[] objectActive;

    public override void UpdateView()
    {
        bool isActive = Gley.EasyIAP.API.IsActive(yourPorduct);

        this.interactable = isActive;
        
        foreach (var item in objectActive)
        {
            item.SetActive(isActive);
        }
    }
}
