using Creator;
using UnityTimer;

public class HomeData
{
    public bool win;

    public HomeData()
    {
        win = false;
    }

    public HomeData(bool win)
    {
        this.win = win;
    }
}

public class HomeController : SingletonController<HomeController>
{
    public const string HOME_SCENE_NAME = "Home";

    public override string SceneName()
    {
        return HOME_SCENE_NAME;
    }

    private HomeData m_Data;

    public override void OnActive(object data)
    {
        if (data != null)
        {
            m_Data = data as HomeData;
        }
        else
        {
            m_Data = new HomeData();
        }
    }


    void Start()
    {
        Timer.Register(0.25f, () =>
        {
            Creator.Director.LoadingAnimation(false);
        });
    }
}