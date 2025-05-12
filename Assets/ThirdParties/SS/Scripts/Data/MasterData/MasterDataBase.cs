using System;
using DesignPatterns;
using LibraryGame;
using UnityEngine;

[System.Serializable]
public class MasterDataBase
{
    // Giá trị mặc định khi khởi tạo, chỉ dùng nếu không có dữ liệu lưu trước đó
    protected int m_value;

    // Kiểu dữ liệu Master tương ứng (dùng để xác định khóa khi lưu)
    public MasterDataType type;

    // Hàm khởi tạo
    public MasterDataBase(int value, MasterDataType type)
    {
        this.type = type;
        this.m_value = value;
    }

    // GET - Lấy giá trị hiện tại từ hệ thống lưu trữ
    public int Get()
    {
        return LibraryGameSave.LoadMasterData(type, "value", m_value);
    }

    // POST - Cộng thêm vào giá trị hiện tại
    public virtual void Post(int amount)
    {
        // Tính giá trị mới bằng cách cộng thêm và giới hạn trong phạm vi hợp lệ
        int newValue = Mathf.Clamp(Get() + amount, 0, int.MaxValue);
        Put(newValue); // Gọi hàm PUT để lưu giá trị mới
    }

    // PUT - Gán giá trị mới trực tiếp
    public virtual void Put(int newValue)
    {
        // Đảm bảo giá trị hợp lệ trước khi lưu
        newValue = Mathf.Clamp(newValue, 0, int.MaxValue);
        LibraryGameSave.SaveMasterData(type, "value", newValue);
    }

    // DELETE - Đặt lại giá trị về 0 (tuỳ chọn thêm cho trường hợp cần reset)
    public virtual void Delete()
    {
        LibraryGameSave.SaveMasterData(type, "value", 0);
    }
}
