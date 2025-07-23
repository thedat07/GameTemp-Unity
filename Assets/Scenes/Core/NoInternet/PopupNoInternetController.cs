using UnityEngine;
using Creator;
using System;
using System.Collections;

[Serializable]
public class CheckInternetData
{
    public MonoBehaviour mono;
    public NetworkReachability network;

    public CheckInternetData(MonoBehaviour mono)
    {
        this.mono = mono;
        network = Application.internetReachability;
    }
}

public class CheckInternet : IInitializableData<CheckInternetData>, IUpdatable, IDisposable
{
    private CheckInternetData m_Data;
    private IDisposable m_InternetCheckDisposable;

    public CheckInternet(CheckInternetData data)
    {
        Initialize(data);
    }

    public void Initialize(CheckInternetData data)
    {
        m_Data = data;
    }

    public bool IsInternet()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            UnityEngine.Console.Log("Internet", "No Internet Connection");
            return false;
        }
        return true;
    }

    private Coroutine m_InternetCheckCoroutine;

    public void CustomUpdate()
    {
        // Hủy coroutine cũ nếu có
        if (m_InternetCheckCoroutine != null)
        {
            m_Data.mono.StopCoroutine(m_InternetCheckCoroutine);
        }

        // Bắt đầu coroutine kiểm tra internet mỗi 3 giây
        m_InternetCheckCoroutine = m_Data.mono.StartCoroutine(InternetCheckCoroutine());
    }

    private IEnumerator InternetCheckCoroutine()
    {
        var wait = new WaitForSeconds(3f);

        while (true)
        {
            if (!IsInternet())
            {
                Creator.Director.ShowNoInternet();
            }

            yield return wait;
        }
    }

    public void SetNetwork(NetworkReachability network)
    {
        m_Data.network = network;
    }

    public void Dispose()
    {
        m_InternetCheckDisposable?.Dispose();
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