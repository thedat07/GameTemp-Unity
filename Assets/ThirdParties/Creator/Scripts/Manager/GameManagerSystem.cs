using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public partial class GameManager
{
    [Button]
    public void RunPlay()
    {
        if (m_MasterModelView.CanPlay())
        {
            bool isRandomColor = StaticData.IsRandomColor;

            m_MasterModelView.ConsumeLife();
            
            Creator.Director.RunScene(GamePlayController.GAMEPLAY_SCENE_NAME);

            StaticData.IsRandomColor = true;
        }
    }

    public void RunHome()
    {
        Creator.Director.RunScene(HomeController.HOME_SCENE_NAME);
    }
}
