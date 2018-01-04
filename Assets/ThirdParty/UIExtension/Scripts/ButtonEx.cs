using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

[AddComponentMenu("UI/ButtonEx")]
public class ButtonEx : Button
{
    //
    // 摘要:
    //     ///
    //     Disabled sprite.
    //     ///
    public SpriteInfo disabledSpriteInfo;
    //
    // 摘要:
    //     ///
    //     Highlighted sprite.
    //     ///
    public SpriteInfo highlightedSpriteInfo;
    //
    // 摘要:
    //     ///
    //     Pressed sprite.
    //     ///
    public SpriteInfo pressedSpriteInfo;

    private SpriteState newSpriteState = new SpriteState();

    public bool useClickAnimation = false;

    private void _RefreshDisabledSprite()
    {
        if (string.IsNullOrEmpty(disabledSpriteInfo.atlasName) || string.IsNullOrEmpty(disabledSpriteInfo.spriteName))
        {
            newSpriteState.disabledSprite = null;
        }
        else if (disabledSpriteInfo.atlas != null && string.Equals(disabledSpriteInfo.atlas.name, disabledSpriteInfo.atlasName))
        {
            newSpriteState.disabledSprite = disabledSpriteInfo.atlas.GetSpriteByName(disabledSpriteInfo.spriteName);
        }
        else
        {
            AssetUtil.I.AsyncLoad(EAssetType.ATLAS, disabledSpriteInfo.atlasName, OnDisableAtlasLoaded);
        }
    }

    private void OnDisableAtlasLoaded(AssetEntity _asset)
    {
        disabledSpriteInfo.atlas = _asset.GetInstantiate() as UIAtlas;
        if (disabledSpriteInfo.atlas != null)
        {
            newSpriteState.disabledSprite = disabledSpriteInfo.atlas.GetSpriteByName(disabledSpriteInfo.spriteName);
        }
        else
        {

        }
    }

    private void _RefreshHighlightedSprite()
    {
        if (string.IsNullOrEmpty(highlightedSpriteInfo.atlasName) || string.IsNullOrEmpty(highlightedSpriteInfo.spriteName))
        {
            newSpriteState.highlightedSprite = null;
        }
        else if (highlightedSpriteInfo.atlas != null && string.Equals(highlightedSpriteInfo.atlas.name, highlightedSpriteInfo.atlasName))
        {
            newSpriteState.highlightedSprite = highlightedSpriteInfo.atlas.GetSpriteByName(highlightedSpriteInfo.spriteName);
        }
        else
        {
            AssetUtil.I.AsyncLoad(EAssetType.ATLAS, highlightedSpriteInfo.atlasName, OnHighlightedAtlasLoaded);
        }
    }

    private void OnHighlightedAtlasLoaded(AssetEntity _asset)
    {
        highlightedSpriteInfo.atlas = _asset.GetInstantiate() as UIAtlas;
        if (highlightedSpriteInfo.atlas != null)
        {
            newSpriteState.highlightedSprite = highlightedSpriteInfo.atlas.GetSpriteByName(highlightedSpriteInfo.spriteName);
        }
        else
        {

        }
    }


    private void _RefreshPressedSprite()
    {
        if (string.IsNullOrEmpty(pressedSpriteInfo.atlasName) || string.IsNullOrEmpty(pressedSpriteInfo.spriteName))
        {
            newSpriteState.pressedSprite = null;
        }
        else if (pressedSpriteInfo.atlas != null && string.Equals(pressedSpriteInfo.atlas.name, pressedSpriteInfo.atlasName))
        {
            newSpriteState.pressedSprite = pressedSpriteInfo.atlas.GetSpriteByName(pressedSpriteInfo.spriteName);
        }
        else
        {
            AssetUtil.I.AsyncLoad(EAssetType.ATLAS, pressedSpriteInfo.atlasName, OnHighlightedAtlasLoaded);
        }
    }

    private void OnPressedAtlasLoaded(AssetEntity _asset)
    {
        pressedSpriteInfo.atlas = _asset.GetInstantiate() as UIAtlas;
        if (pressedSpriteInfo.atlas != null)
        {
            newSpriteState.pressedSprite = pressedSpriteInfo.atlas.GetSpriteByName(pressedSpriteInfo.spriteName);
        }
        else
        {

        }
    }

    protected void _OnClick()
    {
        if (useClickAnimation)
        {
            transform.DOShakeScale(0.2f, Vector3.one);
        }
    }

    protected override void Start()
    {
        base.Start();
        onClick.AddListener(_OnClick);
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
        {
            _RefreshDisabledSprite();
            _RefreshHighlightedSprite();
            _RefreshPressedSprite();
        }
#else
            _RefreshDisabledSprite();
            _RefreshHighlightedSprite();
            _RefreshPressedSprite();
#endif
    }

#if UNITY_EDITOR
    [LuaInterface.NoToLua]
    public static void Copy(ButtonEx des, Button source)
    {
        des.onClick = source.onClick;

        des.animationTriggers = source.animationTriggers;
        des.colors = source.colors;
        des.image = source.image;
        des.interactable = source.interactable;
        des.navigation = source.navigation;
        des.spriteState = source.spriteState;
        des.targetGraphic = source.targetGraphic;
        des.transition = source.transition;

        des.enabled = source.enabled;
    }

    [LuaInterface.NoToLua]
    public void SetButtonInfo(int index, string atlasName, string spriteName)
    {
        switch (index)
        {
            case 0:// SelectionState.Normal:
                break;
            case 1:// SelectionState.Highlighted:
                highlightedSpriteInfo.atlasName = atlasName;
                highlightedSpriteInfo.spriteName = spriteName;
                break;
            case 2:// SelectionState.Pressed:
                pressedSpriteInfo.atlasName = atlasName;
                pressedSpriteInfo.spriteName = spriteName;
                break;
            case 3:// SelectionState.Disabled:
                disabledSpriteInfo.atlasName = atlasName;
                disabledSpriteInfo.spriteName = spriteName;
                break;
            default:
                break;
        }
    }
#endif
}