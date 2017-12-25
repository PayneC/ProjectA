using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/RawImageEx")]
public class RawImageEx : RawImage
{
    [SerializeField]
    private string _textureName;

    public string textureName
    {
        private set { _textureName = value; }
        get { return _textureName; }
    }

    private bool _isLoading = false;

    private bool _isRender = true;
    public bool isRender
    {
        get { return _isRender; }
        set
        {
            if (_isRender != value)
            {
                _isRender = value;
                SetAllDirty();
            }
        }
    }

    public void SetTextureName(string tex)
    {
        if (string.Equals(textureName, tex))
            return;

        textureName = tex;
        _RefreshTexture();
    }

    private void _RefreshTexture()
    {
        if (string.IsNullOrEmpty(textureName))
        {
            texture = null;
        }
        else if (!_isLoading)
        {
            AssetUtil.I.AsyncLoad(EAssetType.TEXTURE, textureName, OnTextureLoaded);
            _isLoading = true;
        }
    }

    public void SetTexture(Texture tex)
    {
        textureName = null;
        texture = tex;
        isRender = texture != null;
    }

    private void OnTextureLoaded(AssetEntity _asset)
    {
        _isLoading = false;
        if (string.Equals(textureName, _asset.assetName))
        {
            texture = _asset.GetInstantiate() as Texture;
        }
        else
        {
            _RefreshTexture();
        }
    }

    protected override void Start()
    {
        base.Start();

        if (string.IsNullOrEmpty(textureName) && texture != null)
        {
            return;
        }

#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
        {
            _RefreshTexture();
        }
#else
            _RefreshTexture();
#endif
    }

    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        if (isRender)
            base.OnPopulateMesh(toFill);
        else
            toFill.Clear();
    }
#if UNITY_EDITOR
    public static void Copy(RawImageEx det, RawImage source)
    {
        det.texture = source.texture;
        det.uvRect = source.uvRect;
        det.maskable = source.maskable;
        det.onCullStateChanged = source.onCullStateChanged;
        det.color = source.color;
        det.material = source.material;
        det.raycastTarget = source.raycastTarget;
        det.enabled = source.enabled;
    }
    public void SetInfo(string name)
    {
        textureName = name;
    }
#endif
}
