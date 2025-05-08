// This code is part of the SS-Scene library, released by Anh Pham (anhpt.csit@gmail.com).

using UnityEngine;
using System.Collections;

namespace SS.View
{
    public class UnscaledLegacyAnimation : MonoBehaviour
    {
        public delegate void OnAnimationEndDelegate(string clipName);
        OnAnimationEndDelegate m_OnAnimationEnd;

        float m_TimeAtLastFrame = 0F;
        float m_TimeAtCurrentFrame = 0F;
        float m_DeltaTime = 0F;
        float m_AccumTime = 0F;

        AnimationState m_CurrState;
        bool m_IsPlayingLegacyAnim = false;
        bool m_IsEndAnim = false;
        string m_CurrClipName;

        Animation m_Animation;
        Animation Animation
        {
            get
            {
                if (m_Animation == null)
                {
                    m_Animation = GetComponent<Animation>();
                }
                return m_Animation;
            }
        }

        public string currentClipName
        {
            get
            {
                return m_CurrClipName;
            }
        }

        public void Play(string clip, OnAnimationEndDelegate onAnimationEnd = null)
        {
            m_AccumTime = 0F;
            m_CurrClipName = clip;
            m_CurrState = Animation[clip];
            m_CurrState.weight = 1;
            m_CurrState.blendMode = AnimationBlendMode.Blend;
            m_CurrState.wrapMode = WrapMode.Once;
            m_CurrState.normalizedTime = 0;
            m_CurrState.enabled = true;
            m_IsPlayingLegacyAnim = true;
            m_IsEndAnim = false;
            m_OnAnimationEnd = onAnimationEnd;
            m_TimeAtLastFrame = Time.realtimeSinceStartup;
        }

        public void PauseAtBeginning(string animationName)
        {
            Animation.Play(animationName);
            Animation[animationName].time = 0;
            Animation.Sample();
            Animation.Stop();
        }

        public void SetDuration(string animationName, float duration)
        {
            if (duration > 0)
            {
                Animation[animationName].speed = Animation[animationName].length / duration;
            }
        }

        private void Update()
        {
            if (Application.isPlaying)
            {
                AnimationUpdate();
            }
        }

        private void AnimationUpdate()
        {
            m_TimeAtCurrentFrame = Time.realtimeSinceStartup;
            m_DeltaTime = m_TimeAtCurrentFrame - m_TimeAtLastFrame;
            m_TimeAtLastFrame = m_TimeAtCurrentFrame; 

            if (m_IsPlayingLegacyAnim)
            {
                if (m_IsEndAnim == true)
                {
                    m_CurrState.enabled = false;
                    m_IsPlayingLegacyAnim = false;

                    if (m_OnAnimationEnd != null)
                    {
                        m_OnAnimationEnd(m_CurrClipName);
                    }

                    return;
                }

                m_AccumTime += m_DeltaTime * Animation[m_CurrClipName].speed;
                if (m_AccumTime >= m_CurrState.length)
                {
                    m_AccumTime = m_CurrState.length;
                    m_IsEndAnim = true;
                }
                m_CurrState.normalizedTime = m_AccumTime / m_CurrState.length;
            }
        }
    }
}