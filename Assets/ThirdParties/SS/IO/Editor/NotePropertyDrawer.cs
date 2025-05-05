using UnityEngine;
using UnityEditor;

[System.Serializable]
public class NoteProperty
{
    [SerializeField] private string note;
}

[CustomPropertyDrawer(typeof(NoteProperty))]
public class NotePropertyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property,
                                            GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 2f;
    }

    public override void OnGUI(Rect position,
                               SerializedProperty property,
                               GUIContent label)
    {
        EditorGUI.BeginChangeCheck();

        var originalColor = GUI.color;
        GUI.color = Color.yellow;

        var noteProperty = property.FindPropertyRelative("note");

        var helpBoxStyle = new GUIStyle(EditorStyles.helpBox);

        var newNoteText = EditorGUI.TextField(position,
                                              noteProperty.stringValue,
                                              style: helpBoxStyle);

        GUI.color = originalColor;

        if (EditorGUI.EndChangeCheck())
        {
            noteProperty.stringValue = newNoteText;
        }
    }
}