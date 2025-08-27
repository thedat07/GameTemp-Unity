using Creator;

public class QuitGamePlayController : Controller
{
    public const string QUITGAMEPLAY_SCENE_NAME = "QuitGamePlay";

    public override string SceneName()
    {
        return QUITGAMEPLAY_SCENE_NAME;
    }

    public void OnQuit()
    {
        ManagerDirector.RunScene(HomeController.HOME_SCENE_NAME);
    }
}