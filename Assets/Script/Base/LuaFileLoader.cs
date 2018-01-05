using LuaInterface;
using System.IO;
using UnityEngine;

public class LuaFileLoader : LuaFileUtils
{
    public static void CreateInstance()
    {
        if (instance == null)
        {
            instance = new LuaFileLoader();
        }
    }

    public override byte[] ReadFile(string fileName)
    {
        if (!beZip)
        {
            string path = FindFile(fileName);
            byte[] str = null;

            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
#if !UNITY_WEBPLAYER
                str = File.ReadAllBytes(path);
#else
                throw new LuaException("can't run in web platform, please switch to other platform");
#endif
            }

            return str;
        }
        else
        {
            return ReadBundleFile(fileName);
        }
    }

    private byte[] ReadBundleFile(string fileName)
    {
        AssetBundle zipFile = null;
        byte[] buffer = null;
        string zipName = "lua";

        using (CString.Block())
        {
            fileName = fileName.Replace('/', '_');
            fileName = fileName.Replace('\\', '_');
            if (!fileName.EndsWith(".lua"))
            {
                fileName += ".lua";
            }

#if UNITY_5 || UNITY_2017
            fileName += ".bytes";
#endif
            zipMap.TryGetValue(zipName, out zipFile);
        }

        if (zipFile != null)
        {
#if UNITY_5 || UNITY_2017
            TextAsset luaCode = zipFile.LoadAsset<TextAsset>(fileName);
#else
            TextAsset luaCode = zipFile.Load(fileName, typeof(TextAsset)) as TextAsset;
#endif

            if (luaCode != null)
            {
                buffer = luaCode.bytes;
                Resources.UnloadAsset(luaCode);
            }
        }

        return buffer;
    }
}
