using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class GenAssetBundle : EditorWindow
{
    [MenuItem("Tools/GenAssetBundle", priority = 2050)]
    static void ShowWindow()
    {
        var window = GetWindow<GenAssetBundle>();
        window.titleContent = new GUIContent("GenAssetBundle");
        window.Show();
    }

    static private List<string> luaDir = new List<string> { "/Lua", "/Tolua/Lua" };
    static private string luaBytesDir = "/ResourcesAB/Lua";

    void OnGUI()
    {
        //  if (EditorApplication.isCompiling)
        //    Close();

        for (int i = 0; i < luaDir.Count; ++i)
        {
            GUILayout.BeginHorizontal();
            GUILayout.TextField(luaDir[i]);
            GUILayout.EndHorizontal();
        }

        GUILayout.BeginHorizontal();
        luaBytesDir = GUILayout.TextField(luaBytesDir);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.TextField(Application.streamingAssetsPath);
        GUILayout.EndHorizontal();

        if (GUILayout.Button("CopyLuaBytesFiles"))
        {
            ClearLuaBytesFiles();
            for (int i = 0; i < luaDir.Count; ++i)
            {
                CopyLuaBytesFiles(Application.dataPath + luaDir[i], Application.dataPath + luaBytesDir);
            }       
        }

        if (GUILayout.Button("GenerateLuaAssetBundleNames"))
        {
            GenerateLuaAssetBundleNames();
        }

        if (GUILayout.Button("GenerateLuaBundle"))
        {
            GenerateLuaBundle(Application.dataPath + "/ResourcesAB/Lua/", Application.streamingAssetsPath);
        }

        if (GUILayout.Button("GenerateAssetBundleNames"))
        {
            GenerateAssetBundleNames();
        }

        if (GUILayout.Button("GenerateAssetBundle"))
        {
            BuildPipeline.BuildAssetBundles(Application.streamingAssetsPath, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
        }
    }

    // Use this for initialization
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void ClearLuaBytesFiles()
    {
        string path = Application.dataPath + "/" + luaBytesDir;
        string[] files;
        if (Directory.Exists(path))
        {
            files = Directory.GetFiles(path, "*.bytes", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; ++i)
            {
                File.Delete(files[i]);
            }
        }
    }

    public static void GenerateLuaBundle(string inPath, string outPath)
    {
        if(!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }

        string[] files = Directory.GetFiles(inPath, "*.bytes", SearchOption.AllDirectories);

        string[] assetNames = new string[files.Length];

        for (int i = 0; i < files.Length; i++)
        {
            string _assetPath = "Assets" + files[i].Substring(Application.dataPath.Length);
            _assetPath = _assetPath.Replace("\\", "/");
            assetNames[i] = _assetPath;
        }
        AssetDatabase.Refresh();

        AssetBundleBuild[] luaBundle = new AssetBundleBuild[1];
        luaBundle[0].assetBundleName = "lua";
        luaBundle[0].assetNames = assetNames;

        BuildPipeline.BuildAssetBundles(outPath, luaBundle, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
    }

    public static void GenerateLuaAssetBundleNames()
    {
        string[] files = Directory.GetFiles("Assets/ResourcesAB/Lua", "*.bytes", SearchOption.TopDirectoryOnly);
        if (null == files || files.Length <= 0)
            return;

        string file;
        for (int j = 0; j < files.Length; ++j)
        {
            file = files[j];

            if (".meta".Equals(Path.GetExtension(file)))
            {
                continue;
            }

            EditorUtility.DisplayProgressBar(luaBytesDir, file, (float)j / (float)files.Length);
            AssetImporter _importer = AssetImporter.GetAtPath(file);
            if (null != _importer)
            {
                _importer.SetAssetBundleNameAndVariant("lua", null);
            }
            else
            {
                Debug.LogError(file);
            }
        }

        AssetDatabase.Refresh();

        EditorUtility.ClearProgressBar();
    }

    public static void GenerateAssetBundleNames()
    {
        string luaDir = "Assets/ResourcesAB/Lua";
        string rootDir = "Assets/ResourcesAB/";
        int rootDirLen = rootDir.Length;
        string[] dirs = Directory.GetDirectories(rootDir);
        if (null == dirs || dirs.Length <= 0)
            return;

        string dir;
        string file;
        for (int i = 0; i < dirs.Length; ++i)
        {
            dir = dirs[i];
            if (Directory.Equals(dir, luaDir))
                continue;

            string[] files = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);
            if (null == files || files.Length <= 0)
                continue;

            for(int j = 0; j < files.Length; ++j)
            {
                file = files[j];
                
                if (".meta".Equals(Path.GetExtension(file)))
                {
                    continue;
                }

                EditorUtility.DisplayProgressBar(dir, file, (float)j / (float)files.Length);
                AssetImporter _importer = AssetImporter.GetAtPath(file);
                if(null != _importer)
                {
                    file = file.Remove(0, rootDirLen);
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    string fileDir = Path.GetDirectoryName(file);
                    _importer.SetAssetBundleNameAndVariant(string.Format("{0}/{1}", fileDir , fileName), null);
                }
                else
                {
                    Debug.LogError(file);
                }
            }
        }

        AssetDatabase.Refresh();

        EditorUtility.ClearProgressBar();
    }

    static void CopyLuaBytesFiles(string sourceDir, string destDir, bool appendext = true, string searchPattern = "*.lua", SearchOption option = SearchOption.AllDirectories)
    {
        if (!Directory.Exists(sourceDir))
        {
            return;
        }

        string[] files = Directory.GetFiles(sourceDir, searchPattern, option);
        int len = sourceDir.Length;

        if (sourceDir[len - 1] == '/' || sourceDir[len - 1] == '\\')
        {
            --len;
        }
        ++len;

        for (int i = 0; i < files.Length; i++)
        {
            string str = files[i].Remove(0, len);

            EditorUtility.DisplayProgressBar(sourceDir, str, (float)i / (float)files.Length);
            str = str.Replace('/', '_');
            str = str.Replace('\\', '_');
            string dest = destDir + "/" + str;
            if (appendext) dest += ".bytes";
            string dir = Path.GetDirectoryName(dest);
            Directory.CreateDirectory(dir);
            File.Copy(files[i], dest, true);
        }

        AssetDatabase.Refresh();

        EditorUtility.ClearProgressBar();
    }
}
