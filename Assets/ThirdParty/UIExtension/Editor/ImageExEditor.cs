using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(ImageEx))]
public class ImageExEditor : ImageEditor
{
    ImageEx _sprite = null;
    ImageEx sprite
    {
        get
        {
            if (_sprite == null)
                _sprite = target as ImageEx;
            return _sprite;
        }
    }

    string[] allSprites = null;
    int index = 0;

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

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SpriteInspector();
    }

    private void SpriteInspector()
    {
        sprite.isRender = EditorGUILayout.Toggle("IsRender", sprite.isRender);
        sprite.SetSpriteOffset(EditorGUILayout.RectField("Sprite Offset", sprite.spriteInfo._offset));

        if(Application.isPlaying)
        {
            EditorGUILayout.ObjectField("Atlas", sprite.spriteInfo.atlas, typeof(UIAtlas), false);
        }
    }

    public override void OnPreviewGUI(Rect rect, GUIStyle background)
    {
        base.OnPreviewGUI(rect, background);

        if (sprite.sprite != null && sprite.sprite.texture != null)
        {            
            EditorGUILayout.LabelField("atlas name", sprite.spriteInfo.atlasName, previewLabelStyle);
            EditorGUILayout.LabelField("sprite name", sprite.spriteInfo.spriteName, previewLabelStyle);
            EditorGUILayout.LabelField("texture2D.name", sprite.sprite.texture.name, previewLabelStyle);
            EditorGUILayout.LabelField("texture2D.instanceId", sprite.sprite.texture.GetInstanceID().ToString(), previewLabelStyle);
            EditorGUILayout.LabelField("sprite.name", sprite.sprite.name, previewLabelStyle);

            EditorGUILayout.RectField("sprite.rect", sprite.sprite.textureRect);
        }        
    }
}
