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

    public string luaInPath = string.Empty;
    public string luaOutPath = string.Empty;
    public string assetOut = string.Empty;

    public string outDirectory = string.Empty;

    void OnGUI()
    {
        //  if (EditorApplication.isCompiling)
        //    Close();

        GUILayout.BeginHorizontal();
        luaInPath = GUILayout.TextField(luaInPath);
        //if (GUILayout.Button("选择路径"))
        //{
        //    luaScriptDirectory = EditorUtility.SaveFolderPanel("Select Lua Script Folder", luaScriptDirectory, "");
        //}
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        luaOutPath = GUILayout.TextField(luaOutPath);
        //if (GUILayout.Button("选择路径"))
        //{
        //    tempDirectory = EditorUtility.SaveFolderPanel("Select Lua Script Folder", tempDirectory, "");
        //}
        GUILayout.EndHorizontal();

        if (GUILayout.Button("CopyLua"))
        {
            CopyLuaToTxt(luaInPath, luaOutPath);
        }
    }

    // Use this for initialization
    void Awake()
    {
        luaInPath = Application.dataPath + "/ScriptLua/";
        assetOut = Application.dataPath + "/StreamingAssets/";
        luaOutPath = Application.dataPath + "/ResourcesAB/Lua/";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void CopyLuaToTxt(string inPath, string outPath)
    {
        string[] files;
        if (!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }
        else
        {
            files = Directory.GetFiles(outPath, "*.txt", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; ++i)
            {
                File.Delete(files[i]);
            }
        }

        files = Directory.GetFiles(inPath, "*.lua", SearchOption.AllDirectories);

        for (int i = 0; i < files.Length; i++)
        {
            string file = files[i];
            string destFile = Path.Combine(outPath, Path.GetFileName(file));
            destFile = destFile.Replace(".lua", ".txt");

            FileUtil.CopyFileOrDirectory(file, destFile);
        }

        AssetDatabase.Refresh();
    }

    public static void BundleLuaFile(string inPath, string outPath)
    {
        string[] files = Directory.GetFiles(inPath, "*.txt", SearchOption.AllDirectories);
        for (int i = 0; i < files.Length; i++)
        {
            string _assetPath = "Assets" + files[i].Substring(Application.dataPath.Length);
            _assetPath = _assetPath.Replace("\\", "/");
            AssetImporter _importer = AssetImporter.GetAtPath(_assetPath);
            if (_importer == null)
            {
                Debug.LogError(_assetPath);
                continue;
            }
            else
            {
                _importer.assetBundleName = "lua";
            }
        }
        AssetDatabase.Refresh();
        BuildPipeline.BuildAssetBundles(outPath, BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
    }
}
