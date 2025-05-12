using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Directory;
using com.cyborgAssets.inspectorButtonPro;

public class DTopController : Controller
{
    public const string SCENE_NAME = "DTop";

    public override string SceneName()
    {
        return SCENE_NAME;
    }

    [ProButton]
    public void OnButtonTap1()
    {
        Manager.PushScene(DPopupController.SCENE_NAME, new DPopupData("Popup1", true), () =>
        {
            Console.Log("Life cycle", "On Show Popup1");
        }, () =>
        {
            Console.Log("Life cycle", "On Hide Popup1");
        }, false);
    }

    [ProButton]
    public void OnButtonTap2()
    {
        Manager.PushScene(DPopupController.SCENE_NAME, new DPopupData("Popup1", false), () =>
        {
            Console.Log("Life cycle", "On Show Popup1");
        }, () =>
        {
            Console.Log("Life cycle", "On Hide Popup1");
        }, true);

        Manager.PushScene(DPopupController.SCENE_NAME, new DPopupData("Popup2", true), () =>
        {
            Console.Log("Life cycle", "On Show Popup2");
        }, () =>
        {
            Console.Log("Life cycle", "On Hide Popup2");
        }, false);
    }

    [ProButton]
    public void OnSelectTap()
    {
        Manager.PushScene(DSelectController.DSELECT_SCENE_NAME);
    }

    public override void OnActive(object data)
    {
        Console.Log("Life cycle", SceneName() + " OnActive");
    }

    public override void OnShown()
    {
        Console.Log("Life cycle", SceneName() + " OnShown");
    }

    public override void OnHidden()
    {
        Console.Log("Life cycle", SceneName() + " OnHidden");
    }

    public override void OnReFocus()
    {
        Console.Log("Life cycle", SceneName() + " OnReFocus");
    }
}
