using Creator;
using UnityEngine;

public partial class WinController
{
    [System.Serializable]
    public class NewFeature
    {
        public int levelUnlock;
        public Sprite icon;
    }

    public NewFeature[] newFeatures;

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