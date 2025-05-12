using UnityEngine;
using DG.Tweening;

namespace Directory
{
    public class SceneDefaultAnimation : SceneAnimation
    {
        #region Enum

        protected enum State
        {
            IDLE,
            SHOW,
            HIDE
        }

        protected enum AnimationType
        {
            None,
            Fade,
            Scale,
            SlideFromBottom,
            SlideFromTop,
            SlideFromLeft,
            SlideFromRight,
        }

        #endregion

        #region SerializeField

        [SerializeField] AnimationType m_AnimationType = AnimationType.SlideFromRight;
        [SerializeField] Ease m_ShowEaseType = Ease.Linear;
        [SerializeField] Ease m_HideEaseType = Ease.Linear;

        #endregion

        #region Private Variable

        protected Vector2 m_Start;
        protected Vector2 m_End;
        protected RectTransform m_RectTransform;
        protected RectTransform m_CanvasRectTransform;
        protected State m_State = State.IDLE;

        #endregion

        void Awake()
        {
            if (Application.isPlaying)
            {
                if (Manager.SceneAnimationDuration > GetDataNullDefault())
                {
                    m_AnimationDuration = Manager.SceneAnimationDuration;
                }
                else
                {
                    m_AnimationDuration = 0.15f;
                }
            }
        }

        RectTransform RectTransform
        {
            get
            {
                if (m_RectTransform == null)
                {
                    m_RectTransform = GetComponent<RectTransform>();
                }

                return m_RectTransform;
            }
        }

        CanvasGroup CanvasGroup
        {
            get
            {
                return GetCanvasGroup();
            }
        }

        RectTransform CanvasRectTransform
        {
            get
            {
                if (m_CanvasRectTransform == null)
                {
                    Transform p = transform.parent;
                    while (p != null)
                    {
                        if (p.GetComponent<Canvas>() != null)
                        {
                            m_CanvasRectTransform = p.GetComponent<RectTransform>();
                            break;
                        }
                        p = p.parent;
                    }
                }

                return m_CanvasRectTransform;
            }
        }

        public override void HideBeforeShowing()
        {
            switch (m_AnimationType)
            {
                case AnimationType.Fade:
                    CanvasGroup.alpha = GetDataNullDefault();
                    break;
                case AnimationType.Scale:
                    RectTransform.localScale = Vector3.zero;
                    break;
                case AnimationType.SlideFromBottom:
                    RectTransform.anchoredPosition = new Vector2(GetDataNullDefault(), -ScreenHeight());
                    break;
                case AnimationType.SlideFromLeft:
                    RectTransform.anchoredPosition = new Vector2(-ScreenWidth(), RectTransform.anchoredPosition.y);
                    break;
                case AnimationType.SlideFromRight:
                    RectTransform.anchoredPosition = new Vector2(ScreenWidth(), RectTransform.anchoredPosition.y);
                    break;
                case AnimationType.SlideFromTop:
                    RectTransform.anchoredPosition = new Vector2(GetDataNullDefault(), ScreenHeight());
                    break;
            }

            Show();
        }

        public override void Show()
        {
            switch (m_AnimationType)
            {
                case AnimationType.SlideFromBottom:
                    m_Start = new Vector2(GetDataNullDefault(), -ScreenHeight());
                    m_End = Vector2.zero;
                    break;
                case AnimationType.SlideFromTop:
                    m_Start = new Vector2(GetDataNullDefault(), ScreenHeight());
                    m_End = Vector2.zero;
                    break;
                case AnimationType.SlideFromRight:
                    m_Start = new Vector2(ScreenWidth(), RectTransform.anchoredPosition.y);
                    m_End = new Vector2(GetDataNullDefault(), RectTransform.anchoredPosition.y);
                    break;
                case AnimationType.SlideFromLeft:
                    m_Start = new Vector2(-ScreenWidth(), RectTransform.anchoredPosition.y);
                    m_End = new Vector2(GetDataNullDefault(), RectTransform.anchoredPosition.y);
                    break;
                case AnimationType.Scale:
                    m_Start = Vector2.zero;
                    m_End = Vector2.one;
                    break;
                case AnimationType.Fade:
                    m_Start = Vector2.zero;
                    m_End = Vector2.one;
                    break;
            }

            if (m_AnimationType != AnimationType.None)
            {
                m_State = State.SHOW;
                this.Play();
            }
            else
            {
                OnShown();
            }
        }

        public override void Hide()
        {
            switch (m_AnimationType)
            {
                case AnimationType.SlideFromBottom:
                    m_Start = Vector2.zero;
                    m_End = new Vector2(GetDataNullDefault(), -ScreenHeight());
                    break;
                case AnimationType.SlideFromTop:
                    m_Start = Vector2.zero;
                    m_End = new Vector2(GetDataNullDefault(), ScreenHeight());
                    break;
                case AnimationType.SlideFromRight:
                    m_Start = new Vector2(GetDataNullDefault(), RectTransform.anchoredPosition.y);
                    m_End = new Vector2(ScreenWidth(), RectTransform.anchoredPosition.y);
                    break;
                case AnimationType.SlideFromLeft:
                    m_Start = new Vector2(GetDataNullDefault(), RectTransform.anchoredPosition.y);
                    m_End = new Vector2(-ScreenWidth(), RectTransform.anchoredPosition.y);
                    break;
                case AnimationType.Scale:
                    m_Start = Vector2.one;
                    m_End = Vector2.zero;
                    break;
                case AnimationType.Fade:
                    m_Start = Vector2.one;
                    m_End = Vector2.zero;
                    break;
            }

            if (m_AnimationType != AnimationType.None)
            {
                m_State = State.HIDE;
                this.Play();
            }
            else
            {
                OnHidden();
            }
        }

