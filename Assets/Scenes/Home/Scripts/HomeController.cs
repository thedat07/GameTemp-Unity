using Creator;

public class HomeController : Controller
{
    public const string HOME_SCENE_NAME = "Home";

    public override string SceneName()
    {
        return HOME_SCENE_NAME;
    }
}