using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpriteData
{
    public string name;
    public int alignment;
    public Vector4 border;
    public Vector2 pivot;
    public Rect rect;
}

public class UIAtlas : ScriptableObject
{
    public Texture2D _tex2d = null;
    public List<SpriteData> _uvs = new List<SpriteData>();

    public Sprite GetSpriteByName(string spriteName)
    {
        Sprite sp = null;
        SpriteData rt = null;
        for (int i = 0; i < _uvs.Count; ++i)
        {
            if (_uvs[i].name.Equals(spriteName))
            {
                rt = _uvs[i];
                sp = Sprite.Create(_tex2d, rt.rect, rt.pivot, 100f, 0, SpriteMeshType.Tight, rt.border);
                sp.name = _uvs[i].name;
            }
        }
        return sp;
    }

    public Sprite GetSpriteByName(string spriteName, Rect offest)
    {
        Sprite sp = null;
        SpriteData rt = null;
        for (int i = 0; i < _uvs.Count; ++i)
        {
            if (_uvs[i].name.Equals(spriteName))
            {
                rt = _uvs[i];
                sp = Sprite.Create(_tex2d, new Rect(rt.rect.x + offest.x, rt.rect.y + offest.y, rt.rect.width + offest.width, rt.rect.height + offest.height), rt.pivot, 100f, 0, SpriteMeshType.Tight, rt.border);
                sp.name = _uvs[i].name;
            }
        }
        return sp;
    }

    public bool HasSpriteData(string spriteName)
    {
        for (int i = 0; i < _uvs.Count; ++i)
        {
            if (_uvs[i].name.Equals(spriteName))
            {
                return true;
            }
        }
        return false;
    }
}