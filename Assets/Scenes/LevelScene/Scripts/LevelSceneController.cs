using Creator;

public class LevelSceneController : Controller
{
    public const string LEVELSCENE_SCENE_NAME = "LevelScene";

    public override string SceneName()
    {
        return LEVELSCENE_SCENE_NAME;
    }
}