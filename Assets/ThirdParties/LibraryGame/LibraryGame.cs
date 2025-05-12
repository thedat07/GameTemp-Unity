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

    public static class Game
    {
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

        public static bool EditCanvasScaler(this CanvasScaler canvasScaler)
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
                    return true;
                }
            }

            return false;
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
    }
}