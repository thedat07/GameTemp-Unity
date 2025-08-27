using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameManager
{
    public void RunPlay()
    {
        if (m_MasterModelView.CanPlay())
        {
            m_MasterModelView.ConsumeLife();
            Creator.Director.RunScene(GamePlayController.GAMEPLAY_SCENE_NAME);
        }
    }

    public void RunHome()
    {
        Creator.Director.RunScene(HomeController.HOME_SCENE_NAME);
    }
}
