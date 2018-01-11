﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class TimeUtilWrap
{
	public static void Register(LuaState L)
	{
		L.BeginStaticLibs("TimeUtil");
		L.RegFunction("SetServerTime", SetServerTime);
		L.RegFunction("GetServerTime", GetServerTime);
		L.RegFunction("GetTimeStamp", GetTimeStamp);
		L.EndStaticLibs();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int SetServerTime(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			double arg0 = (double)LuaDLL.luaL_checknumber(L, 1);
			TimeUtil.SetServerTime(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetServerTime(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			double o = TimeUtil.GetServerTime();
			LuaDLL.lua_pushnumber(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetTimeStamp(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 0);
			double o = TimeUtil.GetTimeStamp();
			LuaDLL.lua_pushnumber(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}
