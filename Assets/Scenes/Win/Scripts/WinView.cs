using Creator;

public partial class WinController
{
    void ViewPopup()
    {
        ShowPopupPiggy();
    }

    void ShowPopupPiggy()
    {
        if (m_DataLog.logPlayDone % 2 == 0 && m_Piggy.IsFull())
        {
            Creator.Director.PushScene(PiggyBankController.PIGGYBANK_SCENE_NAME);
        }
    }
}