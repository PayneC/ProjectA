using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using XLua;

/// <summary>
/// xlua自定义导出
/// </summary>
public static class XLuaCustomExport
{    
    /// <summary>
    /// dotween的扩展方法在lua中调用
    /// </summary>
    [LuaCallCSharp]
    public static List<Type> dotween_lua_call_cs_list = new List<Type>()
    {

        typeof(DG.Tweening.AutoPlay),
        typeof(DG.Tweening.AxisConstraint),
        typeof(DG.Tweening.Ease),
        typeof(DG.Tweening.LogBehaviour),
        typeof(DG.Tweening.LoopType),
        typeof(DG.Tweening.PathMode),
        typeof(DG.Tweening.PathType),
        typeof(DG.Tweening.RotateMode),
        typeof(DG.Tweening.ScrambleMode),
        typeof(DG.Tweening.TweenType),
        typeof(DG.Tweening.UpdateType),
        
        typeof(DG.Tweening.DOTween),
        typeof(DG.Tweening.DOVirtual),
        typeof(DG.Tweening.EaseFactory),
        typeof(DG.Tweening.Tweener),
        typeof(DG.Tweening.Tween),
        typeof(DG.Tweening.Sequence),
        typeof(DG.Tweening.TweenParams),
        typeof(DG.Tweening.Core.ABSSequentiable),
        
        typeof(DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions>),
        
        typeof(DG.Tweening.TweenCallback),
        typeof(DG.Tweening.TweenExtensions),
        typeof(DG.Tweening.TweenSettingsExtensions),
        typeof(DG.Tweening.ShortcutExtensions),
        typeof(DG.Tweening.ShortcutExtensions43),
        typeof(DG.Tweening.ShortcutExtensions46),
        typeof(DG.Tweening.ShortcutExtensions50),

        //dotween pro 的功能
        typeof(DG.Tweening.DOTweenPath),
        typeof(DG.Tweening.DOTweenVisualManager),

        typeof(System.Object),
        typeof(UnityEngine.Object),
        typeof(Vector3),
        typeof(Type),
        typeof(Debug),
        typeof(Vector2),
        typeof(GameObject),
        typeof(Transform),
        typeof(RectTransform),
        typeof(RectTransform.Axis),
        typeof(RectTransform.Edge),
        typeof(EAssetType),
        typeof(UnityEngine.UI.Image),
        typeof(ImageEx),
        typeof(RawImageEx),
        typeof(Button),
        typeof(ButtonEx),
        typeof(Button.ButtonClickedEvent),
        typeof(UILoading),
        typeof(Singleton<AssetUtil>),
        typeof(AssetUtil),
        typeof(GameObjectUtil),
        typeof(AssetEntity),
        typeof(UnityEngine.Events.UnityEvent),
        //typeof(InputUtil),
    };

    [CSharpCallLua]
    public static List<Type> cs_call_lua_list = new List<Type>()
    {
        typeof(UnityAction),            
    };
}