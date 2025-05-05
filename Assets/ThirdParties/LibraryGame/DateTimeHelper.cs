using System;

public static class DateTimeHelper
{
    public static int AdjustedDayOfWeek(DateTime date)
    {
        int dayOfWeekAsInt = (int)date.DayOfWeek;
        int adjustedDayOfWeek = dayOfWeekAsInt == 0 ? 7 : dayOfWeekAsInt;
        return adjustedDayOfWeek;
    }

    public static DateTime AddHoursConvent(this DateTime target, int hours)
    {
        DateTime date = DateTime.Now + System.TimeSpan.FromHours(hours) - DateTime.Now.TimeOfDay;
        return date;
    }

    public static DateTime AddDaysConvent(this DateTime target, int days)
    {
        DateTime date = DateTime.Now + System.TimeSpan.FromDays(days) - DateTime.Now.TimeOfDay;
        return date;
    }

    public static DateTime FindFirstDateOfTheWeek(DateTime dateTime)
    {
        int daysOffset = (int)dateTime.DayOfWeek - 1;
        if (daysOffset < 0) daysOffset += 7;
        return dateTime.AddDays(-daysOffset) - DateTime.Now.TimeOfDay;
    }

    public static DateTime FindLastDateOfTheWeek(DateTime dateTime)
    {
        return dateTime.AddDays(7 - (int)dateTime.DayOfWeek);
    }


    public static DateTime FindLastDateOfTheMonth(DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddDays(-1);
    }


    public static DateTime FindFirstDateOfTheMonth(DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, 1);
    }


    public static DateTime FindLastDateOfTheYear(DateTime dateTime)
    {
        return new DateTime(dateTime.Year, 12, 31);
    }


    public static DateTime FindFirstDateOfTheYear(DateTime dateTime)
    {
        return new DateTime(dateTime.Year, 1, 1);
    }
}