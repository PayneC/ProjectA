using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;
using System.IO;

public class ScriptGenerateSetting : ScriptableObject
{
    public string uiPrefabPath;
    public string uiScriptOutPath;
    public string uiScriptTemplate;
}

public class ScriptEntry
{
    public bool isSelected;
    public string type;
}

public class ScriptTrunk
{
    public int depth;
    public bool isRoot;
    public bool isSelected;
    public string name;
    public string path;
    public List<ScriptEntry> scriptEntries = new List<ScriptEntry>();
    public List<ScriptTrunk> scriptTrunks = new List<ScriptTrunk>();

    public void OnGUI()
    {
        EditorGUILayout.BeginHorizontal("SelectionRect");
        GUILayout.Label("", GUILayout.Width(depth * 20));
        isSelected = EditorGUILayout.Foldout(isSelected, name, true);
        EditorGUILayout.EndHorizontal();

        if (isSelected)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("", GUILayout.Width(depth * 20));
            for (int i = 0; i < scriptEntries.Count; ++i)
            {                
                scriptEntries[i].isSelected = EditorGUILayout.ToggleLeft(scriptEntries[i].type, scriptEntries[i].isSelected, GUILayout.Width(80));             
            }
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < scriptTrunks.Count; ++i)
            {
                scriptTrunks[i].OnGUI();
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
    
    private ScriptTrunk scriptTrunkRoot = new ScriptTrunk();
    private string scriptCode = string.Empty;

    private Vector2 scriptTrunksPos = Vector2.zero;
    private Vector2 templateCodePos = Vector2.zero;
    private Vector2 scriptCodePos = Vector2.zero;

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
            templateCodePos = EditorGUILayout.BeginScrollView(templateCodePos, GUILayout.Width(600), GUILayout.Height(300));
            scriptGenerateSetting.uiScriptTemplate = EditorGUILayout.TextArea(scriptGenerateSetting.uiScriptTemplate);
            EditorGUILayout.EndScrollView();
        }

        if(!string.IsNullOrEmpty(scriptCode))
        {
            scriptCodePos = EditorGUILayout.BeginScrollView(scriptCodePos, GUILayout.Width(600), GUILayout.Height(300));
            EditorGUILayout.TextArea(scriptCode);
            EditorGUILayout.EndScrollView();
        }        

        selectedObject = (GameObject)EditorGUILayout.ObjectField(selectedObject, typeof(GameObject), false);

        if (GUILayout.Button("分析"))
        {
            AnalysisGameobject(selectedObject);
        }

        if (GUILayout.Button("生成代码"))
        {
            GenScript();
        }

