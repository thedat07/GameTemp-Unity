using Creator;

public class LoseController : Controller
{
    public const string LOSE_SCENE_NAME = "Lose";

    public override string SceneName()
    {
        return LOSE_SCENE_NAME;
    }

    void Awake()
    {
        Log();
    }

    void Log()
    {
        GameManager.Instance.log.logPlayDone += 1;
    }
}