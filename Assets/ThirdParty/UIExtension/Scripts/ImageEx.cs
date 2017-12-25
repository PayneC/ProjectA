using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct SpriteInfo
{
    public UIAtlas atlas;
    public Image target;
    public string atlasName;
    public string spriteName;
    public Rect _offset;
}

[AddComponentMenu("UI/ImageEx")]
public class ImageEx : Image
{
    [SerializeField]
    public SpriteInfo spriteInfo;

    [SerializeField]
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

    public void SetSpriteOffset(Rect _rt)
    {
        
    }

    public void SetSprite(string atlasName, string spriteName)
    {
        if (string.Equals(atlasName, spriteInfo.atlasName) && string.Equals(spriteName, spriteInfo.spriteName))
            return;

        spriteInfo.atlasName = atlasName;
        spriteInfo.spriteName = spriteName;
        _RefreshSprite();
    }

    private void OnAtlasLoaded(AssetEntity _asset)
    {
        spriteInfo.atlas = _asset.GetInstantiate() as UIAtlas;
        if (spriteInfo.atlas != null)
        {
            sprite = spriteInfo.atlas.GetSpriteByName(spriteInfo.spriteName);
        }
        else
        {

        }
        isRender = null != sprite;
    }

    private void _RefreshSprite()
    {
        if (string.IsNullOrEmpty(spriteInfo.atlasName) || string.IsNullOrEmpty(spriteInfo.spriteName))
        {
            sprite = null;
        }
        else if (spriteInfo.atlas != null && string.Equals(spriteInfo.atlas.name, spriteInfo.atlasName))
        {
            sprite = spriteInfo.atlas.GetSpriteByName(spriteInfo.spriteName);
        }
        else
        {
            AssetUtil.I.AsyncLoad(EAssetType.ATLAS, spriteInfo.atlasName, OnAtlasLoaded);
        }

        isRender = null != sprite;
    }

    protected override void Start()
    {
        base.Start();

#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
        {
            if (sprite == null)
            {
                _RefreshSprite();
            }
        }
#else
            if (sprite == null)
            {
                _RefreshSprite();
            }
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
    public static void Copy(ImageEx des, Image source)
    {
        des.alphaHitTestMinimumThreshold = source.alphaHitTestMinimumThreshold;
        des.fillAmount = source.fillAmount;
        des.fillCenter = source.fillCenter;
        des.fillClockwise = source.fillClockwise;
        des.fillMethod = source.fillMethod;
        des.fillOrigin = source.fillOrigin;
        des.material = source.material;
        des.overrideSprite = source.overrideSprite;
        des.preserveAspect = source.preserveAspect;
        des.sprite = source.sprite;
        des.type = source.type;

        des.maskable = source.maskable;
        des.onCullStateChanged = source.onCullStateChanged;

        des.color = source.color;
        des.material = source.material;
        des.raycastTarget = source.raycastTarget;

        des.enabled = source.enabled;
    }
    public void SetInfo(string atlasName, string spriteName)
    {
        spriteInfo.atlasName = atlasName;
        spriteInfo.spriteName = spriteName;
    }
#endif
}
