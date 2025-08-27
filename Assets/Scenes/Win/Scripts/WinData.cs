using Creator;

public class WinData
{
    public WinData()
    {

    }
}

public partial class WinController
{
    public override void OnActive(object data)
    {
        if (data != null)
        {
            m_Data = data as WinData;
        }

        InitData();
    }

    void InitData()
    {
        m_DataLog = GameManager.Instance.log;
        m_Piggy = GameManager.Instance.GetFeatureData().featurePiggyBank;
    }
}