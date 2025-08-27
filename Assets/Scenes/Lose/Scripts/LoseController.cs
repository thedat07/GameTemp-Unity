using Creator;
using UnityEngine.Events;

public class LoseData
{
    public UnityAction actionRevival;

    public LoseData(UnityAction actionRevival)
    {
        this.actionRevival = actionRevival;
    }
}

public class LoseController : Controller
{
    public const string LOSE_SCENE_NAME = "Lose";

    public override string SceneName()
    {
        return LOSE_SCENE_NAME;
    }

    LoseData m_Data;

    private bool m_IsRevival;

    void Awake()
    {
        Log();
    }

    void Log()
    {
        GameManager.Instance.log.logPlayDone += 1;
    }

    public override void OnActive(object data)
    {
        m_IsRevival = false;
        if (data != null)
        {
            m_Data = data as LoseData;
        }
    }

    public void OnRevivalCoin()
    {
        GameManager.Instance.GetMasterModelView().PostMoney(StaticData.CoinKeepPlaying, "KeepPlaying", () =>
        {
            OnRevival();
        }, null, null);
    }

    public void OnRevival()
    {
        m_IsRevival = true;
        OnKeyBack();
    }

    public void OnClose()
    {
        ManagerDirector.ReplaceScene(TryAgainController.TRYAGAIN_SCENE_NAME);
    }

    public override void OnHidden()
    {
        if (m_IsRevival)
        {
            m_Data.actionRevival?.Invoke();
        }
    }
}