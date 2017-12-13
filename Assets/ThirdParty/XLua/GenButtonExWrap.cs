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
    public class ButtonExWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(ButtonEx);
			Utils.BeginObjectRegister(type, L, translator, 0, 0, 4, 4);
			
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "disabledSpriteInfo", _g_get_disabledSpriteInfo);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "highlightedSpriteInfo", _g_get_highlightedSpriteInfo);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "pressedSpriteInfo", _g_get_pressedSpriteInfo);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "useClickAnimation", _g_get_useClickAnimation);
            
			Utils.RegisterFunc(L, Utils.SETTER_IDX, "disabledSpriteInfo", _s_set_disabledSpriteInfo);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "highlightedSpriteInfo", _s_set_highlightedSpriteInfo);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "pressedSpriteInfo", _s_set_pressedSpriteInfo);
            Utils.RegisterFunc(L, Utils.SETTER_IDX, "useClickAnimation", _s_set_useClickAnimation);
            
			
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
					
					ButtonEx __cl_gen_ret = new ButtonEx();
					translator.Push(L, __cl_gen_ret);
                    
					return 1;
				}
				
			}
			catch(System.Exception __gen_e) {
				return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
			}
            return LuaAPI.luaL_error(L, "invalid arguments to ButtonEx constructor!");
            
        }
        
		
        
		
        
        
        
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_disabledSpriteInfo(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ButtonEx __cl_gen_to_be_invoked = (ButtonEx)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.disabledSpriteInfo);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_highlightedSpriteInfo(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ButtonEx __cl_gen_to_be_invoked = (ButtonEx)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.highlightedSpriteInfo);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_pressedSpriteInfo(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ButtonEx __cl_gen_to_be_invoked = (ButtonEx)translator.FastGetCSObj(L, 1);
                translator.Push(L, __cl_gen_to_be_invoked.pressedSpriteInfo);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_useClickAnimation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ButtonEx __cl_gen_to_be_invoked = (ButtonEx)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, __cl_gen_to_be_invoked.useClickAnimation);
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 1;
        }
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_disabledSpriteInfo(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ButtonEx __cl_gen_to_be_invoked = (ButtonEx)translator.FastGetCSObj(L, 1);
                SpriteInfo __cl_gen_value;translator.Get(L, 2, out __cl_gen_value);
				__cl_gen_to_be_invoked.disabledSpriteInfo = __cl_gen_value;
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_highlightedSpriteInfo(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ButtonEx __cl_gen_to_be_invoked = (ButtonEx)translator.FastGetCSObj(L, 1);
                SpriteInfo __cl_gen_value;translator.Get(L, 2, out __cl_gen_value);
				__cl_gen_to_be_invoked.highlightedSpriteInfo = __cl_gen_value;
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_pressedSpriteInfo(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ButtonEx __cl_gen_to_be_invoked = (ButtonEx)translator.FastGetCSObj(L, 1);
                SpriteInfo __cl_gen_value;translator.Get(L, 2, out __cl_gen_value);
				__cl_gen_to_be_invoked.pressedSpriteInfo = __cl_gen_value;
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _s_set_useClickAnimation(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                ButtonEx __cl_gen_to_be_invoked = (ButtonEx)translator.FastGetCSObj(L, 1);
                __cl_gen_to_be_invoked.useClickAnimation = LuaAPI.lua_toboolean(L, 2);
            
            } catch(System.Exception __gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + __gen_e);
            }
            return 0;
        }
        
		
		
		
		
    }
}
