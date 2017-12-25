using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class GameObjectUtil
{
    static public GameObject Find(string _path)
    {
        return GameObject.Find(_path);
    }

    static public GameObject New(string _name)
    {
        if (string.IsNullOrEmpty(_name))
        {
            return new GameObject();
        }
        else
        {
            return new GameObject(_name);
        }
    }

    static public void Destroy(Object _obj)
    {
        Object.Destroy(_obj);
    }

    static public Object Instantiate(Object _go)
    {
        return Object.Instantiate(_go);
    }

    static public GameObject FindChild(GameObject _go, string _path)
    {
        Transform _tran = _go == null ? null : _go.transform;

        if (_tran != null)
        {
            Transform _child = _tran.Find(_path);
            return _child == null ? null : _child.gameObject;
        }
        else
        {
            Debug.LogError("GameObjectUtil FindChild GameObject is null");
            return null;
        }
    }

   //static public void SetLuaRef(GameObject _go, XLua.LuaTable _table)
   //{
   //    LuaRef _luaRef = _go.GetComponent<LuaRef>();
   //    if(_luaRef == null)
   //    {
   //        _luaRef = _go.AddComponent<LuaRef>();
   //    }
   //    _luaRef.table = _table;
   //}

    static public Component AddComponent(GameObject _go, System.Type type, string _path)
    {
        if (type == null)
        {
            Debug.LogError("GameObjectUtil GetComponent type is null");
            return null;
        }

        GameObject _child;

        if (string.IsNullOrEmpty(_path))
        {
            _child = _go;
        }
        else
        {
            _child = FindChild(_go, _path);
        }

        if (_child != null)
        {
            return _child.AddComponent(type);
        }
        else
        {
            return null;
        }
    }

    static public Component GetComponent(GameObject _go, System.Type type, string _path)
    {
        if (type == null)
        {
            Debug.LogError("GameObjectUtil GetComponent type is null");
            return null;
        }

        GameObject _child;

        if (string.IsNullOrEmpty(_path))
        {
            _child = _go;
        }
        else
        {
            _child = FindChild(_go, _path);
        }

        if (_child != null)
        {
            Component _component = _child.GetComponent(type);
            if(_component == null)
            {
                return null;
            }
            return _component;
        }
        else
        {
            return null;
        }
    }

    static public void SetActive(GameObject _go, bool _active)
    {
        if(_go != null)
        {
            _go.SetActive(_active);
        }
    }

    static public bool GetActive(GameObject _go)
    {
        if(_go != null)
        {
            return _go.activeSelf;
        }
        return false;
    }

    static public void SetParent(GameObject _go, GameObject _parent)
    {
        Transform _tran = _go == null ? null : _go.transform;

        if (_tran != null)
        {
            Transform _parentTran = _parent == null ? null : _parent.transform;
            _tran.SetParent(_parentTran);
        }
        else
        {
            Debug.LogError("GameObjectUtil SetParent GameObject is null");
        }
    }

    static public GameObject GetParent(GameObject _go)
    {
        Transform _tran = _go == null ? null : _go.transform;

        if (_tran != null)
        {
            Transform _parentTran = _tran.parent;
            return _parentTran == null ? null : _parentTran.gameObject;
        }
        else
        {
            Debug.LogError("GameObjectUtil SetParent GameObject is null");
            return null;
        }
    }

    static public void SetPosition(GameObject _go, Vector3 vec3)
    {
        Transform _tran = _go == null ? null : _go.transform;
        if (_tran != null)
        {
            _tran.position = vec3;
        }
        else
        {
            Debug.LogError("GameObjectUtil SetPosition GameObject is null");
        }
    }

    static public void SetLocalPosition(GameObject _go, Vector3 vec3)
    {
        Transform _tran = _go == null ? null : _go.transform;
        if (_tran != null)
        {
            _tran.localPosition = vec3;
        }
        else
        {
            Debug.LogError("GameObjectUtil SetLocalPosition GameObject is null");
        }
    }

    static public void SetEulerAngles(GameObject _go, Vector3 vec3)
    {
        Transform _tran = _go == null ? null : _go.transform;
        if (_tran != null)
        {
            _tran.eulerAngles = vec3;
        }
        else
        {
            Debug.LogError("GameObjectUtil SetEulerAngles GameObject is null");
        }
    }

    static public void SetLocalEulerAngles(GameObject _go, Vector3 vec3)
    {
        Transform _tran = _go == null ? null : _go.transform;
        if (_tran != null)
        {
            _tran.localEulerAngles = vec3;
        }
        else
        {
            Debug.LogError("GameObjectUtil SetLocalEulerAngles GameObject is null");
        }
    }

    static public void SetLocalScale(GameObject _go, Vector3 vec3)
    {
        Transform _tran = _go == null ? null : _go.transform;
        if (_tran != null)
        {
            _tran.localScale = vec3;
        }
        else
        {
            Debug.LogError("GameObjectUtil SetLocalScale GameObject is null");
        }
    }

    static public Vector3 GetPosition(GameObject _go)
    {
        Transform _tran = _go == null ? null : _go.transform;
        if (_tran != null)
        {
            return _tran.position;
        }
        else
        {
            Debug.LogError("GameObjectUtil GetPosition GameObject is null");
            return Vector3.zero;
        }
    }

    static public Vector3 GetLocalPosition(GameObject _go)
    {
        Transform _tran = _go == null ? null : _go.transform;
        if (_tran != null)
        {
            return _tran.localPosition;
        }
        else
        {
            Debug.LogError("GameObjectUtil GetLocalPosition GameObject is null");
            return Vector3.zero;
        }
    }

    static public Vector3 GetEulerAngles(GameObject _go)
    {
        Transform _tran = _go == null ? null : _go.transform;
        if (_tran != null)
        {
            return _tran.eulerAngles;
        }
        else
        {
            Debug.LogError("GameObjectUtil GetEulerAngles GameObject is null");
            return Vector3.zero;
        }
    }

    static public Vector3 GetLocalEulerAngles(GameObject _go)
    {
        Transform _tran = _go == null ? null : _go.transform;
        if (_tran != null)
        {
            return _tran.localEulerAngles;
        }
        else
        {
            Debug.LogError("GameObjectUtil GetLocalEulerAngles GameObject is null");
            return Vector3.zero;
        }
    }

    static public Vector3 GetLocalScale(GameObject _go)
    {
        Transform _tran = _go == null ? null : _go.transform;
        if (_tran != null)
        {
            return _tran.localScale;
        }
        else
        {
            Debug.LogError("GameObjectUtil GetLocalScale GameObject is null");
            return Vector3.zero;
        }
    }

    static public Vector3 GetForward(GameObject _go)
    {
        Transform _tran = _go == null ? null : _go.transform;
        if (_tran != null)
        {
            return _tran.forward;
        }
        else
        {
            Debug.LogError("GameObjectUtil GetLocalScale GameObject is null");
            return Vector3.zero;
        }
    }

    static public void SetForward(GameObject _go, Vector3 vec3)
    {
        Transform _tran = _go == null ? null : _go.transform;
        if (_tran != null)
        {
            _tran.forward = vec3;
        }
        else
        {
            Debug.LogError("GameObjectUtil SetEulerAngles GameObject is null");
        }
    }

    static public void LookAt(GameObject _go, Vector3 vec3)
    {
        Transform _tran = _go == null ? null : _go.transform;
        if (_tran != null)
        {
            _tran.LookAt(vec3);
        }
        else
        {
            Debug.LogError("GameObjectUtil GetLocalScale GameObject is null");
        }
    }

    static public Vector3 LocalToWorld(GameObject _go, Vector3 vec3)
    {
        if(null != _go)
        {
            return _go.transform.localToWorldMatrix * vec3;
        }
        else
        {
            return vec3;
        }
    }

    static public Vector3 WorldToLocal(GameObject _go, Vector3 vec3)
    {
        if (null != _go)
        {
            return _go.transform.worldToLocalMatrix * vec3;
        }
        else
        {
            return vec3;
        }
    }

    static public Vector3 LocalToWorldNormal(GameObject _go, Vector3 vec3)
    {
        if (null != _go)
        {
            return _go.transform.localToWorldMatrix * vec3.normalized;
        }
        else
        {
            return vec3.normalized;
        }
    }

    static public Vector3 WorldToLocalNormal(GameObject _go, Vector3 vec3)
    {
        if (null != _go)
        {
            return _go.transform.worldToLocalMatrix * vec3.normalized;
        }
        else
        {
            return vec3.normalized;
        }
    }
}
