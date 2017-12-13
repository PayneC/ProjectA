using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DAtlas(UIAtlas atlas);

public class AtlasUtility
{
    private static AtlasUtility _instance;
    private static readonly object lockHelper = new object();

    public static AtlasUtility I
    {
        get
        {
            if (_instance == null)
            {
                lock (lockHelper)
                {
                    if (_instance == null)
                        _instance = new AtlasUtility();
                }
            }
            return _instance;
        }
    }

    public static bool HasInstance()
    {
        return _instance != null;
    }

    private Dictionary<string, UIAtlas> atlass = new Dictionary<string, UIAtlas>();
    private Dictionary<string, List<DAtlas>> waits = new Dictionary<string, List<DAtlas>>();

    private GameObject _root = null;

    public void Close()
    {
        if (atlass != null)
        {
            atlass.Clear();
        }

        if (waits != null)
        {
            waits.Clear();
        }
    }

    public void AsyncLoadAtlas(string name, DAtlas func)
    {
        UIAtlas atlas = null;
        if (atlass.TryGetValue(name, out atlas))
        {
            if (func != null)
                func(atlas);
        }
        else
        {
            if (AssetUtil.I != null)
            {
                if (!waits.ContainsKey(name))
                {
                    waits[name] = new List<DAtlas>();
                }
                waits[name].Add(func);
            }
        }
    }

    public UIAtlas GetAtlas(string name)
    {
        UIAtlas atlas = null;
        atlass.TryGetValue(name, out atlas);
        return atlas;
    }

    private void callback(int instanID, int type, string name, Object oj)
    {
        GameObject go = oj as GameObject;
        UIAtlas atlas = go.GetComponent<UIAtlas>();

        if (atlas == null)
        {
            Debug.LogErrorFormat("Atlas {0} load fail", name);
            return;
        }

        atlass[name] = atlas;
        if (_root == null)
        {
            _root = new GameObject("Atlas");
            _root.transform.localPosition = Vector3.zero;
            _root.transform.localRotation = Quaternion.identity;
            _root.transform.localScale = Vector3.one;
        }

        List<DAtlas> funcs = null;
        if (waits.TryGetValue(name, out funcs) && funcs != null && funcs.Count > 0)
        {
            for (int i = 0; i < funcs.Count; ++i)
            {
                if(funcs[i] != null)
                    funcs[i](atlas);
            }
        }

        waits.Remove(name);
    }
}
