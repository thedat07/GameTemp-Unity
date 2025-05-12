using System;
using DesignPatterns;
using LibraryGame;
using UnityEngine;

[System.Serializable]
public class MasterDataBase
{
    // Giá trị mặc định khi khởi tạo (dùng nếu chưa có dữ liệu lưu)
    protected int m_DefaultValue;

    // Kiểu dữ liệu Master tương ứng (dùng làm key lưu trữ)
    public MasterDataType m_Type;

    // Constructor
    public MasterDataBase(int defaultValue, MasterDataType type)
    {
        this.m_Type = type;
        this.m_DefaultValue = defaultValue;
    }

    /// <summary>
    /// GET: Lấy giá trị hiện tại từ hệ thống lưu
    /// </summary>
    public int Get()
    {
        return LibraryGameSave.GetMaster(m_Type, "value", m_DefaultValue);
    }

    /// <summary>
    /// POST: Cộng thêm vào giá trị hiện tại
    /// </summary>
    public virtual void Post(int amount)
    {
        int newValue = Mathf.Clamp(Get() + amount, 0, int.MaxValue);
        Put(newValue);
    }

    /// <summary>
    /// PUT: Gán giá trị mới trực tiếp
    /// </summary>
    public virtual void Put(int newValue)
    {
        newValue = Mathf.Clamp(newValue, 0, int.MaxValue);
        LibraryGameSave.PutMaster(m_Type, "value", newValue);
    }

    /// <summary>
    /// DELETE: Reset giá trị về 0
    /// </summary>
    public virtual void Delete()
    {
        LibraryGameSave.PutMaster(m_Type, "value", 0);
    }

    /// <summary>
    /// GET: Trả về kiểu dữ liệu Master
    /// </summary>
    public MasterDataType GetDataType() => m_Type;

    /// <summary>
    /// GET: Trả về giá trị mặc định
    /// </summary>
    public int GetDefaultValue() => m_DefaultValue;
}
