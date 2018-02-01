using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LuaInterface;
using UnityEngine;
using UnityEditor;

public class CSVToLua : EditorWindow
{
    class FileState
    {
        public string path;
        public bool isSel = false;
    }

    [MenuItem("Tools/CSVToLua", priority = 2050)]
    static void ShowWindow()
    {
        var window = GetWindow<CSVToLua>();
        window.titleContent = new GUIContent("CSVToLua");
        window.Show();
    }

    static private string sFormat =
        @"local debug = require('base/debug')

local _M = {{
{0}	
}}

local _datas = {{
{1}
}}

local _index = {{
    {2}
}}

local _datasReadOnly = readonly(_datas)
local _indexReadOnly = readonly(_index)

function _M.GetData(_id, _type)
	local _data = _M.GetDataEntire(_id)
	if _data then
		return _data[_type]
	end
	return nil
end

function _M.GetDataEntire(_id)
	local _data = _datas[_id]
	if _data then
		return _data
	end
	debug.LogErrorFormat('{3} not find data ' .. _id)
	return nil
end

function _M.GetAllDatas()
	return _datas
end

function _M.GetAllIndex()
	return _index
end

return _M";

    private string srcPath = "";
    private string desPath = "";

    private string luaPath = "/Lua/csv";
    private string csvPath = "/Csv";

    FileState[] files;

    void OnEnable()
    {
        srcPath = Directory.GetParent(Application.dataPath) + csvPath;
        desPath = Application.dataPath + luaPath;
        RefreshCSVList();
    }

    void OnGUI()
    {
        GUILayout.Label(srcPath);
        GUILayout.Label(desPath);

        if (GUILayout.Button("刷新文件列表"))
        {
            RefreshCSVList();
        }

        if (files == null)
        {
            return;
        }

        for (int i = 0; i < files.Length; ++i)
        {
            files[i].isSel = EditorGUILayout.ToggleLeft(files[i].path, files[i].isSel);
        }

        if (GUILayout.Button("生成Lua文件"))
        {
            for (int i = 0; i < files.Length; ++i)
            {
                if (files[i].isSel)
                {
                    EditorUtility.DisplayProgressBar("生成lua文件", files[i].path, 0);
                    ReadCSV(files[i].path, desPath);
                    EditorUtility.DisplayProgressBar("生成lua文件", files[i].path, 1);
                }
            }
        }
        EditorUtility.ClearProgressBar();
    }

    void RefreshCSVList()
    {
        string[] fs = Directory.GetFiles(srcPath, "*.csv");
        if (fs != null)
        {
            files = new FileState[fs.Length];
        }

        if (files != null)
        {
            for (int i = 0; i < files.Length; ++i)
            {
                files[i] = new FileState();
                files[i].path = fs[i];
            }
        }
    }

    void ReadCSV(string srcPath, string desPath)
    {
        FileStream rfs = new FileStream(srcPath, FileMode.Open, FileAccess.Read);
        Encoding srcEncoding = FileEncoding.GetType(rfs);
        rfs.Close();

        StreamReader sr = new StreamReader(srcPath, srcEncoding);

        StringBuilder sbHeader = new StringBuilder();
        StringBuilder sbAll = new StringBuilder();
        StringBuilder sb = new StringBuilder();
        StringBuilder sbIDS = new StringBuilder();
        StringBuilder sbLine = new StringBuilder();

        string sLine = null;
        string[] sTitle = null;
        string[] sVariable = null;
        string[] sType = null;
        string[] sDescription = null;
        string[] sFields = null;
        int columnCount = 0;

        sLine = sr.ReadLine();
        sTitle = sLine.Split(',');

        sLine = sr.ReadLine();
        sVariable = sLine.Split(',');

        sLine = sr.ReadLine();
        sType = sLine.Split(',');

        sLine = sr.ReadLine();
        sDescription = sLine.Split(',');

        string sf = "\t{0} = {1}, -- {2}\n";
        for (int i = 0; i < sVariable.Length; ++i)
        {
            if (!string.IsNullOrEmpty(sVariable[i]))
            {
                sbHeader.AppendFormat(sf, sVariable[i], i + 1, sTitle[i]);
                ++columnCount;
            }
        }

        while (true)
        {
            sLine = sr.ReadLine();
            if (null == sLine)
                break; ;

            if (!Encoding.UTF8.Equals(srcEncoding))
            {
                byte[] bLine = srcEncoding.GetBytes(sLine);
                bLine = Encoding.Convert(srcEncoding, Encoding.UTF8, bLine);
                sLine = Encoding.UTF8.GetString(bLine);
            }

            sFields = sLine.Split(',');
            if (string.IsNullOrEmpty(sFields[0]))
            {
                continue;
            }

            sbLine.Append("\t[");
            sbLine.Append(sFields[0]);
            sbLine.Append("] = {");
            for (int i = 0; i < columnCount; ++i)
            {
                if (string.IsNullOrEmpty(sFields[i]))
                {
                    sbLine.Append("false");
                }
                else
                {
                    if (sType[i] == "string")
                    {
                        sbLine.Append("'");
                        sbLine.Append(sFields[i]);
                        sbLine.Append("'");
                    }
                    else if (sType[i] == "table")
                    {
                        string s = sFields[i].Replace("|", ", ");

                        sbLine.Append("{");
                        sbLine.Append(s);
                        sbLine.Append("}");
                    }
                    else
                    {
                        sbLine.Append(sFields[i]);
                    }
                }

                if (i < sFields.Length - 1)
                {
                    sbLine.Append(", ");
                }
            }
            sbLine.Append("},");

            sb.AppendLine(sbLine.ToString());
            sbLine.Clear();

            sbIDS.Append(sFields[0]);
            sbIDS.Append(", ");
        }

        sr.Close();

        string fileName = Path.GetFileNameWithoutExtension(srcPath);
        string fileNameEx = string.Format("{0}.{1}", fileName, "lua");
        string outPath = Path.Combine(desPath, fileNameEx);

        FileStream wfs = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

        string all = sbAll.AppendFormat(sFormat, sbHeader.ToString(), sb.ToString(), sbIDS.ToString(), fileName).ToString();

        byte[] desBytes = System.Text.Encoding.UTF8.GetBytes(all);

        wfs.Position = 0;
        wfs.SetLength(0);

        wfs.Write(desBytes, 0, desBytes.Length);
        wfs.Flush();
        sb.Clear();
        sbIDS.Clear();
        sbAll.Clear();
        wfs.Close();
    }
}
