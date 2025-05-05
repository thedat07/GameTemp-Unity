
using UnityEngine;
using SS.View;
using UnityEngine.UI;
using DG.Tweening;
using Lean.Pool;
using System.Collections.Generic;
using TMPro;

public class PopupMaskController : Controller
{
    public const string POPUPTUTOR_SCENE_NAME = "PopupMask";

    public override string SceneName()
    {
        return POPUPTUTOR_SCENE_NAME;
    }

    [Header("Mask")]
    public CanvasGroup canvasMask;
    public LeanGameObjectPool point;
    public RectTransform contentPoint;

    [Header("Text")]
    public GameObject txtPanel;
    public TextMeshProUGUI txtInfo;

    [Header("Hand")]
    public GameObject hand;
    public Image mask;

    Camera m_Cam;

    private List<PointMask> pointMasks;

    private List<PopupMaskData> PopupMaskDatas;

    private int m_Step;

    public float GetSize()
    {
        if (m_Cam)
            return Canvas.worldCamera.orthographicSize / m_Cam.orthographicSize;
        else return 0;
    }

    private void SetCam(SettingCamTutor settingCamTutor)
    {
        if (settingCamTutor.baseCam != null)
        {
            m_Cam = settingCamTutor.baseCam;
            Canvas.worldCamera.transform.position = m_Cam.transform.position;
        }
        else
        {
            m_Cam = Manager.Object.UICamera;
            Canvas.worldCamera.transform.position = Manager.Object.UICamera.transform.position;
        }
    }

    public void OnShow(PopupMaskData data)
    {
        ClearMask();
        OnShow();
        PopupMaskDatas = new List<PopupMaskData>();
        View(data);
    }

    public void OnShow(List<PopupMaskData> datas)
    {
        ClearMask();
        OnShow();
        PopupMaskDatas.AddRange(datas);
        m_Step = 0;
        View(PopupMaskDatas[m_Step]);
    }

    private void View(PopupMaskData data)
    {
        data.callback?.Invoke();

        mask.color = new Color(0, 0, 0, data.mask);

        SetCam(data.settingCam);

        if (data.point != null)
            txtPanel.transform.position = data.point.transform.position;
        else
            txtPanel.transform.position = transform.position;

        UpdateText(data.info);
        foreach (var item in data.lstInfo)
        {
            SetMaskUI(item.Item1, item.Item2, item.Item3);
        }
        ShowHand(data.showHand, data.flipHand);
    }

    public bool UpdateStep()
    {
        point.DespawnAll();
        pointMasks = new List<PointMask>();
        m_Step++;
        if (m_Step < PopupMaskDatas.Count)
        {
            View(PopupMaskDatas[m_Step]);
            return true;
        }
        return false;
    }

    void UpdateText(string text = "")
    {
        if (text == "")
        {
            txtPanel.gameObject.SetActive(false);
        }
        else
        {
            txtPanel.gameObject.SetActive(true);
        }
        txtInfo.text = text;
    }

    void ClearMask()
    {
        point.DespawnAll();
        pointMasks = new List<PointMask>();
        PopupMaskDatas = new List<PopupMaskData>();
    }

    void ShowHand(bool show, bool flipHand)
    {
        hand.SetActive(show);
        if (show == true)
        {
            if (pointMasks != null && pointMasks.Count > 0)
            {
                hand.transform.position = pointMasks[0].transform.position;
                hand.transform.DOScale(new Vector3(!flipHand ? 1 : -1, 1, 1), 0.25f).SetEase(Ease.OutBack).SetLink(gameObject, LinkBehaviour.KillOnDisable).From(0);
            }
            else
            {
                hand.SetActive(false);
            }
        }
    }

    void OnShow()
    {
        gameObject.SetActive(true);
        canvasMask.DOFade(1, 0.2f).From(0).SetEase(Ease.Flash).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
    }

    public void OnHide()
    {
        canvasMask.DOFade(0, 0.2f).From(1).SetEase(Ease.Flash).SetLink(gameObject, LinkBehaviour.KillOnDestroy).OnComplete(() =>
        {
            ClearMask();
            gameObject.SetActive(false);
        });
    }

    public void SetMaskUI(Sprite sprite, Transform pointSpawn, float scale)
    {
        var pointMask = point.Spawn(contentPoint).GetComponent<PointMask>();
        pointMask.Init(pointSpawn, this, sprite, scale);
        pointMask.transform.localScale = Vector3.one;
        pointMasks.Add(pointMask);
    }


    public bool IsPointAllowed(Vector2 screenPoint, Camera camera)
    {
        if (
            screenPoint.x < 0 || screenPoint.x > Screen.width ||
            screenPoint.y < 0 || screenPoint.y > Screen.height)
        {
            return false;
        }

        foreach (var area in pointMasks)
        {
            if (area != null)
            {
                bool isInside = RectTransformUtility.RectangleContainsScreenPoint(
                                area.GetRectMasking(),       // RectTransform bạn muốn kiểm tra
                                screenPoint,         // Vector2 screen point
                                camera               // Camera (thường là camera UI hoặc Camera.main)
                            );

                if (isInside)
                {
                    return true;
                }
            }
        }

        return false;


    }
}