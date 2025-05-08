public class ButtonMusic : ButtonGame
{
    protected override void StartButton()
    {
        UpdateView();
    }

    protected override void OnClickEvent()
    {
        GameManager.Instance.GetSettingPresenter().SetMusic();
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
    using TARGET = ButtonMusic;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(TARGET))]
    public class ButtonMusic_Editor : ButtonGame_Editor
    {

    }
}
#endif