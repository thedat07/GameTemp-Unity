using UnityEngine;
using DG.Tweening;
using LibraryGame;

public class ButtonGame : ButtonBase
{
    [Header("Audio")]
    public TypeAudio typeAudio = TypeAudio.ButtonClick;

    [Header("Audio")]
    public bool activeEffect = true;

    protected override void PlayAudio()
    {
        GameManager.Instance.GetSettingPresenter().PlaySound(typeAudio);
    }

    protected override void PlayEffect()
    {
        if (activeEffect)
        {
            transform.DoResetDefault();
            transform.DOPunchScale(m_Scale * 0.05f, 0.25f).SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }
    }
}

#if UNITY_EDITOR
namespace Lean.Gui.Editor
{
    using UnityEditor;
    using TARGET = ButtonGame;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(TARGET))]
    public class ButtonGame_Editor : ButtonBase_Editor
    {
        protected override void DrawSelectableSettings()
        {
            base.DrawSelectableSettings();

            Draw("typeAudio", "Audio Click");

            Draw("activeEffect", "Effect Default");
        }
    }
}
#endif