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
    public class AssetUtilWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(AssetUtil);
			Utils.BeginObjectRegister(type, L, translator, 0, 11, 1, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Load", _m_Load);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AsyncLoad", _m_AsyncLoad);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "RemoveAsyncCallback", _m_RemoveAsyncCallback);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetAbsolutePath", _m_GetAbsolutePath);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetRelativePath", _m_GetRelativePath);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "GetManifestPath", _m_GetManifestPath);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Update", _m_Update);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "CoroutineUpdate", _m_CoroutineUpdate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "AsyncLoadAssetManifest", _m_AsyncLoadAssetManifest);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Recycle", _m_Recycle);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ClearAssets", _m_ClearAssets);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "currentLoading", _g_get_currentLoading);
            
			
			
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
				if(LuaAPI.lua_gettop(L) == 1)
				{
					
					AssetUtil __cl_gen_ret = new AssetUtil();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to AssetUtil constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Load(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AssetUtil __cl_gen_to_be_invoked = (AssetUtil)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 2);
                    
                        UnityEngine.Object __cl_gen_ret = __cl_gen_to_be_invoked.Load( _path );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AsyncLoad(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AssetUtil __cl_gen_to_be_invoked = (AssetUtil)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    EAssetType assetType;translator.Get(L, 2, out assetType);
                    string _path = LuaAPI.lua_tostring(L, 3);
                    DAssetsCallback call = translator.GetDelegate<DAssetsCallback>(L, 4);
                    
                        AssetEntity __cl_gen_ret = __cl_gen_to_be_invoked.AsyncLoad( assetType, _path, call );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_RemoveAsyncCallback(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AssetUtil __cl_gen_to_be_invoked = (AssetUtil)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    EAssetType assetType;translator.Get(L, 2, out assetType);
                    string _path = LuaAPI.lua_tostring(L, 3);
                    DAssetsCallback call = translator.GetDelegate<DAssetsCallback>(L, 4);
                    
                    __cl_gen_to_be_invoked.RemoveAsyncCallback( assetType, _path, call );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetAbsolutePath(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AssetUtil __cl_gen_to_be_invoked = (AssetUtil)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 2);
                    EAssetType assetType;translator.Get(L, 3, out assetType);
                    
                        string __cl_gen_ret = __cl_gen_to_be_invoked.GetAbsolutePath( _path, assetType );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetRelativePath(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AssetUtil __cl_gen_to_be_invoked = (AssetUtil)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _path = LuaAPI.lua_tostring(L, 2);
                    EAssetType assetType;translator.Get(L, 3, out assetType);
                    
                        string __cl_gen_ret = __cl_gen_to_be_invoked.GetRelativePath( _path, assetType );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_GetManifestPath(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AssetUtil __cl_gen_to_be_invoked = (AssetUtil)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        string __cl_gen_ret = __cl_gen_to_be_invoked.GetManifestPath(  );
                        LuaAPI.lua_pushstring(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Update(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AssetUtil __cl_gen_to_be_invoked = (AssetUtil)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float dt = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    __cl_gen_to_be_invoked.Update( dt );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_CoroutineUpdate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AssetUtil __cl_gen_to_be_invoked = (AssetUtil)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        System.Collections.IEnumerator __cl_gen_ret = __cl_gen_to_be_invoked.CoroutineUpdate(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_AsyncLoadAssetManifest(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AssetUtil __cl_gen_to_be_invoked = (AssetUtil)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        System.Collections.IEnumerator __cl_gen_ret = __cl_gen_to_be_invoked.AsyncLoadAssetManifest(  );
                        translator.Push(L, __cl_gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Recycle(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AssetUtil __cl_gen_to_be_invoked = (AssetUtil)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    UnityEngine.Object oj = (UnityEngine.Object)translator.GetObject(L, 2, typeof(UnityEngine.Object));
                    
                    __cl_gen_to_be_invoked.Recycle( oj );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ClearAssets(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                AssetUtil __cl_gen_to_be_invoked = (AssetUtil)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.ClearAssets(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_currentLoading(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                AssetUtil __cl_gen_to_be_invoked = (AssetUtil)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.currentLoading);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
