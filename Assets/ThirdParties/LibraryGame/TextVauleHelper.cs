using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public static class TextVauleHelper
{
    public static void SetText(this TextMeshProUGUI target, int value, string fromat = "{0}")
    {
        target.text = string.Format(fromat, AbbrevationUtility.AbbreviateNumber(value));
    }
}

public static class AbbrevationUtility
{
    public static string AbbreviateNumber(float value, string format = "{0:0.0}{1}")
    {
        string data = "kMGTPE";
        if (value < 1000) return "" + value;
        int exp = (int)(Mathf.Log(value) / Mathf.Log(1000));
        return string.Format(format, value / Mathf.Pow(1000, exp), data[exp - 1]);
    }

    public static int ConvertAbbreviatedToNumber(string abbreviatedValue)
    {
        // Danh sách các ký tự đại diện cho các bậc rút gọn
        string data = "kMGTPE";

        // Loại bỏ khoảng trắng
        abbreviatedValue = abbreviatedValue.Trim();

        // Lấy ký tự cuối cùng
        char suffix = abbreviatedValue[abbreviatedValue.Length - 1];

        // Kiểm tra nếu ký tự cuối cùng là một chữ cái (đại diện cho bậc rút gọn)
        if (char.IsLetter(suffix))
        {
            // Tìm vị trí của ký tự trong chuỗi "data" để xác định bậc của nó
            int exp = data.IndexOf(suffix) + 1;

            // Lấy phần số của chuỗi (bỏ đi ký tự cuối cùng)
            float number;
            if (float.TryParse(abbreviatedValue.Substring(0, abbreviatedValue.Length - 1), out number))
            {
                // Tính lại giá trị gốc bằng cách nhân với 1000^exp
                return ((int)(number * Mathf.Pow(1000, exp)));
            }
            else
            {
                throw new FormatException("Định dạng chuỗi không hợp lệ.");
            }
        }
        else
        {
            // Nếu không có ký tự rút gọn, chỉ cần chuyển đổi về số
            float number;
            if (float.TryParse(abbreviatedValue, out number))
            {
                return ((int)number);
            }
            else
            {
                throw new FormatException("Định dạng chuỗi không hợp lệ.");
            }
        }
    }
}