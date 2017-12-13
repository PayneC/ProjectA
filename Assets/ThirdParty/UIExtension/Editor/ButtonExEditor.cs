using UnityEditor;
using UnityEngine;
using UnityEditor.UI;

[CustomEditor(typeof(ButtonEx))]

public class ButtonExEditor : ButtonEditor
{
    ButtonEx _button = null;
    ButtonEx button
    {
        get
        {
            if (_button == null)
                _button = target as ButtonEx;
            return _button;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ButtonInspector();
    }

    private void ButtonInspector()
    {
        button.useClickAnimation = EditorGUILayout.Toggle("UseClickAnimation", button.useClickAnimation);
        EditorGUILayout.BeginVertical(GUI.skin.box);
        SpriteInfoInspector(button.highlightedSpriteInfo, "Highlighted");
        SpriteInfoInspector(button.pressedSpriteInfo, "Pressed");
        SpriteInfoInspector(button.disabledSpriteInfo, "Disabled");
        EditorGUILayout.EndVertical();
    }

    private void SpriteInfoInspector(SpriteInfo info, string name)
    {
        EditorGUILayout.LabelField(name);
        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.LabelField("Atlas Name", info.atlasName);
        EditorGUILayout.LabelField("Sprite Name", info.spriteName);
        if(Application.isPlaying)
        {
            EditorGUILayout.ObjectField("Atlas", info.atlas, typeof(UIAtlas), false);
        }
        EditorGUILayout.EndVertical();
    }
}
