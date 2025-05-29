using UnityEngine;
using Directory;
using System.Collections;
using System;
using UniRx;

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

    public void CustomUpdate()
    {
        // Hủy disposable cũ nếu có
        m_InternetCheckDisposable?.Dispose();

        // Khởi động kiểm tra mạng mỗi 3 giây
        m_InternetCheckDisposable = Observable.Interval(TimeSpan.FromSeconds(3.0f))
            .Subscribe(_ =>
            {
                if (!IsInternet())
                {
                    Manager.ShowNoInternet();
                }
            })
            .AddTo(m_Data.mono); // Tự hủy khi mono bị destroy
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