using Creator;

public partial class WinController : Controller
{
    public const string WIN_SCENE_NAME = "Win";

    public override string SceneName()
    {
        return WIN_SCENE_NAME;
    }

    FeaturePiggyBank m_Piggy;

    GameDataLog m_DataLog;

    WinData m_Data;

    void Awake()
    {
        Log();
    }

    void Log()
    {
        m_DataLog.logPlayDone += 1;
    }
}