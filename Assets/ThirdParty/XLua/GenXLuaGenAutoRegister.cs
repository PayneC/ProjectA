#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using System;
using System.Collections.Generic;
using System.Reflection;


namespace XLua.CSObjectWrap
{
    public class XLua_Gen_Initer_Register__
	{
	    static XLua_Gen_Initer_Register__()
        {
		    XLua.LuaEnv.AddIniter((luaenv, translator) => {
			    
				translator.DelayWrapLoader(typeof(DG.Tweening.AutoPlay), DGTweeningAutoPlayWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.AxisConstraint), DGTweeningAxisConstraintWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.Ease), DGTweeningEaseWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.LogBehaviour), DGTweeningLogBehaviourWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.LoopType), DGTweeningLoopTypeWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.PathMode), DGTweeningPathModeWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.PathType), DGTweeningPathTypeWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.RotateMode), DGTweeningRotateModeWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.ScrambleMode), DGTweeningScrambleModeWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.TweenType), DGTweeningTweenTypeWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.UpdateType), DGTweeningUpdateTypeWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.DOTween), DGTweeningDOTweenWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.DOVirtual), DGTweeningDOVirtualWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.EaseFactory), DGTweeningEaseFactoryWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.Tweener), DGTweeningTweenerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.Tween), DGTweeningTweenWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.Sequence), DGTweeningSequenceWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.TweenParams), DGTweeningTweenParamsWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.Core.ABSSequentiable), DGTweeningCoreABSSequentiableWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.Core.TweenerCore<UnityEngine.Vector3, UnityEngine.Vector3, DG.Tweening.Plugins.Options.VectorOptions>), DGTweeningCoreTweenerCore_3_UnityEngineVector3UnityEngineVector3DGTweeningPluginsOptionsVectorOptions_Wrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.TweenExtensions), DGTweeningTweenExtensionsWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.TweenSettingsExtensions), DGTweeningTweenSettingsExtensionsWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.ShortcutExtensions), DGTweeningShortcutExtensionsWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.ShortcutExtensions43), DGTweeningShortcutExtensions43Wrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.ShortcutExtensions46), DGTweeningShortcutExtensions46Wrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.ShortcutExtensions50), DGTweeningShortcutExtensions50Wrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.DOTweenPath), DGTweeningDOTweenPathWrap.__Register);
				
				translator.DelayWrapLoader(typeof(DG.Tweening.DOTweenVisualManager), DGTweeningDOTweenVisualManagerWrap.__Register);
				
				translator.DelayWrapLoader(typeof(object), SystemObjectWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.Object), UnityEngineObjectWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.Vector3), UnityEngineVector3Wrap.__Register);
				
				translator.DelayWrapLoader(typeof(System.Type), SystemTypeWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.Debug), UnityEngineDebugWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.Vector2), UnityEngineVector2Wrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.GameObject), UnityEngineGameObjectWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.Transform), UnityEngineTransformWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.RectTransform), UnityEngineRectTransformWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.RectTransform.Axis), UnityEngineRectTransformAxisWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.RectTransform.Edge), UnityEngineRectTransformEdgeWrap.__Register);
				
				translator.DelayWrapLoader(typeof(EAssetType), EAssetTypeWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.UI.Image), UnityEngineUIImageWrap.__Register);
				
				translator.DelayWrapLoader(typeof(ImageEx), ImageExWrap.__Register);
				
				translator.DelayWrapLoader(typeof(RawImageEx), RawImageExWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.UI.Button), UnityEngineUIButtonWrap.__Register);
				
				translator.DelayWrapLoader(typeof(ButtonEx), ButtonExWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.UI.Button.ButtonClickedEvent), UnityEngineUIButtonButtonClickedEventWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UILoading), UILoadingWrap.__Register);
				
				translator.DelayWrapLoader(typeof(Singleton<AssetUtil>), Singleton_1_AssetUtil_Wrap.__Register);
				
				translator.DelayWrapLoader(typeof(AssetUtil), AssetUtilWrap.__Register);
				
				translator.DelayWrapLoader(typeof(GameObjectUtil), GameObjectUtilWrap.__Register);
				
				translator.DelayWrapLoader(typeof(AssetEntity), AssetEntityWrap.__Register);
				
				translator.DelayWrapLoader(typeof(UnityEngine.Events.UnityEvent), UnityEngineEventsUnityEventWrap.__Register);
				
				translator.DelayWrapLoader(typeof(LuaDebugTool), LuaDebugToolWrap.__Register);
				
				
				
			});
		}
		
		
	}
	
}
namespace XLua
{
	public partial class ObjectTranslator
	{
		static XLua.CSObjectWrap.XLua_Gen_Initer_Register__ s_gen_reg_dumb_obj = new XLua.CSObjectWrap.XLua_Gen_Initer_Register__();
		static XLua.CSObjectWrap.XLua_Gen_Initer_Register__ gen_reg_dumb_obj {get{return s_gen_reg_dumb_obj;}}
	}
	
	internal partial class InternalGlobals
    {
	    
	    static InternalGlobals()
		{
		    extensionMethodMap = new Dictionary<Type, IEnumerable<MethodInfo>>()
			{
			    
			};
			
			genTryArrayGetPtr = StaticLuaCallbacks.__tryArrayGet;
            genTryArraySetPtr = StaticLuaCallbacks.__tryArraySet;
		}
	}
}
