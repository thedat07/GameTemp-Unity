using UnityEngine;
using Directory;
using System.Collections;

[System.Serializable]
public class CheckInternetData : Object
{
    public MonoBehaviour mono;
    public NetworkReachability network;

    public CheckInternetData(MonoBehaviour mono)
    {
        this.mono = mono;
        network = Application.internetReachability;
    }
}

public class CheckInternet : IInitializableData, IUpdatable
{
    private IEnumerator m_Coroutine;

    private CheckInternetData m_Data;

    public CheckInternet(CheckInternetData data)
    {
        Initialize(data);
    }

    public void Initialize(Object data)
    {
        m_Data = data as CheckInternetData;
    }

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

    public void CustomUpdate()
    {
        m_Coroutine = Wait(3.0f);
        m_Data.mono.StartCoroutine(m_Coroutine);
    }

    private IEnumerator Wait(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            if (GameManager.Instance.checkInternet.IsInternet() == false)
            {
                Manager.ShowNoInternet();
            }
        }
    }

    public void SetNetwork(NetworkReachability network)
    {
        m_Data.network = network;
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

    public void OnTryConnect()
    {
        if (GameManager.Instance.checkInternet.IsInternet())
        {
            gameObject.SetActive(false);
            GameManager.Instance.checkInternet.SetNetwork(Application.internetReachability);
        }
    }
}