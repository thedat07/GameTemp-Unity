using UnityEngine;
using DG.Tweening;
using UnityUtilities;

public class ButtonGame : ButtonBase
{
    [Header("Audio")]
    public TypeAudio typeAudio = TypeAudio.ButtonClick;

    [Header("Audio")]
    public bool activeEffect = true;

    [Header("Haptic")]
    public bool activeHaptic = true;

    private Sequence seq;

    private Vector3[] scales =
    {
        new Vector3(1f, 1f, 1f),        
        new Vector3(1.2086f, 1.0963f, 1.0963f), 
        new Vector3(0.95f, 1f, 1f),    
        new Vector3(1f, 1f, 1f),      
        new Vector3(0.985f, 1f, 1f),    
        new Vector3(1f, 1f, 1f)        
    };

    private float[] times =
    {
        0.08f,  
        0.17f,  
        0.17f,  
        0.20f,  
        0.31f  
    };

    /// <summary>
    /// Phát âm thanh khi nhấn nút.
    /// </summary>
    protected override void PlayAudio()
    {
        GameManager.Instance
                   .GetSettingModelView()
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
            PlayPressedAnimation(scales, times);
        }

        if (!activeHaptic) return;

        GameManager.Instance.GetSettingModelView().TapSelectionHaptic();
    }

    public void PlayPressedAnimation(Vector3[] keyScales, float[] durations)
    {
        if (keyScales == null || durations == null || keyScales.Length != durations.Length + 1)
        {
            Debug.LogError("KeyScales phải nhiều hơn Durations đúng 1 phần tử!");
            return;
        }

        if (seq != null && seq.IsActive())
        {
            seq.Kill();
        }

        transform.localScale = Vector3.Scale(keyScales[0], m_Scale);

        seq = DOTween.Sequence();

        for (int i = 1; i < keyScales.Length; i++)
        {
            seq.Append(transform.DOScale(Vector3.Scale(keyScales[i], m_Scale), durations[i - 1]));
        }

        seq.SetEase(Ease.Linear);

        seq.SetLink(gameObject, LinkBehaviour.KillOnDestroy);
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

            Draw("typeAudio", "Loại âm thanh phát khi nhấn nút.");

            Draw("activeEffect", "Bật/tắt hiệu ứng scale khi click.");

            Draw("activeHaptic", "Bật/tắt rung.");
        }
    }
}
#endif