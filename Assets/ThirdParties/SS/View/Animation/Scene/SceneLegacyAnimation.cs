// This code is part of the SS-Scene library, released by Anh Pham (anhpt.csit@gmail.com).

using UnityEngine;
using System.Collections;

namespace Directory
{
    public class SceneLegacyAnimation : SceneAnimation
    {
        [SerializeField]
        protected string m_ShowAnimName = "Show";

        [SerializeField]
        protected string m_HideAnimName = "Hide";

        UnscaledLegacyAnimation m_Animation;

        void Awake()
        {
            if (Application.isPlaying)
            {
                m_Animation = GetComponent<UnscaledLegacyAnimation>();
                if (m_Animation == null)
                {
                    m_Animation = gameObject.AddComponent<UnscaledLegacyAnimation>();
                }

                m_Animation.SetDuration(m_ShowAnimName, Manager.SceneAnimationDuration);
                m_Animation.SetDuration(m_HideAnimName, Manager.SceneAnimationDuration);
            }
        }

        public override void HideBeforeShowing()
        {
            m_Animation.PauseAtBeginning(m_ShowAnimName);
        }

        public override void Show()
        {
            m_Animation.Play(m_ShowAnimName, OnAnimationEnd);
        }

        public override void Hide()
        {
            m_Animation.Play(m_HideAnimName, OnAnimationEnd);
        }

        private void OnAnimationEnd(string clipName)
        {
            if (clipName == m_ShowAnimName)
            {
                OnShown();
            }
            else if (clipName == m_HideAnimName)
            {
                OnHidden();
            }
        }
    }
}