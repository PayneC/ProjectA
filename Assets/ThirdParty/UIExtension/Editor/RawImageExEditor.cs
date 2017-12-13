using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(RawImageEx))]
public class RawImageExEditor : RawImageEditor
{
    RawImageEx _texture = null;
    RawImageEx texture
    {
        get
        {
            if (_texture == null)
                _texture = target as RawImageEx;
            return _texture;
        }
    }

    private GUIStyle m_PreviewLabelStyle;

    protected GUIStyle previewLabelStyle
    {
        get
        {
            if (m_PreviewLabelStyle == null)
            {
                m_PreviewLabelStyle = new GUIStyle("PreOverlayLabel")
                {
                    richText = true,
                    alignment = TextAnchor.UpperLeft,
                    fontStyle = FontStyle.Normal
                };
            }

            return m_PreviewLabelStyle;
        }
    }

    public override void OnPreviewGUI(Rect rect, GUIStyle background)
    {
        base.OnPreviewGUI(rect, background);

        EditorGUILayout.LabelField("Texture Name", texture.textureName, previewLabelStyle);
    }
}