        OnGUIShowScriptTrunk();
    }

    private void OnGUIShowScriptTrunk()
    {
        if(scriptTrunkRoot == null)
        {
            return;
        }

        scriptTrunksPos = EditorGUILayout.BeginScrollView(scriptTrunksPos, GUILayout.Width(600), GUILayout.Height(600));
        scriptTrunkRoot.OnGUI();
        EditorGUILayout.EndScrollView();
    }

    private void AnalysisGameobject(GameObject go)
    {
        if (go == null)
            return;
                
        scriptTrunkRoot = new ScriptTrunk();
        scriptTrunkRoot.name = go.name;
        scriptTrunkRoot.path = string.Empty;
        scriptTrunkRoot.isRoot = true;
        scriptTrunkRoot.depth = 0;

        ScriptEntry scriptEntry = new ScriptEntry();        
        scriptTrunkRoot.scriptEntries.Add(scriptEntry);
        scriptEntry.type = "GameObject";

        Component[] components = go.GetComponents<Component>();
        for (int i = 0; i < components.Length; ++i)
        {
            scriptEntry = new ScriptEntry();
            scriptTrunkRoot.scriptEntries.Add(scriptEntry);
            scriptEntry.type = components[i].GetType().Name;            
        }

        _AnalysisGameobject(selectedObject, scriptTrunkRoot);
    }

    private void _AnalysisGameobject(GameObject go, ScriptTrunk parent)
    {
        if (go == null)
            return;
        
        ScriptTrunk scriptTrunk = new ScriptTrunk();
        parent.scriptTrunks.Add(scriptTrunk);
        scriptTrunk.name = go.name;        
        scriptTrunk.path = parent.isRoot ? go.name : string.Format("{0}/{1}", parent.path, go.name);
        scriptTrunk.depth = parent.depth + 1;

        ScriptEntry scriptEntry = new ScriptEntry();
        scriptTrunk.scriptEntries.Add(scriptEntry);
        scriptEntry.type = "GameObject";

        Component[] components = go.GetComponents<Component>();
        for (int i = 0; i < components.Length; ++i)
        {
            scriptEntry = new ScriptEntry();
            scriptTrunk.scriptEntries.Add(scriptEntry);
            scriptEntry.type = components[i].GetType().Name;            
        }

        for(int i = 0; i < go.transform.childCount; ++i)
        {
            _AnalysisGameobject(go.transform.GetChild(i).gameObject, scriptTrunk);
        }
    }    

    private void GenScript()
    {
        /*
        StringBuilder cstemp = new StringBuilder();
        StringReader stringReader = new StringReader(scriptGenerateSetting.uiScriptTemplate);
        string line;
        while ((line = stringReader.ReadLine()) != null)
        {
            //有宏定义#Control
            if (line.Contains("#Control"))
            {                
                StringBuilder sb = new StringBuilder();
                while ((line = stringReader.ReadLine()) != null && !line.Contains("#endControl"))
                {
                    sb.Append(line);
                }

                for (int i = 0; i < scriptTrunk.Count; i++)
                {
                    if(!scriptTrunks[i].isSelected)
                    {
                        continue;
                    }
                    List<ScriptEntry> scriptEntries = scriptTrunks[i].scriptEntries;                    
                    for (int j = 0; j < scriptEntries.Count; ++j)
                    {
                        if (!scriptEntries[j].isSelected)
                        {
                            continue;
                        }

                        string TypeStr = scriptEntries[j].type;
                        string ControlName;
                        if (!TypeStr.Equals("GameObject"))
                        {
                            ControlName = string.Format("{0}_{1}", scriptTrunks[i].name, TypeStr);
                        }
                        else
                        {
                            ControlName = scriptTrunks[i].name;
                        }

                        string s = sb.ToString().Replace("${ControlName}", ControlName);
                        s = s.Replace("${type}", TypeStr);
                        cstemp.Append(s + "\n");
                    }                                        
                }                
            }
            else if (line.Contains("#ParseCode"))
            {                
                StringBuilder sb = new StringBuilder();
                while ((line = stringReader.ReadLine()) != null && !line.Contains("#endParseCode"))
                {
                    sb.Append(line);                    
                }

                for (int i = 0; i < scriptTrunks.Count; i++)
                {
                    if (!scriptTrunks[i].isSelected)
                    {
                        continue;
                    }
                    List<ScriptEntry> scriptEntries = scriptTrunks[i].scriptEntries;
                    for (int j = 0; j < scriptEntries.Count; ++j)
                    {
                        if (!scriptEntries[j].isSelected)
                        {
                            continue;
                        }

                        string TypeStr = scriptEntries[j].type;
                        string ControlName;
                        if (!TypeStr.Equals("GameObject"))
                        {
                            ControlName = string.Format("{0}_{1}", scriptTrunks[i].name, TypeStr);
                        }
                        else
                        {
                            ControlName = scriptTrunks[i].name;
                        }
                        string ControlPath = scriptTrunks[i].path;

                        string s = sb.ToString().Replace("${ControlName}", ControlName);
                        s = s.Replace("${type}", TypeStr);
                        s = s.Replace("${ControlPath}", ControlPath);
                        cstemp.Append(s + "\n");
                    }
                }
            }
            else
            {
                cstemp.Append(line + "\n");                
            }

            scriptCode = cstemp.ToString();
        }
        */
    }
}
