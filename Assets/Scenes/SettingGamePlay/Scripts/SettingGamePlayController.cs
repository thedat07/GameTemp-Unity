using Creator;

public class SettingGamePlayController : Controller
{
    public const string SETTINGGAMEPLAY_SCENE_NAME = "SettingGamePlay";

    public override string SceneName()
    {
        return SETTINGGAMEPLAY_SCENE_NAME;
    }

    public void OnClose()
    {

    }

    public void OnHome()
    {
        ManagerDirector.ReplaceScene(QuitGamePlayController.QUITGAMEPLAY_SCENE_NAME);
    }
}