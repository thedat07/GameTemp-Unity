using Creator;

public class MoreLivesController : Controller
{
    public const string MORELIVES_SCENE_NAME = "MoreLives";

    public override string SceneName()
    {
        return MORELIVES_SCENE_NAME;
    }

    public void OnWatchAds()
    {
        GameManager.Instance.GetAdsModelView().ShowRewardedVideo("MoreLives", () =>
        {
            OnKeyBack();
        });
    }

    public void OnCoin()
    {
        GameManager.Instance.GetMasterModelView().PostMoney(StaticData.CoinAds, "MoreLives", () =>
        {
            OnKeyBack();
        }, null, null);
    }
}