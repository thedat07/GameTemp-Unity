using Creator;

public partial class GamePlayController : SingletonController<Controller>
{
    public const string GAMEPLAY_SCENE_NAME = "GamePlay";

    public override string SceneName()
    {
        return GAMEPLAY_SCENE_NAME;
    }

    void Start()
    {
        int secondGamePlay = 0;
        View();
        ViewTime(secondGamePlay);
    }

    void Update()
    {
        int secondGamePlay = 0;
        ViewTime(secondGamePlay);
    }
}