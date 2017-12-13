using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

public class LuaMgr : Singleton<LuaMgr>
{
    [CSharpCallLua]
    public delegate int FDelegate(float _dt);

    private LuaEnv luaEnv = null;
    private AssetBundle luaAssets;
    public bool isLoaded { private set; get; }
    FDelegate _update;


    public void EnterGame()
    {
#if UNITY_EDITOR && !TEST_AB
        isLoaded = true;
#else
        SyncLoadLuaAsset();
#endif
        luaEnv = new LuaEnv();
        luaEnv.AddLoader(CustomLoader);
        luaEnv.DoString("require 'main' \n OnEnter()");

        _update = luaEnv.Global.Get<FDelegate>("Update");
    }

    public void ExitGame()
    {
        if (null != _update)
        {
            _update = null;
        }

        luaEnv.DoString("require 'main' \n OnExit()");

        if (null != luaEnv)
        {
            luaEnv.Dispose();
            luaEnv = null;
        }
    }

    public void Update(float dt)
    {
        if (!isLoaded)
            return;

        if (null != _update)
        {
            _update(dt);
        }
    }

    /// <summary>
    /// 加载Lua包
    /// 预先加载完所有的Lua文件
    /// </summary>
    /// <returns></returns>
    public void SyncLoadLuaAsset()
    {
        isLoaded = false;
        luaAssets = AssetBundle.LoadFromFile(PathUtil.I.GetStreamingAssesPath() + "lua");
        object[] _ojs = luaAssets.LoadAllAssets();
        isLoaded = true;
    }

    /// <summary>
    /// 加载Lua包
    /// 预先加载完所有的Lua文件
    /// </summary>
    /// <returns></returns>
    public IEnumerator AsyncLoadLuaAsset()
    {
        isLoaded = false;
        AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromFileAsync("ScriptLua");
        yield return assetBundleCreateRequest;
        luaAssets = assetBundleCreateRequest.assetBundle;
        AssetBundleRequest bundleRequest = luaAssets.LoadAllAssetsAsync();
        yield return bundleRequest;
        isLoaded = true;
    }

    /// <summary>
    /// require lua
    /// 文件查找接口
    /// </summary>
    /// <param name="filepath"></param>
    /// <returns></returns>
    private byte[] CustomLoader(ref string filepath)
    {
        byte[] _byte = null;
#if UNITY_EDITOR && !TEST_AB
        filepath = string.Format("{0}{1}{2}{3}", Application.dataPath, "/Lua/", filepath, ".lua");

        if (File.Exists(filepath))
        {
            _byte = File.ReadAllBytes(filepath);
        }
#else
        filepath = filepath.Replace("\\", "/");
        int _index = filepath.LastIndexOf('/');
        if (_index > 0)
        {
            filepath = filepath.Remove(0, _index + 1);
        }
        TextAsset _text = luaAssets.LoadAsset<TextAsset>(filepath);
        _byte = _text.bytes;
#endif
        return _byte;
    }
}
