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
        if (GUILayout.Button("ToLua"))
        {
            ReadCSV("E:\\GitHub/PAConfig/csv/cf_build.csv");
        }
    }

    static private string sFormat =
        @"local debug = require('base/debug')

local _M = {{
	id = 1,
	itemID = 2,
	stuff = 3,
	timeCost = 4,
}}

local _datas = {{
	{0}
}}

local _index = {{
	{1}
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
	debug.LogErrorFormat('cf_bulid not find data ' .. _id)
	return nil
end

function _M.GetAllDatas()
	return _datas
end

function _M.GetAllIndex()
	return _index
end

return _M 
";

    void ReadCSV(string filePath)
    {
        Encoding encoding = Encoding.ASCII;
        FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);

        StreamReader sr = new StreamReader(fs, encoding);

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

        sLine = sr.ReadLine();
        while (sLine != null)
        {
            sFields = sLine.Split(',');
            sbLine.Append("[");
            sbLine.Append(sFields[0]);
            sbLine.Append("] = {");            
            for (int i = 0; i < sFields.Length; ++i)
            {
                if (sType[i] == "string")
                {
                    sbLine.Append("'");
                    sbLine.Append(sFields[i]);
                    sbLine.Append("'");
                }
                else if (sType[i] == "table")
                {
                    sbLine.Append("{");
                    sbLine.Append(sFields[i]);
                    sbLine.Append("}");
                }
                else
                {
                    sbLine.Append(sFields[i]);
                }
            
                if (i < sFields.Length - 1)
                {
                    sbLine.Append(",");
                }
            }
            sbLine.Append("},");

            sb.AppendLine(sbLine.ToString());
            sbLine.Clear();

            sbIDS.Append(sFields[0]);
            sbIDS.Append(",");

            sLine = sr.ReadLine();
        }

        fs.Close();

        string outPath = Path.ChangeExtension(filePath, "lua"); 
        FileStream wfs = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
        string all = sbAll.AppendFormat(sFormat, sb.ToString(), sbIDS.ToString()).ToString();
        byte[] allB = System.Text.Encoding.ASCII.GetBytes(all);
        allB = System.Text.Encoding.Convert(Encoding.ASCII, Encoding.UTF8, allB);
        wfs.Write(allB, 0, allB.Length);

        sb.Clear();
        sbIDS.Clear();
        sbAll.Clear();
        wfs.Close();
    }
}
