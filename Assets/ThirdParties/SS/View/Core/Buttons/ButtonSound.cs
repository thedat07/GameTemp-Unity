public class ButtonSound : ButtonGame
{
    protected override void StartButton()
    {
        UpdateView();
    }

    protected override void OnClickEvent()
    {
        GameManager.Instance.GetSettingPresenter().SetSound();
        UpdateView();
    }

    private void UpdateView()
    {

    }
}

#if UNITY_EDITOR
namespace Lean.Gui.Editor
{
    using UnityEditor;
    using TARGET = ButtonSound;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(TARGET))]
    public class ButtonSound_Editor : ButtonGame_Editor
    {

    }
}
#endif