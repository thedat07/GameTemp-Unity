using Creator;

public partial class GamePlayController : SingletonController<Controller>
{
    public const string GAMEPLAY_SCENE_NAME = "GamePlay";

    public override string SceneName()
    {
        return GAMEPLAY_SCENE_NAME;
    }
}