// This code is part of the SS-Scene library, released by Anh Pham (anhpt.csit@gmail.com).

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lean.Gui;

namespace Directory
{
    [ExecuteInEditMode]
    public class SceneAnimation : BaseSS
    {
        CanvasGroup m_CanvasGroup;

        [SerializeField] Controller m_Controller;

        public Image shield;

        public CanvasGroup GetCanvasGroup()
        {
            if (m_CanvasGroup == null)
            {
                gameObject.TryGetComponent<CanvasGroup>(out m_CanvasGroup);
            }
            return m_CanvasGroup;
        }

        /// <summary>
        /// After a scene is loaded, its view will be put at center of screen. So you have to put it somewhere temporary before playing the show-animation.
        /// </summary>
        public virtual void HideBeforeShowing()
        {
        }

        /// <summary>
        /// Play the show-animation. Don't forget to call OnShown right after the animation finishes.
        /// </summary>
        public virtual void Show()
        {
            OnShown();
        }

        /// <summary>
        /// Play the hide-animation. Don't forget to call OnHidden right after the animation finishes.
        /// </summary>
        public virtual void Hide()
        {
            OnHidden();
        }

        public void StartShow()
        {
            GetCanvasGroup().blocksRaycasts = false;
            Manager.StartShow(m_Controller);
        }

        public void StartHide()
        {
            GetCanvasGroup().blocksRaycasts = false;
            Hide();
        }

        public void OnShown()
        {
            GetCanvasGroup().blocksRaycasts = true;
            Manager.OnShown(m_Controller);
        }

        public void OnHidden()
        {
            Manager.OnHidden(m_Controller);
        }

        public virtual void SetImmediate()
        {
        }

        private void Start()
        {
            if (Application.isPlaying)
            {
                if (m_Controller != Manager.MainController)
                {
                    HideBeforeShowing();
                }
            }
        }
    }
}

