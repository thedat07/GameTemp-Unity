using UnityEngine;
using LibraryGame;

public class InfoQuest
{
    // Level yêu cầu để mở khóa nhiệm vụ
    protected int m_LevelUnlock = 0;

    // Giá trị tối đa nhiệm vụ
    protected int m_MaxValue;

    // Kiểu nhiệm vụ
    public TypeQuest type;

    // Constructor
    public InfoQuest(TypeQuest type, int maxValue, int levelUnlock = int.MaxValue)
    {
        this.type = type;
        this.m_MaxValue = maxValue;
        this.m_LevelUnlock = levelUnlock;
    }

    // Kiểm tra điều kiện mở khóa nhiệm vụ
    protected virtual bool UnLock()
    {
        return GameManager.Instance.GetMasterData().dataStage.Get() >= m_MaxValue;
    }

    // GET: Lấy giá trị hiện tại
    public int Get()
    {
        return LibraryGameSave.LoadQuestData(type, "value", 0);
    }

    // POST: Cộng thêm giá trị (nếu đã mở khóa)
    public virtual void Post(int amount)
    {
        if (UnLock())
        {
            int newValue = Mathf.Clamp(Get() + amount, 0, m_MaxValue);
            Put(newValue);
        }
    }

    // PUT: Gán giá trị mới (với giới hạn min/max)
    public virtual void Put(int value)
    {
        value = Mathf.Clamp(value, 0, m_MaxValue);
        LibraryGameSave.SaveQuestData(type, "value", value);
    }

    // DELETE: Reset nhiệm vụ về 0
    public virtual void Delete()
    {
        LibraryGameSave.SaveQuestData(type, "value", 0);
    }

    // GET: Giá trị tối đa
    public virtual int GetMax() => m_MaxValue;

    // PUT: Cập nhật giá trị tối đa
    public virtual void SetMax(int value) => m_MaxValue = value;
}
