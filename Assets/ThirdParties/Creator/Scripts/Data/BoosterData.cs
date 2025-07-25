using UnityUtilities;
using UniRx;

[System.Serializable]
public class BoosterData : MasterDataBase
{
    protected int m_LevelUnlock;

    public BoosterData(int defaultValue, MasterDataType type, int levelUnlock)
        : base(defaultValue, type)
    {
        this.m_LevelUnlock = levelUnlock;
    }

    public int GetLevelUnlock()
    {
        return m_LevelUnlock;
    }

    public bool IsLock()
    {
        return SaveExtensions.GetMaster<bool>(m_Type, "IsLock", true);
    }

    public void UnLock()
    {
        SaveExtensions.PutMaster<bool>(m_Type, "IsLock", false);
    }
}
