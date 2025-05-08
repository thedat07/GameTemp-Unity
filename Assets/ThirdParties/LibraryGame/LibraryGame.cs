using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;
using System.Linq;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using LibraryGame;

public class GachaItem
{
    public int index;
    public int dropRate;

    public GachaItem(int index, int dropRate)
    {
        this.index = index;
        this.dropRate = dropRate;
    }
}

[System.Serializable]
public class ImageOnOff
{
    public Image image;
    public Sprite on;
    public Sprite off;
    public bool isOn;

    public void Set(bool isOn)
    {
        if (image)
        {
            this.isOn = isOn;
            if (this.isOn)
            {
                image.sprite = on;
            }
            else
            {
                image.sprite = off;
            }
        }
    }
}

[System.Serializable]
public class TextOnOff
{
    public TextMeshProUGUI txt;
    public Color on;
    public Color off;
    public bool isOn;

    public void Set(bool isOn)
    {
        if (txt)
        {
            this.isOn = isOn;
            if (this.isOn)
            {
                txt.color = on;
            }
            else
            {
                txt.color = off;
            }
        }
    }
}

[System.Serializable]
public class ObjectOnOff
{
    public GameObject on;
    public GameObject off;
    public bool isOn;

    public void Set(bool isOn)
    {
        on.SetActive(false);
        off.SetActive(false);

        this.isOn = isOn;

        on.SetActive(this.isOn);
        off.SetActive(!this.isOn);
    }
}


namespace LibraryGame
{
    public class Canvas
    {
        public static Vector2 WorldToScreenPoint(Camera camera, RectTransform area, Vector3 point)
        {
            Vector2 screenPosition = camera.WorldToScreenPoint(point);

            screenPosition.x *= area.rect.width / (float)camera.pixelWidth;
            screenPosition.y *= area.rect.height / (float)camera.pixelHeight;

            return screenPosition - area.sizeDelta / 2f; ;
        }

        public static Vector3 WorldToScreenSpace(Vector3 worldPos, Camera camMain, RectTransform area, float size)
        {
            Vector3 screenPoint = camMain.WorldToScreenPoint(worldPos);
            screenPoint.z = 0;

            Vector2 screenPos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(area, screenPoint, camMain, out screenPos))
            {
                return screenPos * size;
            }

            return screenPoint * size;
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

    public static class Game
    {
        public static void UpdateCollider(this BoxCollider2D boxCollider, Sprite sprite)
        {
            Vector2 spriteSize = sprite.bounds.size;
            boxCollider.size = spriteSize;
            boxCollider.offset = sprite.bounds.center;
        }

        public static string CapitalizeFirstLetter(string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            // Chuyển chữ cái đầu tiên thành chữ hoa và nối với phần còn lại của chuỗi
            return char.ToUpper(str[0]) + str.Substring(1);
        }

        public static float ScaleScreen(float defaultScale, float defaultScaleTable, Vector2 sreen)
        {
            float max = MathF.Max(sreen.y, sreen.x);

            float maxDefault = MathF.Max(SettingPresenter.ScreenGame.y, SettingPresenter.ScreenGame.x);

            return defaultScaleTable / (max / maxDefault);
        }

        public static void ResizeCamera(this Camera cam, float minCam, float maxCam)
        {
            float height = (float)Screen.height;
            float width = (float)Screen.width;
            float defaultSize = cam.orthographicSize;

            float ratio = width / height;
            float ratioDefault = SettingPresenter.ScreenGame.x / SettingPresenter.ScreenGame.y;
            if (ratio < ratioDefault)
            {
                cam.orthographicSize = Mathf.Clamp(defaultSize / (ratio / ratioDefault), minCam, maxCam);
            }
        }

        public static void ReSize(this Image target)
        {
            if (target.sprite != null)
            {
                Vector2 spriteOriginSize = target.sprite.rect.size;
                target.rectTransform.sizeDelta = spriteOriginSize;
            }
        }

        public static void ReSize(this Image target, RectTransform rt, float time = 0, float scale = 1)
        {
            if (target.sprite != null)
            {
                Vector2 spriteOriginSize = target.sprite.rect.size;
                float ratio = rt.sizeDelta.x;
                float ratioDefault = SettingPresenter.ScreenGame.x;
                spriteOriginSize.x = (spriteOriginSize.x / (ratioDefault / ratio)) * scale;
                target.rectTransform.DOSizeDelta(spriteOriginSize, time);
            }
        }

