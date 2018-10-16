using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;

public class ScriptGenerateSetting : ScriptableObject
{
    public string uiPrefabPath;
    public string uiScriptOutPath;
    public string uiScriptTemplate;
}

public class ScriptEntry
{
    public bool selected;
    public Type types;

    public void OnGUI()
    {
        EditorGUILayout.BeginHorizontal("IN Title");
        selected = EditorGUILayout.ToggleLeft(types.Name, selected);
        EditorGUILayout.EndHorizontal();
    }
}

public class ScriptTrunk
{
    public bool selected;
    public string name;
    public List<ScriptEntry> scriptEntries = new List<ScriptEntry>();


    public void OnGUI()
    {
        EditorGUILayout.BeginHorizontal("toolbarbutton");
        selected = EditorGUILayout.Foldout(selected, name, true);
        EditorGUILayout.EndHorizontal();
        if (selected)
        {
            for (int i = 0; i < scriptEntries.Count; ++i)
            {
                scriptEntries[i].OnGUI();
            }
        }        
    }
}

public class ScriptGenerate : EditorWindow
{	    
    [MenuItem("Tools/代码生成")]
    public static EditorWindow ShowWindow()
    {
        return GetWindow<ScriptGenerate>("代码生成");
    }

    private ScriptGenerateSetting scriptGenerateSetting;

    private GameObject selectedObject;
    private List<ScriptTrunk> scriptTrunks = new List<ScriptTrunk>();
    private Vector2 scriptTrunksPos = Vector2.zero;

    private void OnGUI()
    {
        EditorGUILayout.ObjectField(scriptGenerateSetting, typeof(ScriptGenerateSetting), false);
        if (GUILayout.Button("载入配置"))
        {
            scriptGenerateSetting = AssetDatabase.LoadAssetAtPath<ScriptGenerateSetting>("Assets/EditorSetting/scriptGenerateSetting.asset");
            if (null == scriptGenerateSetting)
            {
                scriptGenerateSetting = ScriptableObject.CreateInstance<ScriptGenerateSetting>();
                scriptGenerateSetting.uiPrefabPath = "Assets/EditorSetting/";
                AssetDatabase.CreateAsset(scriptGenerateSetting, "Assets/EditorSetting/scriptGenerateSetting.asset");
            }            
        }

        if(null != scriptGenerateSetting)
        {
            scriptGenerateSetting.uiScriptTemplate = EditorGUILayout.TextArea(scriptGenerateSetting.uiScriptTemplate);
        }

        selectedObject = (GameObject)EditorGUILayout.ObjectField(selectedObject, typeof(GameObject), false);

        if (GUILayout.Button("分析"))
        {
            AnalysisGameobject(selectedObject);
        }

        OnGUIShowScriptTrunk();
    }

    private void OnGUIShowScriptTrunk()
    {
        if(scriptTrunks == null || scriptTrunks.Count <= 0)
        {
            return;
        }

        scriptTrunksPos = EditorGUILayout.BeginScrollView(scriptTrunksPos, GUILayout.Width(200), GUILayout.Height(600));
        for(int i = 0; i < scriptTrunks.Count; ++i)
        {
            scriptTrunks[i].OnGUI();
        }
        EditorGUILayout.EndScrollView();
    }

    private void AnalysisGameobject(GameObject go)
    {
        scriptTrunks.Clear();
        _AnalysisGameobject(selectedObject, true);
    }

    private void _AnalysisGameobject(GameObject go, bool isRoot)
    {
        if (go == null)
            return;

        Component[] components = go.GetComponents<Component>();
        ScriptTrunk scriptTrunk = new ScriptTrunk();
        scriptTrunks.Add(scriptTrunk);

        scriptTrunk.name = isRoot ? string.Empty : go.name;
        for(int i = 0; i < components.Length; ++i)
        {
            ScriptEntry scriptEntry = new ScriptEntry();
            scriptEntry.types = components[i].GetType();
            scriptTrunk.scriptEntries.Add(scriptEntry);
        }

        for(int i = 0; i < go.transform.childCount; ++i)
        {
            _AnalysisGameobject(go.transform.GetChild(i).gameObject, false);
        }
    }    

    private void GenScript()
    {
        int index = 0;
        StringBuilder cstemp = new StringBuilder();
        while (index < cslines.Count)
        {
            //有宏定义#Control
            if (cslines[index].Contains("#Control"))
            {

                int end = index + 1;
                StringBuilder sb = new StringBuilder();
                while (!cslines[end].Contains("#endControl"))
                {
                    sb.Append(cslines[end]);
                    end++;
                }
                for (int i = 0; i < controls.Count; i++)
                {
                    string TypeStr = controls[i][0];
                    string ControlName = controls[i][1];
                    string s = sb.ToString().Replace("${ControlName}", ControlName);
                    s = s.Replace("${type}", TypeStr);
                    cstemp.Append(s + "\n");
                }
                //结束后下一行
                index = end + 1;
            }
            else if (cslines[index].Contains("#ParseCode"))
            {
                int end = index + 1;
                StringBuilder sb = new StringBuilder();
                while (!cslines[end].Contains("#endParseCode"))
                {
                    sb.Append(cslines[end]);
                    end++;
                }
                for (int i = 0; i < controls.Count; i++)
                {
                    string TypeStr = controls[i][0];
                    string ControlName = controls[i][1];
                    string ControlPath = controls[i][2];
                    string s = sb.ToString().Replace("${ControlName}", ControlName);
                    s = s.Replace("${type}", TypeStr);
                    s = s.Replace("${ControlPath}", ControlPath + "_" + ControlName);
                    cstemp.Append(s + "\n");
                }
                //结束后下一行
                index = end + 1;
            }
            else
            {
                cstemp.Append(cslines[index] + "\n");
                index++;
            }
        }
    }
}
