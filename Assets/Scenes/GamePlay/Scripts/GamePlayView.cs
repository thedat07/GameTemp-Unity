
using UnityEngine;
using Creator;
using TMPro;

public partial class GamePlayController
{
    [Header("View")]

    [SerializeField] TextMeshPro m_TxtLevel;

    [SerializeField] TextMeshPro m_TxtTime;


    void View()
    {
        int level = GameManager.Instance.GetMasterData().dataStage.Get();

        if (m_TxtLevel) m_TxtLevel.text = string.Format(string.Format("Level {0}", level));
    }

    void ViewTime(int second)
    {
        if (m_TxtTime)
        {
            if (second > 0)
            {
                int minutes = second / 60;
                int seconds = second % 60;
                m_TxtTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
            else
            {
                m_TxtTime.text = string.Format("00:00");
            }
        }
    }
}