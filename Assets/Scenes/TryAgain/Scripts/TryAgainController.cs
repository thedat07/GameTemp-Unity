using Creator;

public class TryAgainController : Controller
{
    public const string TRYAGAIN_SCENE_NAME = "TryAgain";

    public override string SceneName()
    {
        return TRYAGAIN_SCENE_NAME;
    }

    public void OnTryGame()
    {
        if (GameManager.Instance.GetMasterModelView().CanPlay())
        {
            GameManager.Instance.GetMasterModelView().PlayGame();
        }
        else
        {
            ManagerDirector.ReplaceScene(MoreLivesController.MORELIVES_SCENE_NAME);
        }
    }

    public void OnClose()
    {
        ManagerDirector.RunScene(HomeController.HOME_SCENE_NAME);
    }
}