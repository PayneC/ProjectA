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
    public class UILoadingWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(UILoading);
			Utils.BeginObjectRegister(type, L, translator, 0, 2, 3, 3);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetLoading", _m_SetLoading);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OpenLoading", _m_OpenLoading);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "tex_bg", _g_get_tex_bg);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "sli_progress", _g_get_sli_progress);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "txt_progress", _g_get_txt_progress);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "tex_bg", _s_set_tex_bg);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "sli_progress", _s_set_sli_progress);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "txt_progress", _s_set_txt_progress);
            
			
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
					
					UILoading __cl_gen_ret = new UILoading();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to UILoading constructor!");
            
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLoading(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UILoading __cl_gen_to_be_invoked = (UILoading)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float t = (float)LuaAPI.lua_tonumber(L, 2);
                    float p = (float)LuaAPI.lua_tonumber(L, 3);
                    bool autoClose = LuaAPI.lua_toboolean(L, 4);
                    
                    __cl_gen_to_be_invoked.SetLoading( t, p, autoClose );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OpenLoading(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                UILoading __cl_gen_to_be_invoked = (UILoading)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    __cl_gen_to_be_invoked.OpenLoading(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_tex_bg(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILoading __cl_gen_to_be_invoked = (UILoading)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.tex_bg);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_sli_progress(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILoading __cl_gen_to_be_invoked = (UILoading)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.sli_progress);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_txt_progress(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILoading __cl_gen_to_be_invoked = (UILoading)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.txt_progress);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_tex_bg(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILoading __cl_gen_to_be_invoked = (UILoading)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.tex_bg = (UnityEngine.UI.RawImage)translator.GetObject(L, 2, typeof(UnityEngine.UI.RawImage));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_sli_progress(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILoading __cl_gen_to_be_invoked = (UILoading)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.sli_progress = (UnityEngine.UI.Slider)translator.GetObject(L, 2, typeof(UnityEngine.UI.Slider));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_txt_progress(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                UILoading __cl_gen_to_be_invoked = (UILoading)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.txt_progress = (UnityEngine.UI.Text)translator.GetObject(L, 2, typeof(UnityEngine.UI.Text));
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
