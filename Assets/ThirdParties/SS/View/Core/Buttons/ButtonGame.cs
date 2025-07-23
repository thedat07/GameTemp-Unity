using UnityEngine;
using DG.Tweening;
using UnityUtilities;

public class ButtonGame : ButtonBase
{
    [Header("Audio")]
    public TypeAudio typeAudio = TypeAudio.ButtonClick;

    [Header("Audio")]
    public bool activeEffect = true;

    /// <summary>
    /// Phát âm thanh khi nhấn nút.
    /// </summary>
    protected override void PlayAudio()
    {
        GameManager.Instance
                   .GetSettingPresenter()
                   .PlaySound(typeAudio);
    }

    /// <summary>
    /// Tạo hiệu ứng scale khi click nếu được bật.
    /// </summary>
    protected override void PlayEffect()
    {
        if (!activeEffect) return;

        if (this != null && transform != null && gameObject != null)
        {
            transform.DoResetDefault(); // Reset nếu có
            transform.DOPunchScale(m_Scale * 0.05f, 0.25f)
                     .SetLink(gameObject, LinkBehaviour.KillOnDestroy)
                     .OnKill(() =>
                     {
                         if (this != null && transform != null)
                             transform.localScale = m_Scale;
                     });
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

            Draw("typeAudio", "Loại âm thanh phát khi nhấn nút.k");

            Draw("activeEffect", "Bật/tắt hiệu ứng scale khi click.");
        }
    }
}
#endif