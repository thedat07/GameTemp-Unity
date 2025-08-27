using Creator;

public partial class GamePlayController
{
    public void PopupWin(WinData data)
    {
        ManagerDirector.PushScene(WinController.WIN_SCENE_NAME, data);
    }

    public void PopupLose(WinData data)
    {
        ManagerDirector.PushScene(LoseController.LOSE_SCENE_NAME, data);
    }
}