        protected override Tween Effect()
        {
            Ease ease = (m_State == State.SHOW) ? m_ShowEaseType : m_HideEaseType;

            Sequence mySequence = Helper.DOTweenSequence(gameObject);

            switch (m_AnimationType)
            {
                case AnimationType.Scale:
                    {
                        mySequence.Append(DOAnimationTypeScale());
                        mySequence.Join(DOFadeShow());
                    }
                    break;
                case AnimationType.Fade:
                    {
                        mySequence.Append(DOAnimationTypeFade());
                        break;
                    }
                default:
                    {
                        mySequence.Append(DOAnimationTypeMove());
                        mySequence.Join(DOFadeShow());
                    }
                    break;
            }

            if (shield != null)
            {
                mySequence.Join(shield.DOFade(GetFade(true), GetAnimationDuration()).From(GetFade()).SetEase(Ease.Linear));
            }

            Tween DOFadeShow()
            {
                return CanvasGroup
                .DOFade((m_State == State.SHOW) ? GetDataDefault() : GetDataNullDefault(), GetAnimationDuration())
                .From((m_State == State.SHOW) ? GetDataNullDefault() : GetDataDefault())
                .SetEase(Ease.Linear);
            }

            Tween DOAnimationTypeScale()
            {
                if (m_State == State.SHOW)
                {
                    RectTransform.localScale = Vector3.zero;
                    return Helper.SetDelay(GetAnimationDurationFlash(), () => { Effect(); }, gameObject);
                }
                else
                {
                    return Effect();
                }

                Tween Effect()
                {
                    return RectTransform.DOScale(new Vector3(m_End.x, m_End.y, GetDataDefault()), GetAnimationDuration())
                            .From(new Vector3(m_Start.x, m_Start.y, GetDataDefault()))
                            .SetEase(ease);
                }
            }

            Tween DOAnimationTypeMove()
            {
                return RectTransform.DOAnchorPos(m_End, GetAnimationDuration())
                .From(m_Start)
                .SetEase(ease);
            }

            Tween DOAnimationTypeFade()
            {
                if (m_State == State.SHOW)
                {
                    return Helper.SetDelay(GetAnimationDurationFlash(), () => { Effect(); }, gameObject);
                }
                else
                {
                    return Effect();
                }

                Tween Effect()
                {
                    return CanvasGroup.DOFade(m_End.y, GetAnimationDuration())
                    .From(m_Start.x)
                    .SetEase(ease);
                }
            }

            return mySequence;
        }

        protected override void OnEndAnimation()
        {
            switch (m_State)
            {
                case State.SHOW:
                    OnShown();
                    break;
                case State.HIDE:
                    OnHidden();
                    break;
            }

            m_State = State.IDLE;
        }

        public override void SetImmediate()
        {
            m_AnimationType = AnimationType.None;
        }

        float ScreenHeight()
        {
            if (CanvasRectTransform != null)
            {
                return CanvasRectTransform.sizeDelta.y;
            }
            return Screen.height;
        }

        float ScreenWidth()
        {
            if (CanvasRectTransform != null)
            {
                return CanvasRectTransform.sizeDelta.x;
            }
            return Screen.width;
        }

        private float GetAnimationDuration() => m_AnimationDuration;

        private float GetAnimationDurationFlash() => m_AnimationDuration / 2;

        private float GetFade(bool back = false) => back == false ? (m_State == State.SHOW ? GetDataNullDefault() : GetFadeDefault()) : (m_State == State.SHOW ? GetFadeDefault() : GetDataNullDefault());

        public void PlayShow(bool hide = true)
        {
            Sequence mySequence = Helper.DOTweenSequence(gameObject);

            mySequence.Append(CanvasGroup.DOFade(GetDataDefault(), GetAnimationDurationFlash()).SetEase(Ease.Linear));

            if (hide && shield != null)
            {
                mySequence.Join(shield.DOFade(GetFadeDefault(), GetAnimationDurationFlash()).SetEase(Ease.Linear));
            }

            mySequence.OnComplete(() => { CanvasGroup.blocksRaycasts = true; });
        }

        public void PlayHide(bool hide = true)
        {
            CanvasGroup.alpha = GetDataNullDefault();
            CanvasGroup.blocksRaycasts = false;

            if (hide && shield != null)
            {
                Color colorShield = shield.color;
                colorShield.a = GetDataNullDefault();
                shield.color = colorShield;
            }

            m_State = State.HIDE;
        }
    }
}