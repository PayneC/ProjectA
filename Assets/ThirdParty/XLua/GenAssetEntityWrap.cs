#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class AssetEntityWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(AssetEntity);
			Utils.BeginObjectRegister(type, L, translator, 0, 5, 4, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AddListener", _m_AddListener);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveListener", _m_RemoveListener);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetAsset", _m_SetAsset);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OnLoaded", _m_OnLoaded);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetInstantiate", _m_GetInstantiate);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "IsLoaded", _g_get_IsLoaded);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "assetType", _g_get_assetType);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "assetName", _g_get_assetName);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "hashCode", _g_get_hashCode);
            
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            
			try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
				if(LuaAPI.lua_gettop(L) == 3 && (LuaAPI.lua_isnil(L, 2) || LuaAPI.lua_type(L, 2) == LuaTypes.LUA_TSTRING) && translator.Assignable<EAssetType>(L, 3))
				{
					string n = LuaAPI.lua_tostring(L, 2);
					EAssetType t;translator.Get(L, 3, out t);
					
					AssetEntity __cl_gen_ret = new AssetEntity(n, t);
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to AssetEntity constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AddListener(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AssetEntity __cl_gen_to_be_invoked = (AssetEntity)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    DAssetsCallback _func = translator.GetDelegate<DAssetsCallback>(L, 2);
                    
                    __cl_gen_to_be_invoked.AddListener( _func );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveListener(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AssetEntity __cl_gen_to_be_invoked = (AssetEntity)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    DAssetsCallback _func = translator.GetDelegate<DAssetsCallback>(L, 2);
                    
                    __cl_gen_to_be_invoked.RemoveListener( _func );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetAsset(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AssetEntity __cl_gen_to_be_invoked = (AssetEntity)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Object _mainAsset = (UnityEngine.Object)translator.GetObject(L, 2, typeof(UnityEngine.Object));
                    
                    __cl_gen_to_be_invoked.SetAsset( _mainAsset );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OnLoaded(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AssetEntity __cl_gen_to_be_invoked = (AssetEntity)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.OnLoaded(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetInstantiate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AssetEntity __cl_gen_to_be_invoked = (AssetEntity)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        UnityEngine.Object __cl_gen_ret = __cl_gen_to_be_invoked.GetInstantiate(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_IsLoaded(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                AssetEntity __cl_gen_to_be_invoked = (AssetEntity)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.IsLoaded);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_assetType(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                AssetEntity __cl_gen_to_be_invoked = (AssetEntity)translator.FastGetCSObj(L, 1);
                translator.PushEAssetType(L, __cl_gen_to_be_invoked.assetType);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_assetName(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                AssetEntity __cl_gen_to_be_invoked = (AssetEntity)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, __cl_gen_to_be_invoked.assetName);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_hashCode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                AssetEntity __cl_gen_to_be_invoked = (AssetEntity)translator.FastGetCSObj(L, 1);
                LuaAPI.xlua_pushinteger(L, __cl_gen_to_be_invoked.hashCode);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
