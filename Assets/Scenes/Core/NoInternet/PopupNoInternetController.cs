using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.View;
using com.cyborgAssets.inspectorButtonPro;

[System.Serializable]
public class CheckInternet
{
    public NetworkReachability network;

    public bool IsInternet()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Console.Log("Internet", "No Internet Connection");
            return false;
        }
        else
        {
            return true;
        }
    }
}

public interface INoInternet
{
    void OnShownInternet();
}

public class PopupNoInternetController : Controller
{
    public const string SCENE_NAME = "PopupNoInternet";

    public override string SceneName()
    {
        return SCENE_NAME;
    }

    public void OnShownInternet()
    {
        gameObject.SetActive(true);
    }

    [ProButton]
    public void OnTryConnect()
    {
        if (GameManager.Instance.checkInternet.IsInternet())
        {
            gameObject.SetActive(false);
            GameManager.Instance.checkInternet.network = Application.internetReachability;
        }
    }
}