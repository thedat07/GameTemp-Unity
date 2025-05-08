public class ButtonVibrate : ButtonGame
{
    protected override void StartButton()
    {
        UpdateView();
    }

    protected override void OnClickEvent()
    {
        GameManager.Instance.GetSettingPresenter().SetVibration();
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
    using TARGET = ButtonVibrate;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(TARGET))]
    public class ButtonVibrate_Editor : ButtonGame_Editor
    {

    }
}
#endif