        public static void ReSizeX(this Image target, RectTransform rt)
        {
            if (target.sprite != null)
            {
                Vector2 size = rt.sizeDelta;
                float ratio = (target.sprite.rect.size.y) / (target.sprite.rect.size.x * 1f);
                size = new Vector2(size.x, size.x * ratio);
                rt.sizeDelta = size;
            }
        }

        public static void Number(this TextMeshProUGUI target, int value)
        {
            target.text = string.Format("{0}", AbbrevationUtility.AbbreviateNumber(value));
        }

        public static void Number2(this TextMeshProUGUI target, int value)
        {
            target.text = string.Format("{0}", AbbrevationUtility.AbbreviateNumber(value, "{0:0}{1}"));
        }

        public static string GetDay(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        public static void Update(this Slider target, float min, float max, float value)
        {
            if (value < min)
            {
                target.fillRect.gameObject.SetActive(false);
                target.value = 0;
            }
            else
            {
                target.fillRect.gameObject.SetActive(true);
                target.value = Mathf.Clamp(value, min, max);
            }
        }

        public static bool TimeSpanNow(this Text target, DateTime timeEnd, string formatD = "{0}d {1}h", string formatH = "{0}h {1}m", string formatM = "{0}m {1}s", string formatZero = "--:--")
        {
            var time = timeEnd - DateTime.Now;
            return target.TimeSpan(time, formatD, formatH, formatM, formatZero);
        }

        public static bool TimeSpan(this Text target, System.TimeSpan time, string formatD = "{0}d {1}h", string formatH = "{0}h {1}m", string formatM = "{0}m {1}s", string formatZero = "--:--")
        {
            string data = "";

            if (time.TotalDays > 1)
            {
                data = string.Format(formatD, time.Days, time.Hours);
            }
            else
            {
                if (time.TotalHours > 1)
                {
                    data = string.Format(formatH, time.Hours, time.Minutes);
                }
                else
                {
                    data = string.Format(formatM, time.Minutes <= 0 ? 0 : time.Minutes, time.Seconds <= 0 ? 0 : time.Seconds);
                }
            }

            if (time.TotalSeconds <= 0)
            {
                data = formatZero;
            }

            target.text = data;

            return (time.Minutes <= 0 && time.Seconds <= 0 ? false : true);
        }

        public static bool TimeSpan(this TextMeshProUGUI target, System.TimeSpan time, string formatD = "{0}d {1}h", string formatH = "{0}h {1}m", string formatM = "{0}m {1}s", string formatZero = "--:--")
        {
            string data = "";

            if (time.TotalDays > 1)
            {
                data = string.Format(formatD, time.Days, time.Hours);
            }
            else
            {
                if (time.TotalHours > 1)
                {
                    data = string.Format(formatH, time.Hours, time.Minutes);
                }
                else
                {
                    data = string.Format(formatM, time.Minutes <= 0 ? 0 : time.Minutes, time.Seconds <= 0 ? 0 : time.Seconds);
                }
            }

            if (time.TotalSeconds <= 0)
            {
                data = formatZero;
            }

            target.text = data;

            return (time.Minutes <= 0 && time.Seconds <= 0 ? false : true);
        }

        public static void PriceVND(this TextMeshProUGUI target, int value)
        {
            target.text = string.Format("{0:N0} d", value);
        }

        public static void EditCanvasScaler(this CanvasScaler canvasScaler)
        {
            float ratio = (float)Screen.width / Screen.height;
            float defaultRatio = SettingPresenter.ScreenGame.x / SettingPresenter.ScreenGame.y;

            if (ratio < defaultRatio)
            {
                float match = ratio / defaultRatio;
                match = (DeviceTypeChecker.GetDeviceType() == ENUM_Device_Type.Phone)
                    ? Mathf.Clamp(1 - (1 / match), 0, 1)
                    : Mathf.Clamp(1 / match, 0, 1);

                bool update = canvasScaler.matchWidthOrHeight != match;
                
                canvasScaler.matchWidthOrHeight = match;

                if (update)
                {
                    if (canvasScaler.TryGetComponent<RectTransform>(out RectTransform rootRect))
                    {
                        LayoutRebuilder.ForceRebuildLayoutImmediate(rootRect);
                    }
                }
            }
        }

        public static float GetScaleScreen()
        {
            float height = (float)Screen.height;
            float width = (float)Screen.width;

            float ratio = width / height;
            float ratioDefault = SettingPresenter.ScreenGame.x / SettingPresenter.ScreenGame.y;
            if (ratio < ratioDefault)
            {
                if (DeviceTypeChecker.GetDeviceType() == ENUM_Device_Type.Phone)
                {
                    return Mathf.Clamp(1 - (1 / (ratio / ratioDefault)), 0, 1);
                }
                else
                {
                    return Mathf.Clamp(1 / (ratio / ratioDefault), 0, 1);
                }
            }
            return 1;
        }

        public static void DoProgressVaule(this Slider target, float value)
        {
            if (value == 0)
            {
                target.fillRect.gameObject.SetActive(false);
            }
            else
            {
                target.fillRect.gameObject.SetActive(true);
                target.DOValue(Mathf.Clamp(value, target.maxValue / 200, target.maxValue), 0.2f);
            }
        }

        public static void SetGameLayerRecursive(this GameObject _go, int _layer)
        {
            _go.layer = _layer;
            foreach (Transform child in _go.transform)
            {
                child.gameObject.layer = _layer;

                Transform _HasChildren = child.GetComponentInChildren<Transform>();
                if (_HasChildren != null)
                    SetGameLayerRecursive(child.gameObject, _layer);

            }
        }

#if UNITY_EDITOR
        public static void DrawTexture(this SpriteRenderer target, float w, float h)
        {
            if (target.sprite != null)
            {
                Texture2D texture = UnityEditor.AssetPreview.GetAssetPreview(target.sprite);
                //We crate empty space 80x80 (you may need to tweak it to scale better your sprite
                //This allows us to place the image JUST UNDER our default inspector
                GUILayout.Label("", GUILayout.Height(50), GUILayout.Width(50));
                //Draws the texture where we have defined our Label (empty space)
                if (texture)
                    GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
            }
            else
            {
                GUILayout.Label("Null", GUILayout.Height(50), GUILayout.Width(50));
            }

        }
#endif
        public static Sprite ConvertToSprite(this Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }

        public static Sprite ConvertToSprite(this Texture texture, float width, float height)
        {
            return Sprite.Create(texture as Texture2D, new Rect(0, 0, width, height), Vector2.zero);
        }

#if UNITY_EDITOR
        public static void ChangeTextAssetContent(this TextAsset textAsset, string newContent)
        {
            // Đường dẫn tới tệp TextAsset
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(textAsset);

            // Thay đổi nội dung tệp
            System.IO.StreamWriter writer = new System.IO.StreamWriter(assetPath, false);
            writer.Write(newContent);
            writer.Close();

            // Cập nhật lại tệp trong Project Window
            UnityEditor.AssetDatabase.ImportAsset(assetPath);
        }
#endif


        public static int RollGacha(List<GachaItem> data)
        {
            // Total drop rate
            float totalDropRate = 0f;

            // Calculate the total drop rate
            foreach (var item in data)
            {
                totalDropRate += item.dropRate;
            }

            // Generate a random number between 0 and the total drop rate
            float randomValue = UnityEngine.Random.Range(0f, totalDropRate);

            // Loop through the items in the gacha
            foreach (var item in data)
            {
                // Subtract the drop rate of the current item from the random value
                randomValue -= item.dropRate;

                // If the random value is less than or equal to 0, return the item
                if (randomValue <= 0)
                {
                    return item.index;
                }
            }

            // Return null if no item is obtained
            return -1;
        }

    }

    public class Random<T>
    {
        public static T GetRandomElement(HashSet<T> hashSet)
        {
            int randomIndex = UnityEngine.Random.Range(0, hashSet.Count);
            int currentIndex = 0;
            foreach (T element in hashSet)
            {
                if (currentIndex == randomIndex)
                {
                    return element;
                }
                currentIndex++;
            }
            throw new System.Exception("HashSet is empty or out of bounds");
        }

        public static T GetRandomElement(List<T> list)
        {
            int randomIndex = UnityEngine.Random.Range(0, list.Count);
            return list[randomIndex];
        }

        public static List<T> GetRandom(List<T> list)
        {
            System.Random random = new System.Random();
            return list.OrderBy(x => random.Next()).ToList();
        }
    }
}