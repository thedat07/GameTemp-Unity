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
            GameManager.Instance.RunPlay();
        }
        else
        {
            ManagerDirector.ReplaceScene(MoreLivesController.MORELIVES_SCENE_NAME);
        }
    }

    public void OnClose()
    {
        GameManager.Instance.RunHome();
    }
}