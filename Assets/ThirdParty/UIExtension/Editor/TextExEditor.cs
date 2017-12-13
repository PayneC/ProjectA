using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(TextEx))]
public class TextExEditor : TextEditor
{
    TextEx _text = null;
    TextEx text
    {
        get
        {
            if (_text == null)
                _text = target as TextEx;
            return _text;
        }
    }

    public override void OnInspectorGUI()
    {        
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("Font Name", text.GetFontName());           
    }
}
