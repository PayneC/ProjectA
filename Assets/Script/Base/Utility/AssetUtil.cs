using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;

public enum EAssetType
{
    NONE = 0,
    LUA = 1,
    ENTITY = 2,
    TEXTURE = 3,
    SCENE = 4,
    UI = 5,
    ATLAS = 6,
}

[CSharpCallLua]
public delegate void DAssetsCallback(AssetEntity _entity);

public class AssetEntity
{
    public bool IsLoaded { private set; get; }
    public EAssetType assetType { private set; get; }
    public string assetName { private set; get; }
    public int hashCode { private set; get; }

    private DAssetsCallback fCallback;
    private Object _original;
    private Object _instance;
    private Queue<Object> _pool;

    public AssetEntity(string n, EAssetType t)
    {
        assetName = n;
        hashCode = assetName.GetHashCode();
        assetType = t;
    }

    public void AddListener(DAssetsCallback _func)
    {
        fCallback += _func;
    }

    public void RemoveListener(DAssetsCallback _func)
    {
        fCallback -= _func;
    }

    public void SetAsset(Object _mainAsset)
    {
        if (_mainAsset == null)
        {
            Debug.LogErrorFormat("{0} {1} load fail", assetType, assetName);
        }

        IsLoaded = true;
        _original = _mainAsset;
    }

    public void OnLoaded()
    {
        if (null != fCallback)
            fCallback(this);

        fCallback = null;
    }

    /// <summary>
    /// 获取实例
    /// </summary>
    /// <returns></returns>
    public Object GetInstantiate()
    {
        if (null == _original)
        {
            return null;
        }

        if (assetType == EAssetType.ATLAS
           || assetType == EAssetType.SCENE
           || assetType == EAssetType.UI)
        {
            if (_instance == null)
            {
                _instance = GameObject.Instantiate(_original);
                _instance.name = assetName;
            }

            return _instance;
        }
        else if (assetType == EAssetType.TEXTURE
           || assetType == EAssetType.LUA)
        {
            return _original;
        }
        else
        {
            Object _oj = GameObject.Instantiate(_original);
            _oj.name = assetName;
            return _oj;
        }
    }
}

public class AssetUtil : Singleton<AssetUtil>
{
    //当前加载的资源
    public AssetEntity currentLoading
    {
        get;
        private set;
    }

    //资源引用关系
    private AssetBundleManifest bundleManifest;
    //已经加载和等待加载的资源
    private Dictionary<string, AssetEntity> m_kAssets = new Dictionary<string, AssetEntity>();
    //已经加载的依赖资源
    private Dictionary<string, AssetBundle> m_kDependencies = new Dictionary<string, AssetBundle>();
    //等待加载的资源
    private Queue<AssetEntity> m_kWaitQueue = new Queue<AssetEntity>();
    //加载完的资源
    private Queue<AssetEntity> m_kLoaded = new Queue<AssetEntity>();
    private string m_sAssetTail = string.Empty;//".unity3d";
    public Object Load(string _path)
    {
        Object original = Resources.Load(_path);
        if (original == null)
            return null;

        return GameObject.Instantiate(original);
    }

    public T Load<T>(string _path) where T : Object
    {
        T original = Resources.Load<T>(_path);
        if (original == null)
            return null;

        return GameObject.Instantiate<T>(original);
    }

    public AssetEntity AsyncLoad(EAssetType assetType, string _path, DAssetsCallback call)
    {
        string _name = GetRelativePath(_path, assetType);
        AssetEntity _entity = null;
        if (!m_kAssets.TryGetValue(_name, out _entity))
        {
            _entity = new AssetEntity(_path, assetType);
            m_kAssets.Add(_name, _entity);
            m_kWaitQueue.Enqueue(_entity);
        }
        if (!_entity.IsLoaded)
        {
            _entity.AddListener(call);
        }
        else
        {
            call(_entity);
        }
        return _entity;
    }

    /// <summary>
    /// 取消资源加载监听
    /// </summary>
    /// <param name="_type">资源类型</param>
    /// <param name="_path">资源路径</param>
    /// <param name="call">要取消的回调函数</param>
    public void RemoveAsyncCallback(EAssetType assetType, string _path, DAssetsCallback call)
    {
        string _name = GetRelativePath(_path, assetType);
        AssetEntity _entity = null;
        if (m_kAssets.TryGetValue(_name, out _entity))
        {
            _entity.RemoveListener(call);
        }
    }

    public string GetAbsolutePath(string _path, EAssetType assetType)
    {
        return PathUtil.I.GetStreamingAssesPath() + GetRelativePath(_path, assetType);
    }

