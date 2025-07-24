using UnityEngine;
using UniRx;
using UnityUtilities;

[System.Serializable]
public class MasterDataBase
{
    // Giá trị mặc định khi khởi tạo (nếu chưa có dữ liệu được lưu trước đó)
    protected int m_DefaultValue;

    // Kiểu dữ liệu Master (dùng làm key để lưu và truy xuất dữ liệu)
    public MasterDataType m_Type;

    // Thuộc tính reactive giúp theo dõi sự thay đổi của giá trị
    // Có thể dùng Subscribe để lắng nghe thay đổi (rất hữu ích cho UI hoặc gameplay)
    private ReactiveProperty<int> _value;

    // Chỉ cho phép đọc từ bên ngoài (readonly)
    public IReadOnlyReactiveProperty<int> Value => _value;

    /// <summary>
    /// Constructor khởi tạo dữ liệu
    /// </summary>
    /// <param name="defaultValue">Giá trị mặc định nếu chưa có dữ liệu lưu</param>
    /// <param name="type">Kiểu dữ liệu master</param>
    public MasterDataBase(int defaultValue, MasterDataType type)
    {
        this.m_Type = type;
        this.m_DefaultValue = defaultValue;

        // Lấy giá trị đã lưu (nếu có), nếu chưa thì dùng default
        int savedValue = SaveExtensions.GetMaster(m_Type, "value", defaultValue);

        // Gán vào reactive property
        _value = new ReactiveProperty<int>(savedValue);
    }

    /// <summary>
    /// Lấy giá trị hiện tại (dùng như getter thông thường)
    /// </summary>
    public int Get() => _value.Value;

    /// <summary>
    /// POST: Cộng thêm vào giá trị hiện tại
    /// Dùng để tăng điểm, tiền, level,... 
    /// </summary>
    public virtual void Post(int amount)
    {
        int newValue = Mathf.Clamp(_value.Value + amount, 0, int.MaxValue);
        Put(newValue); // Gọi Put để cập nhật giá trị và notify observer
    }

    /// <summary>
    /// PUT: Gán giá trị mới trực tiếp
    /// </summary>
    public virtual void Put(int newValue)
    {
        newValue = Mathf.Clamp(newValue, 0, int.MaxValue);

        // Lưu vào hệ thống lưu trữ
        SaveExtensions.PutMaster(m_Type, "value", newValue);

        // Gán cho ReactiveProperty => các observer sẽ được notify
        _value.Value = newValue;
    }

    /// <summary>
    /// DELETE: Reset giá trị về 0
    /// </summary>
    public virtual void Delete()
    {
        SaveExtensions.PutMaster(m_Type, "value", 0);
        _value.Value = 0;
    }

    /// <summary>
    /// Lấy kiểu dữ liệu master đang dùng
    /// </summary>
    public MasterDataType GetDataType() => m_Type;

    /// <summary>
    /// Lấy giá trị mặc định
    /// </summary>
    public int GetDefaultValue() => m_DefaultValue;

    /*
        stageData.Value
        .Subscribe(newVal => Debug.Log("Stage changed: " + newVal))
        .AddTo(this);
    */
}
