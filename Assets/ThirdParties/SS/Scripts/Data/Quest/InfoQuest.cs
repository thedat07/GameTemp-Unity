using UnityEngine;
using LibraryGame;

public class InfoQuest
{
    // Level yêu cầu để mở khóa nhiệm vụ
    protected int m_LevelUnlock = 0;

    // Giá trị tối đa nhiệm vụ
    protected int m_MaxValue;

    // Kiểu nhiệm vụ
    protected TypeQuest m_Type;

    // Constructor
    public InfoQuest(TypeQuest type, int maxValue, int levelUnlock = 0)
    {
        this.m_Type = type;
        this.m_MaxValue = maxValue;
        this.m_LevelUnlock = levelUnlock;
    }

    /// <summary>
    /// Kiểm tra điều kiện mở khóa nhiệm vụ (so sánh stage hiện tại)
    /// </summary>
    protected virtual bool CanUnlock()
    {
        return GameManager.Instance.GetMasterData().dataStage.Get() >= m_LevelUnlock;
    }

    /// <summary>
    /// GET: Lấy giá trị hiện tại
    /// </summary>
    public int Get()
    {
        return LibraryGameSave.GetQuest(m_Type, "value", 0);
    }

    /// <summary>
    /// POST: Cộng thêm giá trị (chỉ nếu đã mở khóa)
    /// </summary>
    public virtual void Post(int amount)
    {
        if (!CanUnlock()) return;

        int newValue = Mathf.Clamp(Get() + amount, 0, m_MaxValue);
        Put(newValue);
    }

    /// <summary>
    /// PUT: Gán giá trị mới (có kiểm tra min/max)
    /// </summary>
    public virtual void Put(int value)
    {
        value = Mathf.Clamp(value, 0, m_MaxValue);
        LibraryGameSave.PutQuest(m_Type, "value", value);
    }

    /// <summary>
    /// DELETE: Reset nhiệm vụ về 0
    /// </summary>
    public virtual void Delete()
    {
        LibraryGameSave.DeleteQuest(m_Type, "value");
    }

    /// <summary>
    /// GET: Giá trị tối đa
    /// </summary>
    public virtual int GetMax() => m_MaxValue;

    /// <summary>
    /// PUT: Cập nhật giá trị tối đa
    /// </summary>
    public virtual void SetMax(int value) => m_MaxValue = value;

    /// <summary>
    /// GET: Lấy level yêu cầu mở khóa
    /// </summary>
    public int GetUnlockLevel() => m_LevelUnlock;

    /// <summary>
    /// PUT: Cập nhật level yêu cầu mở khóa
    /// </summary>
    public void SetUnlockLevel(int level) => m_LevelUnlock = level;

    /// <summary>
    /// GET: Loại nhiệm vụ
    /// </summary>
    public TypeQuest GetQuestType() => m_Type;
}
