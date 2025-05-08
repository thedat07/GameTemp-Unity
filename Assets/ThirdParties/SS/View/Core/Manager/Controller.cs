using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DesignPatterns;
using DG.Tweening;
using com.cyborgAssets.inspectorButtonPro;

namespace SS.View
{
    public interface IController
    {
        string SceneName();
    }

    public abstract class Controller : MonoBehaviour, IController
    {
        protected GameObject m_Shield;

        [Header("Info")]
        [SerializeField] protected Canvas m_Canvas;
        [SerializeField] protected Camera m_Camera;

        [Header("Setting")]
        public bool FullScreen;
        public bool HasShield = true;
        public bool UseCameraUI = true;
        public bool SafeArea = true;

        [Header("Effect")]
        public SceneAnimation Animation;

        /// <summary>
        /// Each scene must has an unique scene name.
        /// </summary>
        /// <returns>The name.</returns>
        public abstract string SceneName();

        /// <summary>
        /// This event is raised right after this view becomes active after a call of LoadScene() or ReloadScene() or Popup() of SceneManager.
        /// Same OnEnable but included the data which is transfered from the previous view. (Raised after Awake() and OnEnable())
        /// </summary>
        /// <param name="data">Data.</param>
        public virtual void OnActive(object data)
        {
        }

        /// <summary>
        /// This event is raised right after the above view is hidden.
        /// </summary>
        public virtual void OnReFocus()
        {
        }

        /// <summary>
        /// This event is raised right after this view appears and finishes its show-animation.
        /// </summary>
        public virtual void OnShown()
        {
        }

        /// <summary>
        /// This event is raised right after this view finishes its hide-animation and disappears.
        /// </summary>
        public virtual void OnHidden()
        {
        }

        /// <summary>
        /// This event is raised right after player pushs the ESC button on keyboard or Back button on android devices.
        /// You should assign this method to OnClick event of your Close Buttons.
        /// </summary>
        [ProButton]
        public virtual void OnKeyBack()
        {
            Manager.PopScene();
        }


        public Manager.Data Data
        {
            get;
            set;
        }

        public Canvas Canvas
        {
            get
            {
                return m_Canvas;
            }
            set
            {
                m_Canvas = value;
            }
        }

        public Camera Camera
        {
            get
            {
                return m_Camera;
            }
            set
            {
                m_Camera = value;
            }
        }

        public virtual void Show()
        {
            if (Animation)
                Animation.StartShow();
        }

        public virtual void Hide()
        {
            if (Animation)
                Animation.StartHide();
        }

        public virtual void CreateShield()
        {
            if (m_Shield == null && m_Canvas.sortingOrder > 0 && HasShield)
            {
                m_Shield = Instantiate(Resources.Load<GameObject>("Shield"));

                m_Shield.name = "Shield";

                Transform t = m_Shield.transform;

                SettingTransform();

                SettingRectTransform();

                if (m_Shield)
                {
                    Animation.shield = m_Shield.GetComponent<Image>();
                }

                void SettingRectTransform()
                {
                    RectTransform rt = t.GetComponent<RectTransform>();
                    rt.anchorMin = Vector2.zero;
                    rt.anchorMax = Vector2.one;
                    rt.pivot = new Vector2(0.5f, 0.5f);
                    rt.offsetMax = new Vector2(2, 2);
                    rt.offsetMin = new Vector2(-2, -2);
                }

                void SettingTransform()
                {
                    t.SetParent(m_Canvas.transform);
                    t.SetSiblingIndex(0);
                    t.localScale = Vector3.one;
                    t.localPosition = new Vector3(t.localPosition.x, t.localPosition.y, 0);
                }
            }
        }

        public void SetupCanvas(int sortingOrder)
        {
            if (SafeArea)
            {
                Animation.UpdateSafeArea();
            }

            if (m_Canvas == null)
            {
                m_Canvas = transform.GetComponentInChildren<Canvas>(true);
            }

            if (UseCameraUI)
            {
                m_Canvas.sortingOrder = sortingOrder;
                m_Canvas.worldCamera = Manager.Object.UICamera;
            }

            LibraryGame.Game.GetScale(GetCanvasScaler());

            RectTransform rootRect = m_Canvas.GetComponent<RectTransform>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(rootRect);
        }

        public virtual void HideUI()
        {
            var data = Animation.GetCanvasGroup();
            data.alpha = GameManager.Instance.hideUI == true ? 0 : 1;
        }

        public void EventShow()
        {
            if (Camera == null)
            {
                FirebaseEvent.LogEvent(string.Format("show_{0}", SceneName()), "level", GameManager.Instance.GetMasterData().GetData(MasterDataType.Stage).ToString());
            }
        }

        public void EventHide()
        {
            if (Camera == null)
            {
                FirebaseEvent.LogEvent(string.Format("hide_{0}", SceneName()), "level", GameManager.Instance.GetMasterData().GetData(MasterDataType.Stage).ToString());
            }
        }

        public void HidePopup(bool hideShield)
        {
            (Animation as SceneDefaultAnimation).PlayHide(hideShield);
        }

        public void ShowPopup(bool hideShield)
        {
            (Animation as SceneDefaultAnimation).PlayShow(hideShield);
        }

        private RectTransform m_RectTransform;

        private CanvasScaler m_CanvasScaler;

        private CanvasGroup m_CanvasGroup;

        public RectTransform GetRect()
        {
            if (m_RectTransform == null)
            {
                m_RectTransform = GetComponent<RectTransform>();
            }
            return m_RectTransform;
        }

        public CanvasGroup GetCanvasGroup()
        {
            if (m_CanvasGroup == null)
            {
                m_CanvasGroup = Animation.GetComponent<CanvasGroup>();
            }
            return m_CanvasGroup;
        }

        public CanvasScaler GetCanvasScaler()
        {
            if (m_CanvasScaler == null)
            {
                m_CanvasScaler = m_Canvas.GetComponent<CanvasScaler>();
            }
            return m_CanvasScaler;
        }
    }
}