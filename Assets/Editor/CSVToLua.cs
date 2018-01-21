using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LuaInterface;
using UnityEngine;
using UnityEditor;

public class CSVToLua : EditorWindow
{
    [MenuItem("Tools/CSVToLua", priority = 2050)]
    static void ShowWindow()
    {
        var window = GetWindow<CSVToLua>();
        window.titleContent = new GUIContent("CSVToLua");
        window.Show();
    }

    void OnGUI()
    {
        path = GUILayout.TextField(path);

        if (GUILayout.Button("ToLua"))
        {
            ReadCSV(path);
        }
    }

    static private string path = "E:\\GitHub/PAConfig/csv/cf_build.csv";

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

    void ReadCSV(string filePath)
    {
        Encoding encoding = Encoding.UTF8;
        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

        StreamReader sr = new StreamReader(fs, encoding);

        StringBuilder sbHeader = new StringBuilder();
        StringBuilder sbAll = new StringBuilder();
        StringBuilder sb = new StringBuilder();
        StringBuilder sbIDS = new StringBuilder();
        StringBuilder sbLine = new StringBuilder();

        string sLine = "";
        string[] sFields = null;

        string[] sTitle = null;
        string[] sVariable = null;
        string[] sType = null;
        string[] sDescription = null;

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
            sbHeader.AppendFormat(sf, sVariable[i], i + 1, sTitle[i]);
        }

        sLine = sr.ReadLine();
        while (sLine != null)
        {
            byte[] bLine = encoding.GetBytes(sLine);
            bLine = Encoding.Convert(encoding, Encoding.UTF8, bLine);
            sLine = Encoding.UTF8.GetString(bLine);

            sFields = sLine.Split(',');
            sbLine.Append("\t[");
            sbLine.Append(sFields[0]);
            sbLine.Append("] = {");
            for (int i = 0; i < sFields.Length; ++i)
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

            sLine = sr.ReadLine();
        }

        fs.Close();

        string outPath = Path.ChangeExtension(filePath, "lua");
        string fileName = Path.GetFileNameWithoutExtension(filePath);
        FileStream wfs;
        wfs = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

        string all = sbAll.AppendFormat(sFormat, sbHeader.ToString(), sb.ToString(), sbIDS.ToString(), fileName).ToString();
        byte[] allB = System.Text.Encoding.UTF8.GetBytes(all);
        //allB = System.Text.Encoding.Convert(Encoding.ASCII, Encoding.UTF8, allB);

        wfs.Position = 0;
        wfs.SetLength(0);

        wfs.Write(allB, 0, allB.Length);
        wfs.Flush();
        sb.Clear();
        sbIDS.Clear();
        sbAll.Clear();
        wfs.Close();
    }
}
