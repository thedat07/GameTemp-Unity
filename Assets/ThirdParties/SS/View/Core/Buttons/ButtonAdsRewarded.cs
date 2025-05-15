using UnityEngine.Events;
using UnityEngine;

/// <summary>
/// Nút hiển thị quảng cáo rewarded và xử lý phần thưởng.
/// </summary>
public class ButtonAdsRewarded : ButtonGame
{
    [Header("Settings")]
    public EAdsPack pack;

    public InfoViewRoot infoViewRoot;

    [Header("Events")]
    public UnityEvent OnSuccess;

    public UnityEvent OnFail;

    public UnityEvent OnCompleted;

    /// <summary>
    /// Dữ liệu quảng cáo được ánh xạ từ pack.
    /// </summary>
    protected AdsInfoPack m_Data;

    /// <summary>
    /// Khởi tạo dữ liệu phần thưởng và cập nhật UI khi nút được kích hoạt.
    /// </summary>
    protected override void StartButton()
    {
        m_Data = GameManager.Instance
                            .GetShopPresenter()
                            .soDataRewards
                            .GetAdsShop(pack);

        infoViewRoot.Init(m_Data);
    }

    /// <summary>
    /// Xử lý khi người dùng click nút: thiết lập callback và gọi xem quảng cáo.
    /// </summary>
    protected override void OnClickEvent()
    {
        // Thiết lập callback trước khi xem quảng cáo
        m_Data.SetData(OnRewardSuccess, OnRewardFail, OnRewardCompleted);
    }

    /// <summary>
    /// Gọi khi người chơi nhận phần thưởng thành công sau khi xem quảng cáo.
    /// </summary>
    private void OnRewardSuccess()
    {
        OnSuccess?.Invoke();
    }

    /// <summary>
    /// Gọi khi quảng cáo thất bại hoặc bị hủy.
    /// </summary>
    private void OnRewardFail()
    {
        OnFail?.Invoke();
    }

    /// <summary>
    /// Gọi khi quảng cáo hoàn tất (thành công hoặc không).
    /// </summary>
    private void OnRewardCompleted()
    {
        OnCompleted?.Invoke();
    }
}

#if UNITY_EDITOR
namespace Lean.Gui.Editor
{
    using UnityEditor;
    using TARGET = ButtonAdsRewarded;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(TARGET))]
    public class ButtonAdsRewarded_Editor : ButtonGame_Editor
    {
        protected override void DrawSelectableSettings()
        {
            base.DrawSelectableSettings();

            Draw("pack", "Gói quảng cáo được liên kết với nút.");

            Draw("infoViewRoot", "UI hiển thị thông tin phần thưởng.");
        }

        protected override void DrawSelectableEvents(bool showUnusedEvents)
        {
            TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

            base.DrawSelectableEvents(showUnusedEvents);

            if (showUnusedEvents == true || Any(tgts, t => t.OnDown.GetPersistentEventCount() > 0))
            {
                Draw("OnSuccess", "Sự kiện được gọi khi người chơi xem quảng cáo thành công.");
            }

            if (showUnusedEvents == true || Any(tgts, t => t.OnClick.GetPersistentEventCount() > 0))
            {
                Draw("OnFail", "Sự kiện được gọi khi quảng cáo bị hủy hoặc lỗi.");
            }

            if (showUnusedEvents == true || Any(tgts, t => t.OnClick.GetPersistentEventCount() > 0))
            {
                Draw("OnCompleted", "Sự kiện được gọi khi quảng cáo kết thúc (bất kể thành công hay thất bại).");
            }
        }
    }
}
#endif