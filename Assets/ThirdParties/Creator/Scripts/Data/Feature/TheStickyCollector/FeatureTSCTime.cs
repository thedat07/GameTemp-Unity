using UnityEngine;
using UnityUtilities;
using System;

public partial class FeatureTheStickyCollector
{
    public void CheckTimeReset()
    {
        DateTime now = NetworkTime.Now; // hoặc DateTime.Now

        // Tìm tuần hiện tại (Thứ Hai 00:00 đến Chủ Nhật 23:59)
        DateTime currentWeekStart = DateTimeExtensions.FindFirstDateOfTheWeek(now).Date;
        DateTime currentWeekEnd = DateTimeExtensions.FindLastDateOfTheWeek(now).Date.AddDays(1).AddTicks(-1);

        // Nếu chưa từng reset thì gán tuần hiện tại
        if (m_LastResetTime == default)
        {
            m_LastResetTime = now;
            UnityEngine.Console.Log("FeatureTheStickyCollector", "First time setup, event bắt đầu từ tuần hiện tại.");
            return;
        }

        // Nếu lastReset nằm ở tuần trước → reset
        bool isInCurrentWeek = m_LastResetTime >= currentWeekStart && m_LastResetTime <= currentWeekEnd;
        if (!isInCurrentWeek)
        {
            ResetFeature();
            // Reset logic ở đây
            UnityEngine.Console.Log("FeatureTheStickyCollector", "Reset tuần mới!");

            // Cập nhật lại lastResetTime = tuần hiện tại
            m_LastResetTime = now;
        }
        else
        {
            UnityEngine.Console.Log("FeatureTheStickyCollector", "Vẫn còn trong tuần này, không cần reset.");
        }
    }
}

public static class WeeklyEventHelper
{
    /// <summary>
    /// Trả về khoảng thời gian event tuần (Thứ Hai 00:00 → Chủ Nhật 23:59:59)
    /// Nếu user vào giữa tuần thì vẫn tính đến cuối tuần đó.
    /// </summary>
    public static (DateTime start, DateTime end) GetWeeklyEventRange(DateTime userJoinTime)
    {
        // Lấy ngày đầu tuần (Thứ Hai 00:00)
        DateTime weekStart = DateTimeExtensions.FindFirstDateOfTheWeek(userJoinTime).Date;

        // Lấy ngày cuối tuần (Chủ Nhật 23:59:59)
        DateTime weekEnd = DateTimeExtensions.FindLastDateOfTheWeek(userJoinTime).Date
                            .AddDays(1).AddTicks(-1); // 23:59:59.9999999

        // Nếu userJoinTime > weekStart thì sự kiện bắt đầu từ lúc userJoinTime
        DateTime eventStart = userJoinTime.Date < weekStart ? weekStart : userJoinTime.Date;

        return (eventStart, weekEnd);
    }

    /// <summary>
    /// Lấy số ngày còn lại trong tuần event (bao gồm ngày hiện tại).
    /// </summary>
    public static int GetRemainingDaysInWeek(DateTime userJoinTime)
    {
        var range = GetWeeklyEventRange(userJoinTime);
        return (range.end.Date - userJoinTime.Date).Days + 1;
    }
}