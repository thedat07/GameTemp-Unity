using Creator;

public class RetryController : Controller
{
    public const string RETRY_SCENE_NAME = "Retry";

    public override string SceneName()
    {
        return RETRY_SCENE_NAME;
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
}