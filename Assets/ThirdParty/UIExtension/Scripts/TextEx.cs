using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/TextEx")]
public class TextEx : Text
{
    private bool _isLoading = false;

    [SerializeField]
    private string _fontName = null;

    public string GetFontName()
    {
        return _fontName;
    }

    public void SetFontName(string tex)
    {
        if (!string.IsNullOrEmpty(_fontName) && _fontName.Equals(tex))
            return;

        _fontName = tex;

        if (string.IsNullOrEmpty(_fontName))
        {
            SetFont(null, _fontName);
        }
        else if (!_isLoading)
        {
            LoadFont();
        }
    }

    public void SetFont(Font f, string name)
    {
        _fontName = name;
        font = f;
    }

    private void callback(int instanID, int type, string name, Object oj)
    {
        _isLoading = false;
        if (_fontName != name)
        {
            LoadFont();
        }
        else
        {
            Font f = oj as Font;
            SetFont(f, name);
        }
    }

    protected override void Start()
    {
        base.Start();
        LoadFont();
    }

    private void LoadFont()
    {
        if (string.IsNullOrEmpty(_fontName))
            return;

        _isLoading = true;
    }

    public void Copy(Text source)
    {
        alignByGeometry = source.alignByGeometry;
        alignment = source.alignment;
        font = source.font;
        fontSize = source.fontSize;
        fontStyle = source.fontStyle;
        horizontalOverflow = source.horizontalOverflow;
        lineSpacing = source.lineSpacing;
        resizeTextForBestFit = source.resizeTextForBestFit;
        resizeTextMaxSize = source.resizeTextMaxSize;
        resizeTextMinSize = source.resizeTextMinSize;
        supportRichText = source.supportRichText;
        text = source.text;
        verticalOverflow = source.verticalOverflow;

        maskable = source.maskable;
        onCullStateChanged = source.onCullStateChanged;

        color = source.color;
        material = source.material;
        raycastTarget = source.raycastTarget;

        enabled = source.enabled;
    }

#if UNITY_EDITOR
    [LuaInterface.NoToLua]
    public void SetInfo(string name)
    {
        _fontName = name;
    }
#endif

    public override float preferredHeight
    {
        get
        {
            if (font != null)
                return base.preferredHeight;
            return 0f;
        }
    }

    public override float preferredWidth
    {
        get
        {
            if (font != null)
                return base.preferredWidth;
            return 0f;
        }
    }
}