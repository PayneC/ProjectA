﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class UIGridElementWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(UIGridElement), typeof(UnityEngine.MonoBehaviour));
		L.RegFunction("SetIndex", SetIndex);
		L.RegFunction("GetIndex", GetIndex);
		L.RegFunction("AddIndexChangeListener", AddIndexChangeListener);
		L.RegFunction("RemoveIndexChangeListener", RemoveIndexChangeListener);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("index", get_index, set_index);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetIndex(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UIGridElement obj = (UIGridElement)ToLua.CheckObject<UIGridElement>(L, 1);
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			obj.SetIndex(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetIndex(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			UIGridElement obj = (UIGridElement)ToLua.CheckObject<UIGridElement>(L, 1);
			int o = obj.GetIndex();
			LuaDLL.lua_pushinteger(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AddIndexChangeListener(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UIGridElement obj = (UIGridElement)ToLua.CheckObject<UIGridElement>(L, 1);
			UnityEngine.Events.UnityAction arg0 = (UnityEngine.Events.UnityAction)ToLua.CheckDelegate<UnityEngine.Events.UnityAction>(L, 2);
			obj.AddIndexChangeListener(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveIndexChangeListener(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UIGridElement obj = (UIGridElement)ToLua.CheckObject<UIGridElement>(L, 1);
			UnityEngine.Events.UnityAction arg0 = (UnityEngine.Events.UnityAction)ToLua.CheckDelegate<UnityEngine.Events.UnityAction>(L, 2);
			obj.RemoveIndexChangeListener(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int op_Equality(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
			UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.ToObject(L, 2);
			bool o = arg0 == arg1;
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_index(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UIGridElement obj = (UIGridElement)o;
			int ret = obj.index;
			LuaDLL.lua_pushinteger(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index index on a nil value");
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int set_index(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			UIGridElement obj = (UIGridElement)o;
			int arg0 = (int)LuaDLL.luaL_checknumber(L, 2);
			obj.index = arg0;
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index index on a nil value");
		}
	}
}

