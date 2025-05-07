using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.View;

public class DGameController : Controller
{
    public const string SCENE_NAME = "DGame";

    public override string SceneName()
    {
        return SCENE_NAME;
    }

    public override void OnActive(object data = null)
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

    public void OnButtonTap()
    {
        Manager.PushScene(DTopController.SCENE_NAME);
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);

        Manager.LoadingAnimation(false);
    }
}