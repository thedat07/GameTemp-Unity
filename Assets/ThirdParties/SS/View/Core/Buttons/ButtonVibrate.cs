using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;
using LibraryGame;
using UnityEngine.UI;

public class ButtonVibrate : ButtonGame
{
    protected override void StartButton()
    {
        UpdateView();
    }

    protected override void OnClickEvent()
    {
        GameManager.Instance.GetSettingPresenter().SetVibration();
        UpdateView();
    }

    private void UpdateView()
    {

    }
}
