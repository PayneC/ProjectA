﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class AssetUtilWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(AssetUtil), typeof(Singleton<AssetUtil>));
		L.RegFunction("Load", Load);
		L.RegFunction("AsyncLoad", AsyncLoad);
		L.RegFunction("RemoveAsyncCallback", RemoveAsyncCallback);
		L.RegFunction("GetAbsolutePath", GetAbsolutePath);
		L.RegFunction("GetRelativePath", GetRelativePath);
		L.RegFunction("GetManifestPath", GetManifestPath);
		L.RegFunction("Update", Update);
		L.RegFunction("CoroutineUpdate", CoroutineUpdate);
		L.RegFunction("AsyncLoadAssetManifest", AsyncLoadAssetManifest);
		L.RegFunction("Recycle", Recycle);
		L.RegFunction("ClearAssets", ClearAssets);
		L.RegFunction("New", _CreateAssetUtil);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("currentLoading", get_currentLoading, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int _CreateAssetUtil(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 0)
			{
				AssetUtil obj = new AssetUtil();
				ToLua.PushObject(L, obj);
				return 1;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to ctor method: AssetUtil.New");
			}
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Load(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			AssetUtil obj = (AssetUtil)ToLua.CheckObject<AssetUtil>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			UnityEngine.Object o = obj.Load(arg0);
			ToLua.Push(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AsyncLoad(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 4);
			AssetUtil obj = (AssetUtil)ToLua.CheckObject<AssetUtil>(L, 1);
			EAssetType arg0 = (EAssetType)ToLua.CheckObject(L, 2, typeof(EAssetType));
			string arg1 = ToLua.CheckString(L, 3);
			DAssetsCallback arg2 = (DAssetsCallback)ToLua.CheckDelegate<DAssetsCallback>(L, 4);
			AssetEntity o = obj.AsyncLoad(arg0, arg1, arg2);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int RemoveAsyncCallback(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 4);
			AssetUtil obj = (AssetUtil)ToLua.CheckObject<AssetUtil>(L, 1);
			EAssetType arg0 = (EAssetType)ToLua.CheckObject(L, 2, typeof(EAssetType));
			string arg1 = ToLua.CheckString(L, 3);
			DAssetsCallback arg2 = (DAssetsCallback)ToLua.CheckDelegate<DAssetsCallback>(L, 4);
			obj.RemoveAsyncCallback(arg0, arg1, arg2);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetAbsolutePath(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			AssetUtil obj = (AssetUtil)ToLua.CheckObject<AssetUtil>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			EAssetType arg1 = (EAssetType)ToLua.CheckObject(L, 3, typeof(EAssetType));
			string o = obj.GetAbsolutePath(arg0, arg1);
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetRelativePath(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			AssetUtil obj = (AssetUtil)ToLua.CheckObject<AssetUtil>(L, 1);
			string arg0 = ToLua.CheckString(L, 2);
			EAssetType arg1 = (EAssetType)ToLua.CheckObject(L, 3, typeof(EAssetType));
			string o = obj.GetRelativePath(arg0, arg1);
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetManifestPath(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			AssetUtil obj = (AssetUtil)ToLua.CheckObject<AssetUtil>(L, 1);
			string o = obj.GetManifestPath();
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Update(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			AssetUtil obj = (AssetUtil)ToLua.CheckObject<AssetUtil>(L, 1);
			float arg0 = (float)LuaDLL.luaL_checknumber(L, 2);
			obj.Update(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int CoroutineUpdate(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			AssetUtil obj = (AssetUtil)ToLua.CheckObject<AssetUtil>(L, 1);
			System.Collections.IEnumerator o = obj.CoroutineUpdate();
			ToLua.Push(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int AsyncLoadAssetManifest(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			AssetUtil obj = (AssetUtil)ToLua.CheckObject<AssetUtil>(L, 1);
			System.Collections.IEnumerator o = obj.AsyncLoadAssetManifest();
			ToLua.Push(L, o);
			return 1;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Recycle(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			AssetUtil obj = (AssetUtil)ToLua.CheckObject<AssetUtil>(L, 1);
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.CheckObject<UnityEngine.Object>(L, 2);
			obj.Recycle(arg0);
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ClearAssets(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			AssetUtil obj = (AssetUtil)ToLua.CheckObject<AssetUtil>(L, 1);
			obj.ClearAssets();
			return 0;
		}
		catch (Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_currentLoading(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			AssetUtil obj = (AssetUtil)o;
			AssetEntity ret = obj.currentLoading;
			ToLua.PushObject(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o, "attempt to index currentLoading on a nil value");
		}
	}
}
