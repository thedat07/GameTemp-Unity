using Creator;

public class GamePlaySceneData
{
    public int level;
    public bool randomColor;

    public GamePlaySceneData(int level, bool randomColor)
    {
        this.level = level;
        this.randomColor = randomColor;
    }
}

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