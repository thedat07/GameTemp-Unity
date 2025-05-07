using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.View;

public class DTopController : Controller
{
    public const string SCENE_NAME = "DTop";

    public override string SceneName()
    {
        return SCENE_NAME;
    }

    public void OnButtonTap()
    {
        Manager.PushScene(DPopupController.SCENE_NAME, "Popup1", null, null, false);
        Manager.PushScene(DPopupController.SCENE_NAME, "Popup2", null, null, false);
    }

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
