using Creator;

public class BuyBoosterData
{

} 

public class BuyBoosterController : Controller
{
    public const string BUYBOOSTER_SCENE_NAME = "BuyBooster";

    public override string SceneName()
    {
        return BUYBOOSTER_SCENE_NAME;
    }

    public void OnWatchAds()
    {
        GameManager.Instance.GetAdsModelView().ShowRewardedVideo("BuyBooster", () =>
        {
            OnKeyBack();
        });
    }

    public void OnCoin()
    {
        GameManager.Instance.GetMasterModelView().PostMoney(StaticData.CoinAds, "BuyBooster", () =>
        {
            OnKeyBack();
        }, null, null);
    }

    public void OnClose()
    {

    }
}