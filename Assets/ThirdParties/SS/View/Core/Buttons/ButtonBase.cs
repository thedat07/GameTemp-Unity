using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using SS.View;

public class ButtonBase : Selectable
{
    protected Vector3 m_Scale;

    [SerializeField] private bool m_ApplySettings = true;

    protected override void Awake()
    {
        base.Awake();
        if (Application.isPlaying)
        {
            m_Scale = transform.localScale;

            AwakeButton();
        }
    }

    protected override void Start()
    {
        base.Start();

        if (Application.isPlaying)
        {
            if (m_ApplySettings)
                ApplySettings();
        }
    }

    protected virtual void Update()
    {
        if (Application.isPlaying)
        {
            UpdateButton();
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (Application.isPlaying)
        {
            DestroyButton();
        }
    }

    protected virtual void AwakeButton() { }

    protected virtual void StartButton() { }

    protected virtual void UpdateButton() { }

    protected virtual void DestroyButton() { }

    public void ApplySettings()
    {
        if (GameManager.Instance != null)
        {
            SettingPresenter sp = GameManager.Instance.GetSettingPresenter();

            ColorBlock cb = this.colors;
            cb.normalColor = sp.normalColor;
            cb.highlightedColor = sp.highlightedColor;
            cb.pressedColor = sp.pressedColor;
            cb.selectedColor = sp.selectedColor;
            cb.disabledColor = sp.disabledColor;
            cb.colorMultiplier = sp.colorMultiplier;
            cb.fadeDuration = sp.fadeDuration;

            this.colors = cb;
            this.transition = Selectable.Transition.ColorTint;
        }
    }
}