    /// <summary>
    /// 相对于ResourcesAB的路径
    /// </summary>
    /// <param name="_path">相对于ResourcesAB的路径</param>
    /// <param name="_type">资源类型</param>
    /// <returns></returns>
    public string GetRelativePath(string _path, EAssetType assetType)
    {
#if UNITY_EDITOR && !TEST_AB
        switch (assetType)
        {
            case EAssetType.LUA:
                return string.Format("Lua/{0}", _path);
            case EAssetType.ENTITY:
                return string.Format("Entity/{0}.prefab", _path);
            case EAssetType.TEXTURE:
                return string.Format("Texture/{0}.jpg", _path);
            case EAssetType.SCENE:
                return string.Format("Scene/{0}.prefab", _path);
            case EAssetType.UI:
                return string.Format("UI/{0}.prefab", _path);
            case EAssetType.ATLAS:
                return string.Format("Atlas/{0}.prefab", _path);
            default:
                return _path;
        }
#else
        switch (assetType)
        {
            case EAssetType.LUA:
                return string.Format("Lua/{0}", _path);
            case EAssetType.ENTITY:
                return string.Format("Entity/{0}", _path);
            case EAssetType.TEXTURE:
                return string.Format("Texture/{0}", _path);
            case EAssetType.SCENE:
                return string.Format("Scene/{0}", _path);
            case EAssetType.UI:
                return string.Format("UI/{0}", _path);
            case EAssetType.ATLAS:
                return string.Format("Atlas/{0}", _path);
            default:
                return _path;
        }
#endif
    }

    public string GetManifestPath()
    {
        return PathUtil.I.GetStreamingAssesPath() + PathUtil.I.GetRuntimePlatform();
    }

    public void Update(float dt)
    {
        if(m_kLoaded.Count > 0)
        {
            AssetEntity _entity = m_kLoaded.Dequeue();
            _entity.OnLoaded();
        }
    }

    public IEnumerator CoroutineUpdate()
    {
        //从等待队列里面取出待加载的资源
        currentLoading = null;
        if (m_kWaitQueue != null && m_kWaitQueue.Count > 0)
        {
            currentLoading = m_kWaitQueue.Dequeue();
        }

        if (currentLoading != null)
        {
            yield return _AsyncLoadAsset(currentLoading);
        }
        else
        {
            yield return null;
        }
    }

    //加载Manifest文件
    public IEnumerator AsyncLoadAssetManifest()
    {
        //加载AssetBundleManifest
        string manifestName = GetManifestPath();
        Debug.Log("manifestName : " + manifestName);

        AssetBundleCreateRequest assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(manifestName);
        yield return assetBundleCreateRequest;

        AssetBundle assetBundle = assetBundleCreateRequest.assetBundle;

        AssetBundleRequest bundleRequest = assetBundle.LoadAllAssetsAsync();
        yield return bundleRequest;

        bundleManifest = bundleRequest.asset as AssetBundleManifest;
        if (bundleManifest == null)
        {
            Debug.LogError("bundleManifest load fail");
            yield break;
        }

        assetBundle.Unload(false);
        yield return null;
    }

    public void Recycle(Object oj)
    {
        GameObject.DestroyObject(oj);
    }

    public void ClearAssets()
    {
        m_kAssets.Clear();
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }

    //通过AssetBundle的方式加载
    private IEnumerator _AsyncLoadDependencieAsset(string filePath)
    {
        Debug.LogFormat("dependenceAssetName : {0}", filePath);
        if (m_kDependencies.ContainsKey(filePath))
        {
            yield return null;
        }
        else
        {
            AssetBundleCreateRequest _assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(filePath);
            yield return _assetBundleCreateRequest;

            AssetBundle assetBundle = _assetBundleCreateRequest.assetBundle;
            m_kDependencies.Add(filePath, assetBundle);
            yield return null;
        }
    }

    //通过AssetBundle的方式加载
    private IEnumerator _AsyncLoadAsset(AssetEntity _entity)
    {
#if UNITY_EDITOR && !TEST_AB
        string AssetName = PathUtil.I.GetStreamingAssesPath() + GetRelativePath(_entity.assetName, _entity.assetType);
        Object _object = UnityEditor.AssetDatabase.LoadAssetAtPath<Object>(AssetName);
        _entity.SetAsset(_object);
        m_kLoaded.Enqueue(_entity);        
#else
        AssetBundle assetBundle = null;
        AssetBundleCreateRequest _assetBundleCreateRequest;

        string _path = GetRelativePath(_entity.assetName, _entity.assetType).ToLower();

        string[] dependences = bundleManifest.GetAllDependencies(_path);
        if (dependences != null && dependences.Length > 0)
        {
            //Debug.LogFormat("assets", "{0}依赖文件个数：{1}", _entity.assetName, dependences.Length);

            string dependenceAssetName = string.Empty;
            for (int i = 0; i < dependences.Length; ++i)
            {
                dependenceAssetName = PathUtil.I.GetStreamingAssesPath() + dependences[i];
                yield return _AsyncLoadDependencieAsset(dependenceAssetName);
            }
        }

        string AssetName = PathUtil.I.GetStreamingAssesPath() + _path;

        _assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(AssetName);
        yield return _assetBundleCreateRequest;

        assetBundle = _assetBundleCreateRequest.assetBundle;
        AssetBundleRequest bundleRequest = assetBundle.LoadAllAssetsAsync();
        yield return bundleRequest;

        _entity.SetAsset(bundleRequest.asset);
        m_kLoaded.Enqueue(_entity);

        assetBundle.Unload(false);
#endif
        yield return null;
    }
}
