using System;
using TMPro;
using UnityEngine.UI;

public static class TextTimeHelper
{
    public static bool SetTextNow(this Text target, DateTime timeEnd, string formatD = "{0}d {1}h", string formatH = "{0}h {1}m", string formatM = "{0}m {1}s", string formatZero = "--:--")
    {
        var time = timeEnd - DateTime.Now;
        return target.SetText(time, formatD, formatH, formatM, formatZero);
    }

    public static bool SetText(this Text target, System.TimeSpan time, string formatD = "{0}d {1}h", string formatH = "{0}h {1}m", string formatM = "{0}m {1}s", string formatZero = "--:--")
    {
        string data;

        if (time.TotalSeconds <= 0)
        {
            data = formatZero;
            target.text = data;
            return false;
        }

        if (time.TotalDays >= 1)
            data = string.Format(formatD, time.Days, time.Hours);
        else if (time.TotalHours >= 1)
            data = string.Format(formatH, time.Hours, time.Minutes);
        else
            data = string.Format(formatM, time.Minutes, time.Seconds);

        target.text = data;
        return true;
    }

    public static bool SetText(this TextMeshProUGUI target, System.TimeSpan time, string formatD = "{0}d {1}h", string formatH = "{0}h {1}m", string formatM = "{0}m {1}s", string formatZero = "--:--")
    {
        string data;

        if (time.TotalSeconds <= 0)
        {
            data = formatZero;
            target.text = data;
            return false;
        }

        if (time.TotalDays >= 1)
            data = string.Format(formatD, time.Days, time.Hours);
        else if (time.TotalHours >= 1)
            data = string.Format(formatH, time.Hours, time.Minutes);
        else
            data = string.Format(formatM, time.Minutes, time.Seconds);

        target.text = data;
        return true;
    }
}