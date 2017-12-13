using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;

#pragma warning disable 0649,0414,0618,0219,0162,0168

public class UIEditor : EditorWindow
{
    [MenuItem("Tools/UIEditor")]
    static public void CreateAtlas()
    {
        if (Application.isPlaying || EditorApplication.isPlaying || EditorApplication.isPaused)
        {
            EditorUtility.DisplayDialog("错误", "游戏正在运行或者暂定，请不要操作！", "确定");
            return;
        }

        if (EditorApplication.isCompiling)
        {
            EditorUtility.DisplayDialog("错误", "游戏脚本正在编译，请不要操作！", "确定");
            return;
        }
        EditorWindow window = EditorWindow.GetWindowWithRect<UIEditor>(new Rect(100f, 100f, 600, 800f), true, "UIEditor", true);
    }

    [MenuItem("Assets/Generate To Assets")]
    static public void GenerateAssets()
    {
        if (Application.isPlaying || EditorApplication.isPlaying || EditorApplication.isPaused)
        {
            EditorUtility.DisplayDialog("错误", "游戏正在运行或者暂定，请不要操作！", "确定");
            return;
        }

        if (EditorApplication.isCompiling)
        {
            EditorUtility.DisplayDialog("错误", "游戏脚本正在编译，请不要操作！", "确定");
            return;
        }

        string _path = AssetDatabase.GetAssetPath(Selection.activeObject);
        string _name = Path.GetFileName(_path);
        string _directory = Path.GetDirectoryName(_path) + "/";
        if (_uiPrefabPath.Equals(_directory))
        {
            GenPrefab(_name);
            EditorUtility.DisplayDialog("UIEditor", "预制生成完成", "确定");
        }
        else
        {
            EditorUtility.DisplayDialog("UIEditor", string.Format("错误路径{0}\n只能生成{1}路径下预制体", _directory, _uiPrefabPath), "确定");
        }
    }

    public class SpriteInfo
    {
        public string sName = string.Empty;
        public string sPath = string.Empty;
        public int refCount = 0;
    }

    public class AtlasInfo
    {
        public bool isSel = false;
        public bool isShowDetails = false;
        public string sName = string.Empty;
        public List<SpriteInfo> spriteInfos = new List<SpriteInfo>();
        public List<TextureImporter> imps = new List<TextureImporter>();
    }

    public class UIPrefabInfo
    {
        public bool isSel = false;
        public bool isShowDetail = false;
        public string prefabName = null;
        public string prefabPath = null;
        public Dictionary<string, List<string>> atlass = new Dictionary<string, List<string>>();
        public List<string> textures = new List<string>();
        public List<string> others = new List<string>();
        public List<string> noAtlas = new List<string>();
        public List<string> fonts = new List<string>();
    }

    static private string _jpg = ".jpg";
    static private string _png = ".png";
    static private string _tga = ".tga";
    static private string _ttf = ".ttf";

    static private bool AllowTightWhenTagged = true;
    static private bool AllowRotationFlipping = false;
    static private string TagPrefix = "[TIGHT]";
    static private string invalidName = "(unnamed)";

    static private string _uiAtlasOutPath = "Assets/ResourcesAB/Atlas/";
    static private string _uiTextureOutPath = "Assets/AssetEditor/Atlas/";
    static private string _uiAssetsPath = "Assets/AssetEditor/Sprites/";
    static private string _uiPrefabPath = "Assets/AssetEditor/UIPrefabs/";
    static private string _uiPrefabOutPath = "Assets/ResourcesAB/UI/";

    private string _uiAssetsFullPath = null;    
    private string _uiTextureOutFullPath = null;    
    private string _uiAtlasOutFullPath = null;
    private string _uiPrefabFullPath = null;
    private string _uiPrefabOutFullPath = null;

    private Dictionary<string, AtlasInfo> _atlasInfoDic = new Dictionary<string, AtlasInfo>();
    private List<AtlasInfo> _atlasInfoList = new List<AtlasInfo>();

    private UIPrefabInfo[] _prefabInfos = null;

    private string[] _excessPrefab = null;
    private string[] _excessAtlas = null;

    private bool _hasAtlasData = false;
    private bool _hasPrefabData = false;
    private Vector2 _scrAtlas = Vector2.zero;
    private Vector2 _scrPrefab = Vector2.zero;

    private string[] _titles =
    {
        "精简模式",
        "预制",
        "图集",        
    };

    private int _currentTitle = 0;

    public void OnEnable()
    {
        _uiAssetsFullPath = Path.GetFullPath(_uiAssetsPath);
        _uiTextureOutFullPath = Path.GetFullPath(_uiTextureOutPath);
        _uiAtlasOutFullPath = Path.GetFullPath(_uiAtlasOutPath);

        _uiPrefabFullPath = Path.GetFullPath(_uiPrefabPath);
        _uiPrefabOutFullPath = Path.GetFullPath(_uiPrefabOutPath);  
    }

    public void OnDisable()
    {
        _refInfos.Clear();
        _atlasInfoDic.Clear();
        _atlasInfoList.Clear();

        _prefabInfos = null;

        Resources.UnloadUnusedAssets();
        EditorUtility.UnloadUnusedAssets();
        System.GC.Collect();
        UnityEditor.AssetDatabase.Refresh();
    }

    public void Update()
    {
        if (Application.isPlaying || EditorApplication.isPlaying || EditorApplication.isPaused || EditorApplication.isCompiling)
        {
            Close();
            return;
        }
    }

    public void OnGUI()
    {
        _currentTitle = GUILayout.SelectionGrid(_currentTitle, _titles, _titles.Length);
        GUILayout.Space(10);
        switch (_currentTitle)
        {
            case 0:
                ShowEasy();
                break;
            case 1:
                ShowPrefabList();
                break;
            case 2:
                ShowAtlasList();
                break;
        }
        GUILayout.Space(10);
    }

    private void ShowEasy()
    {
        GUILayout.Label("小图资源路径：" + _uiAssetsFullPath);
        GUILayout.Label("图集纹理输出路径：" + _uiTextureOutFullPath);
        GUILayout.Label("图集信息输出路径：" + _uiAtlasOutFullPath);
        GUILayout.Label("预制体制作路径：" + _uiPrefabFullPath);
        GUILayout.Label("预制体输出路径：" + _uiPrefabOutFullPath);

        if (GUILayout.Button("一键生成(全部的预制和图集重新生成)"))
        {
            if (!_hasPrefabData)
            {
                ParsePrefab();
            }
            if (!_hasAtlasData)
            {
                ParseAtlas();
            }
            GenAllPrefab();
            ClearPrefab();
            GenAllAtlas();
            ClearAtlas();
            EditorUtility.DisplayDialog("UIEditor", "预制图集生成完成", "ok");
        }

        if (GUILayout.Button("图集名转小写"))
            PackTagToLower();
    }

    private void ParsePrefab()
    {
        string[] files = Directory.GetFiles(_uiPrefabFullPath, "*.prefab", SearchOption.AllDirectories);
        _prefabInfos = new UIPrefabInfo[files.Length];
        for (int i = 0; i < files.Length; ++i)
        {
            EditorUtility.DisplayProgressBar("UI预制解析", files[i], (float)i / (float)files.Length);
            string name = Path.GetFileName(files[i]);
            _prefabInfos[i] = GetPrefabAtlasInfo(name);
        }
        _hasPrefabData = true;
        EditorUtility.ClearProgressBar();
    }

    static private void GenPrefab(string name)
    {
        EditorUtility.DisplayProgressBar("生成UI预制", name, 0.8f);

        string inPath = _uiPrefabPath + name;
        string outPath = _uiPrefabOutPath + name;

        //meta（Before GenPrefab）
        bool metaExist = false;
        string metaPath = Application.dataPath.Replace("Assets", outPath + ".meta");
        byte[] metaContent = null;
        if (File.Exists(metaPath))
        {
            metaExist = true;
            metaContent = File.ReadAllBytes(metaPath);
        }

        Object oj = AssetDatabase.LoadAssetAtPath(inPath, typeof(Object));
        Object ojC = Object.Instantiate(oj);
        GameObject go = ojC as GameObject;

        RewriteUIPrefab(go.transform);
        PrefabUtility.CreatePrefab(outPath, go);

        DestroyImmediate(go);

        EditorUtility.ClearProgressBar();

        //meta（After GenPrefab）
        if (metaExist)
        {
            File.WriteAllBytes(metaPath, metaContent);
        }
    }

    private void GenAllPrefab()
    {
        if (_prefabInfos == null || _prefabInfos.Length <= 0)
            return;

        for (int i = 0; i < _prefabInfos.Length; ++i)
        {
            GenPrefab(_prefabInfos[i].prefabName);
        }
        AssetDatabase.Refresh();
    }

    private void ClearPrefab()
    {
        //Clear ui prefab
        string[] names = Directory.GetFiles(_uiPrefabOutFullPath, "*.prefab", SearchOption.AllDirectories);
        string name = null;
        for (int i = 0; i < names.Length; ++i)
        {
            bool isUseful = false;
            name = Path.GetFileName(names[i]);
            EditorUtility.DisplayProgressBar("清理Prefab", name, (float)i / (float)names.Length);
            for (int j = 0; j < _prefabInfos.Length; ++j)
            {
                if (_prefabInfos[j].prefabName.Equals(name))
                {
                    isUseful = true;
                    break;
                }
            }

            if (!isUseful)
                File.Delete(names[i]);
        }
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
    }

    private void ShowPrefabList()
    {        
        if (!_hasPrefabData)
        {
            ParsePrefab();
        }

        if (_prefabInfos == null || _prefabInfos.Length <= 0)
            return;

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("预制体", GUILayout.Width(200));
        EditorGUILayout.LabelField("图集数", GUILayout.Width(50));
        EditorGUILayout.LabelField("无图集数", GUILayout.Width(50));
        EditorGUILayout.LabelField("纹理数", GUILayout.Width(50));
        EditorGUILayout.LabelField("字集数", GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();

        _scrAtlas = EditorGUILayout.BeginScrollView(_scrAtlas);

        for (int i = 0; i < _prefabInfos.Length; ++i)
        {
            ShowPrefabInfo(_prefabInfos[i]);
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("全选"))
        {
            bool isAllTrue = true;
            for (int i = 0; i < _prefabInfos.Length; ++i)
            {
                if (_prefabInfos[i].isSel == false)
                {
                    isAllTrue = false;
                    break;
                }
            }

            for (int i = 0; i < _prefabInfos.Length; ++i)
            {
                _prefabInfos[i].isSel = !isAllTrue;
            }
        }

        if (GUILayout.Button("生成预制"))
        {
            List<TextureImporter> ot = null;
            for (int i = 0; i < _prefabInfos.Length; ++i)
            {
                if (_prefabInfos[i].isSel)
                    GenPrefab(_prefabInfos[i].prefabName);
            }
            EditorUtility.DisplayDialog("UIEditor", "预制生成完成", "ok");
        }

        if (GUILayout.Button("清理多余prefab"))
        {
            ClearPrefab();
            EditorUtility.DisplayDialog("UIEditor", "预制清理完成", "ok");
        }

        if (GUILayout.Button("分析预制"))
        {
            ParsePrefab();
            EditorUtility.DisplayDialog("UIEditor", "预制分析完成", "ok");
        }

        EditorGUILayout.EndHorizontal();
    }

    private void ParseAtlas2()
    {
        CalBeReferences();

        for (int i = 0; i < _atlasInfoList.Count; ++i)
        {
            AtlasInfo aInfo = _atlasInfoList[i];
            for (int j = 0; j < aInfo.spriteInfos.Count; ++j)
            {
                SpriteInfo sInfo = aInfo.spriteInfos[j];
                sInfo.refCount = GetRefCount(sInfo.sPath);
            }
        }
    }

    private void ParseAtlas()
    {        
        _atlasInfoDic.Clear();
        _atlasInfoList.Clear();

        List<string> tmp = new List<string>();
        
        string[] files = Directory.GetFiles(_uiAssetsFullPath, "*.png", SearchOption.AllDirectories);

        for (int i = 0; i < files.Length; ++i)
        {
            EditorUtility.DisplayProgressBar("图集统计", files[i], (float)i / (float)files.Length);

            string path = files[i].Remove(0, Directory.GetCurrentDirectory().Length + 1);      
                 
            TextureImporter imp = AssetImporter.GetAtPath(path) as TextureImporter;

            string atlasName = ParseAtlasName(imp.spritePackingTag);

            AtlasInfo _atlasInfo = null;

            if (!_atlasInfoDic.TryGetValue(atlasName, out _atlasInfo))
            {
                _atlasInfo = new AtlasInfo();
                _atlasInfo.isSel = false;
                _atlasInfo.isShowDetails = false;
                _atlasInfoDic[atlasName] = _atlasInfo;
                _atlasInfoList.Add(_atlasInfo);
                _atlasInfo.sName = atlasName;
            }
            SpriteInfo spriteInfo = new SpriteInfo();
            _atlasInfo.spriteInfos.Add(spriteInfo);
            _atlasInfo.imps.Add(imp);
            spriteInfo.sPath = path;
            spriteInfo.sName = Path.GetFileName(spriteInfo.sPath);            
        }

        _hasAtlasData = true;
        EditorUtility.ClearProgressBar();
    }

    private void GenAtlas(string name, List<TextureImporter> imps)
    {
        if (string.IsNullOrEmpty(name) || invalidName.Equals(name))
            return;

        if (imps == null || imps.Count <= 0)
            return;

        EditorUtility.DisplayProgressBar("生成UI图集", name, 0.8f);

        AtlasPackTool.PackTexture(imps, name, _uiAtlasOutPath, _uiTextureOutPath);
        EditorUtility.ClearProgressBar();
    }

    private void GenAllAtlas()
    {
        for (int i = 0; i < _atlasInfoList.Count; ++i)
        {
            AtlasInfo _atlasInfo = _atlasInfoList[i];
            GenAtlas(_atlasInfo.sName, _atlasInfo.imps);
        }

        AssetDatabase.Refresh();
    }

    private void ClearAtlas()
    {
        //Clear Atlas prefab
        string[] names = Directory.GetFiles(_uiAtlasOutFullPath, "*.prefab", SearchOption.AllDirectories);
        string name = null;
        for (int i = 0; i < names.Length; ++i)
        {
            bool isUseful = false;
            name = Path.GetFileNameWithoutExtension(names[i]);

            EditorUtility.DisplayProgressBar("清理Atlas", name, (float)i * 0.5f / (float)names.Length);

            for (int j = 0; j < _atlasInfoList.Count; ++j)
            {
                AtlasInfo _atlasInfo = _atlasInfoList[j];
                if (_atlasInfo.sName.Equals(name))
                {
                    isUseful = true;
                    break;
                }
            }

            if (!isUseful)
                File.Delete(names[i]);
        }
        //Clear Atlas texture
        names = Directory.GetFiles(_uiTextureOutFullPath, "*.png", SearchOption.AllDirectories);
        for (int i = 0; i < names.Length; ++i)
        {
            bool isUseful = false;
            name = Path.GetFileNameWithoutExtension(names[i]);
            EditorUtility.DisplayProgressBar("清理Atlas", name, (float)i * 0.5f / (float)names.Length + 0.5f);
            for (int j = 0; j < _atlasInfoList.Count; ++j)
            {
                AtlasInfo _atlasInfo = _atlasInfoList[j];
                if (_atlasInfo.sName.Equals(name))
                {
                    isUseful = true;
                    break;
                }
            }

            if (!isUseful)
                File.Delete(names[i]);
        }
        AssetDatabase.Refresh();
        EditorUtility.ClearProgressBar();
    }

    private void ShowAtlasList()
    {
        if (!_hasAtlasData)
        {
            ParseAtlas();
        }

        AtlasInfo _atlasInfo = null;
        if (_atlasInfoList != null && _atlasInfoList.Count > 0)
        {
            _scrAtlas = EditorGUILayout.BeginScrollView(_scrAtlas);


            for (int i = 0; i < _atlasInfoList.Count; ++i)
            {
                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                _atlasInfo = _atlasInfoList[i];
                _atlasInfo.isSel = GUILayout.Toggle(_atlasInfo.isSel, _atlasInfo.sName, GUILayout.Width(400));
                _atlasInfo.isShowDetails = EditorGUILayout.Foldout(_atlasInfo.isShowDetails, "显示详情");
                EditorGUILayout.EndHorizontal();
                if (_atlasInfo.isShowDetails)
                {
                    for (int j = 0; j < _atlasInfo.spriteInfos.Count; ++j)
                    {
                        ShowSpriteInfo(_atlasInfo.spriteInfos[j]);
                    }
                }
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("全选"))
            {
                bool isAllTrue = true;
                for (int i = 0; i < _atlasInfoList.Count; ++i)
                {
                    if (_atlasInfoList[i].isSel == false)
                    {
                        isAllTrue = false;
                        break;
                    }
                }

                for (int i = 0; i < _atlasInfoList.Count; ++i)
                {
                    _atlasInfoList[i].isSel = !isAllTrue;
                }
            }

            if (GUILayout.Button("生成图集"))
            {
                for (int i = 0; i < _atlasInfoList.Count; ++i)
                {
                    _atlasInfo = _atlasInfoList[i];
                    if (_atlasInfo.isSel)
                        GenAtlas(_atlasInfo.sName, _atlasInfo.imps);
                }
                EditorUtility.DisplayDialog("UIEditor", "图集生成完成", "ok");
            }

            if (GUILayout.Button("清理多余Atlas"))
            {
                ClearAtlas();
                EditorUtility.DisplayDialog("UIEditor", "图集清理完成", "ok");
            }

            if (GUILayout.Button("图集分析"))
            {
                ParseAtlas2();
                EditorUtility.DisplayDialog("UIEditor", "图集分析完成", "ok");
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("统计图集"))
        {
            ParseAtlas();
            EditorUtility.DisplayDialog("UIEditor", "图集统计完成", "ok");
        }

        EditorGUILayout.EndHorizontal();
    }

    private void ShowSpriteInfo(SpriteInfo _info)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(_info.sName, GUILayout.Width(200));
        if (_info.refCount <= 0)
        {
            GUI.color = Color.red;
            EditorGUILayout.LabelField(_info.refCount.ToString(), GUILayout.Width(50));
            GUI.color = Color.white;
        }
        else
        {
            EditorGUILayout.LabelField(_info.refCount.ToString(), GUILayout.Width(50));
        }
        if (GUILayout.Button("定位", GUILayout.Width(50)))
        {
            Object _object = AssetDatabase.LoadAssetAtPath(_info.sPath, typeof(Object));
            EditorGUIUtility.PingObject(_object);
        }
        EditorGUILayout.EndHorizontal();
    }

    protected bool IsTagPrefixed(string packingTag)
    {
        packingTag = packingTag.Trim();
        if (packingTag.Length < TagPrefix.Length)
            return false;
        return (packingTag.Substring(0, TagPrefix.Length) == TagPrefix);
    }

    private string ParseAtlasName(string packingTag)
    {
        string name = packingTag.Trim();
        if (IsTagPrefixed(name))
            name = name.Substring(TagPrefix.Length).Trim();
        return (name.Length == 0) ? invalidName : name;
    }

    private SpritePackingMode GetPackingMode(string packingTag, SpriteMeshType meshType)
    {
        if (meshType == SpriteMeshType.Tight)
            if (IsTagPrefixed(packingTag) == AllowTightWhenTagged)
                return SpritePackingMode.Tight;
        return SpritePackingMode.Rectangle;
    }

    static private void RewriteUIPrefab(Transform node)
    {
        ImageEx _uisprite = node.GetComponent<ImageEx>();
        if (_uisprite != null)
        {
            if (_uisprite.sprite != null)
            {
                string path = AssetDatabase.GetAssetPath(_uisprite.sprite);
                TextureImporter texImp = AssetImporter.GetAtPath(path) as TextureImporter;

                if (texImp != null)
                {
                    _uisprite.SetInfo(texImp.spritePackingTag, _uisprite.sprite.name);
                }
                else
                {
                    _uisprite.SetInfo(null, null);
                }
                _uisprite.sprite = null;
            }
            else
            {
                _uisprite.SetInfo(null, null);
            }
        }

        RawImageEx _uitexture = node.GetComponent<RawImageEx>();
        if (_uitexture != null)
        {
            if (_uitexture.texture != null)
            {
                _uitexture.SetInfo(_uitexture.texture.name);
                _uitexture.texture = null;
            }
            else
            {
                _uitexture.SetInfo(null);
            }
        }
        ButtonEx _uibutton = node.GetComponent<ButtonEx>();
        if (_uibutton != null)
        {
            if (_uibutton.spriteState.highlightedSprite != null)
            {
                string path = AssetDatabase.GetAssetPath(_uibutton.spriteState.highlightedSprite);
                TextureImporter texImp = AssetImporter.GetAtPath(path) as TextureImporter;
                if (texImp != null)
                {
                    _uibutton.SetButtonInfo(1, texImp.spritePackingTag, _uibutton.spriteState.highlightedSprite.name);
                }
                else
                {
                    _uibutton.SetButtonInfo(1, null, null);
                }
            }
            else
            {
                _uibutton.SetButtonInfo(1, null, null);
            }

            if (_uibutton.spriteState.pressedSprite != null)
            {
                string path = AssetDatabase.GetAssetPath(_uibutton.spriteState.pressedSprite);
                TextureImporter texImp = AssetImporter.GetAtPath(path) as TextureImporter;
                if (texImp != null)
                {
                    _uibutton.SetButtonInfo(2, texImp.spritePackingTag, _uibutton.spriteState.pressedSprite.name);
                }
                else
                {
                    _uibutton.SetButtonInfo(2, null, null);
                }
            }
            else
            {
                _uibutton.SetButtonInfo(2, null, null);
            }

            if (_uibutton.spriteState.disabledSprite != null)
            {
                string path = AssetDatabase.GetAssetPath(_uibutton.spriteState.disabledSprite);
                TextureImporter texImp = AssetImporter.GetAtPath(path) as TextureImporter;
                if (texImp != null)
                {
                    _uibutton.SetButtonInfo(3, texImp.spritePackingTag, _uibutton.spriteState.disabledSprite.name);
                }
                else
                {
                    _uibutton.SetButtonInfo(3, null, null);
                }
            }
            else
            {
                _uibutton.SetButtonInfo(3, null, null);
            }

            _uibutton.spriteState = new SpriteState();
        }

        TextEx _uitext = node.GetComponent<TextEx>();
        if (_uitext != null)
        {
            if (_uitext.font != null)
            {
                if (_uitext.font.name.Equals("Arial"))
                {
                    _uitext.SetInfo("DFYuanW7");
                }
                else
                {
                    _uitext.SetInfo(_uitext.font.name);
                }
                _uitext.font = null;
            }
            else
            {
                _uitext.SetInfo(null);
            }
        }
        for (int i = 0; i < node.childCount; ++i)
        {
            RewriteUIPrefab(node.GetChild(i));
        }
    }

    private UIPrefabInfo GetPrefabAtlasInfo(string name)
    {
        UIPrefabInfo info = new UIPrefabInfo();

        info.prefabName = name;
        info.prefabPath = _uiPrefabPath + name;

        string[] _referenceList = AssetDatabase.GetDependencies(info.prefabPath, true);

        for (int i = 0; i < _referenceList.Length; ++i)
        {
            string _ext = Path.GetExtension(_referenceList[i]);
            if (_png.Equals(_ext))
            {
                TextureImporter imp = AssetImporter.GetAtPath(_referenceList[i]) as TextureImporter;
                string packTag = ParseAtlasName(imp.spritePackingTag);

                if (invalidName.Equals(packTag) && !info.noAtlas.Contains(_referenceList[i]))
                {
                    info.noAtlas.Add(_referenceList[i]);
                }
                else
                {
                    if (!info.atlass.ContainsKey(packTag))
                    {
                        info.atlass[packTag] = new List<string>();
                    }

                    info.atlass[packTag].Add(_referenceList[i]);
                }
            }
            else if (_jpg.Equals(_ext) || _tga.Equals(_ext))
            {
                if (!info.textures.Contains(_referenceList[i]))
                {
                    info.textures.Add(_referenceList[i]);
                }
            }
            else if (_ttf.Equals(_ext))
            {
                if (!info.fonts.Contains(_referenceList[i]))
                {
                    info.fonts.Add(_referenceList[i]);
                }
            }
            else
            {
                if (!info.others.Contains(_referenceList[i]))
                {
                    info.others.Add(_referenceList[i]);
                }
            }
        }
        return info;
    }

    private void ShowPrefabInfo(UIPrefabInfo info)
    {
        EditorGUILayout.BeginHorizontal(GUI.skin.box);
        info.isSel = EditorGUILayout.ToggleLeft(info.prefabName, info.isSel, GUILayout.Width(200));
        EditorGUILayout.LabelField(info.atlass.Count.ToString(), GUILayout.Width(50));
        EditorGUILayout.LabelField(info.noAtlas.Count.ToString(), GUILayout.Width(50));
        EditorGUILayout.LabelField(info.textures.Count.ToString(), GUILayout.Width(50));
        EditorGUILayout.LabelField(info.fonts.Count.ToString(), GUILayout.Width(50));
        info.isShowDetail = EditorGUILayout.Foldout(info.isShowDetail, "显示详情");
        EditorGUILayout.EndHorizontal();
        if (info.isShowDetail)
        {
            if (info.atlass.Count > 0)
            {
                EditorGUILayout.LabelField("图集详情");
                EditorGUILayout.BeginVertical(GUI.skin.box);

                foreach (string k in info.atlass.Keys)
                {
                    EditorGUILayout.LabelField(k);
                    EditorGUILayout.BeginVertical(GUI.skin.box);

                    List<string> ns = info.atlass[k];
                    for (int i = 0; i < ns.Count; ++i)
                    {
                        if (GUILayout.Button(ns[i]))
                        {
                            Object _object = AssetDatabase.LoadAssetAtPath(ns[i], typeof(Object));
                            EditorGUIUtility.PingObject(_object);
                        }

                        //EditorGUILayout.LabelField(ns[i]);
                    }

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndVertical();
            }

            if (info.noAtlas.Count > 0)
            {
                EditorGUILayout.LabelField("未设置图集精灵");
                EditorGUILayout.BeginVertical(GUI.skin.box);

                GUI.color = Color.red;
                for (int i = 0; i < info.noAtlas.Count; ++i)
                {
                    if (GUILayout.Button(info.noAtlas[i]))
                    {
                        Object _object = AssetDatabase.LoadAssetAtPath(info.noAtlas[i], typeof(Object));
                        EditorGUIUtility.PingObject(_object);
                    }
                    //EditorGUILayout.LabelField(info.noAtlas[i]);
                }
                GUI.color = Color.white;

                EditorGUILayout.EndVertical();
            }

            if (info.textures.Count > 0)
            {
                EditorGUILayout.LabelField("纹理详情");
                EditorGUILayout.BeginVertical(GUI.skin.box);

                for (int i = 0; i < info.textures.Count; ++i)
                {
                    if (GUILayout.Button(info.textures[i]))
                    {
                        Object _object = AssetDatabase.LoadAssetAtPath(info.textures[i], typeof(Object));
                        EditorGUIUtility.PingObject(_object);
                    }
                    //EditorGUILayout.LabelField(info.textures[i]);
                }

                EditorGUILayout.EndVertical();
            }

            if (info.fonts.Count > 0)
            {
                EditorGUILayout.LabelField("字集引用");
                EditorGUILayout.BeginVertical(GUI.skin.box);

                for (int i = 0; i < info.fonts.Count; ++i)
                {
                    EditorGUILayout.LabelField(info.fonts[i]);
                }

                EditorGUILayout.EndVertical();
            }

            if (info.others.Count > 0)
            {
                EditorGUILayout.LabelField("其他引用");
                EditorGUILayout.BeginVertical(GUI.skin.box);

                for (int i = 0; i < info.others.Count; ++i)
                {
                    if (GUILayout.Button(info.others[i]))
                    {
                        Object _object = AssetDatabase.LoadAssetAtPath(info.others[i], typeof(Object));
                        EditorGUIUtility.PingObject(_object);
                    }
                    //EditorGUILayout.LabelField(info.others[i]);
                }

                EditorGUILayout.EndVertical();
            }
        }
    }

    private void PackTagToLower()
    {
        string[] files = Directory.GetFiles(_uiAssetsFullPath, "*.png", SearchOption.AllDirectories);

        for (int i = 0; i < files.Length; ++i)
        {
            EditorUtility.DisplayProgressBar("UI资源解析", files[i], (float)i / (float)files.Length);

            string path = files[i].Remove(0, Application.dataPath.Length - 6);
            TextureImporter imp = AssetImporter.GetAtPath(path) as TextureImporter;

            imp.isReadable = true;
            imp.spritePackingTag = imp.spritePackingTag.ToLower();
            imp.SaveAndReimport();
        }
        EditorUtility.ClearProgressBar();
    }

    private Dictionary<string, List<string>> _refInfos = new Dictionary<string, List<string>>();

    private void CalBeReferences()
    {
        _refInfos.Clear();
        string[] allGuids = AssetDatabase.FindAssets("t:Material t:Prefab");//"t:Material t:Prefab t:Scene t:Model"

        string file = null;

        for (int i = 0; i < allGuids.Length; ++i)
        {
            file = AssetDatabase.GUIDToAssetPath(allGuids[i]);

            EditorUtility.DisplayProgressBar("正在查询引用", file, (float)i / (float)allGuids.Length);
            string[] paths = AssetDatabase.GetDependencies(file, false);
            for (int j = 0; j < paths.Length; ++j)
            {
                string _pathGuid = AssetDatabase.AssetPathToGUID(paths[j]);
                List<string> refs = null;
                if (!_refInfos.TryGetValue(_pathGuid, out refs))
                {
                    refs = new List<string>();
                    _refInfos.Add(_pathGuid, refs);
                }
                refs.Add(file);
            }
        }

        EditorUtility.ClearProgressBar();
    }

    private int GetRefCount(string path)
    {
        string _pathGuid = AssetDatabase.AssetPathToGUID(path);
        List<string> refs = null;
        if (_refInfos.TryGetValue(_pathGuid, out refs))
        {
            return refs.Count;
        }
        return 0;
    }
}

public class AtlasPackTool
{
    static float matAtlasSize = 2048;//最大图集尺寸  
    static float padding = 1;//每两个图片之间用多少像素来隔开              

    //判断图片格式不对了，先进性格式的转换
    static private void CheckFormat(List<TextureImporter> imps)
    {
        bool bNeedReImport = false;

        for (int i = 0; i < imps.Count; ++i)
        {
            TextureImporter imp = imps[i];
            if (imp.textureCompression != TextureImporterCompression.Uncompressed
                || imp.isReadable == false)
            {
                imp.textureCompression = TextureImporterCompression.Uncompressed;
                imp.textureType = TextureImporterType.Sprite;
                imp.isReadable = true;
                imp.mipmapEnabled = false;
                imp.npotScale = TextureImporterNPOTScale.None;//用于非二次幂纹理的缩放模式  
                imp.SaveAndReimport();

                bNeedReImport = true;
            }
        }

        if (bNeedReImport)
        {
            AssetDatabase.Refresh();
        }
    }

    //加载图片资源
    static private Texture2D[] LoadTextures(List<TextureImporter> imps)
    {
        Texture2D[] texs = new Texture2D[imps.Count];
        for (int i = 0; i < imps.Count; i++)
        {
            Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(imps[i].assetPath);
            texs[i] = tex;
            //判断图片命名的合法性
            if (tex.name.StartsWith(" ") || tex.name.EndsWith(" "))
            {
                string newName = tex.name.TrimStart(' ').TrimEnd(' ');
                Debug.LogWarning(string.Format("rename texture'name old name : {0}, new name {1}", tex.name, newName));
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(tex), newName);
            }

            string assetPath = AssetDatabase.GetAssetPath(tex);
            //重新把图片导入内存，理论上unity工程中的资源在用到的时候，Unity会自动导入到内存，但有的时候却没有自动导入，为了以防万一，我们手动导入一次  
            AssetDatabase.ImportAsset(assetPath);
        }

        return texs;
    }

    static public void PackTexture(List<TextureImporter> imps, string atlasName, string atlasPath, string texturePath)
    {
        if (imps == null || imps.Count <= 0)
        {
            Debug.Log("选择打包的小图数量为0");
            return;
        }

        //检查文件夹是否存在
        if (!Directory.Exists(atlasPath))
        {
            Directory.CreateDirectory(atlasPath);
        }

        //检查文件夹是否存在
        if (!Directory.Exists(texturePath))
        {
            Directory.CreateDirectory(texturePath);
        }

        //图集预制相对路径
        string relativePath = atlasPath + atlasName + ".prefab";
        //图集预制绝对路径
        //string absolutePath = Path.GetFullPath(atlasPath) + atlasName + ".png";

        //纹理相对路径
        string relativeTexPath = texturePath + atlasName + ".png";
        //纹理绝对路径
        string absoluteTexPath = Path.GetFullPath(texturePath) + atlasName + ".png";

        //判断图片格式不对了，先进性格式的转换
        CheckFormat(imps);
        //加载图片资源
        Texture2D[] texs = LoadTextures(imps);

        //创建Atlas实例
        UIAtlas atlasInfo = ScriptableObject.CreateInstance<UIAtlas>();
        atlasInfo.name = atlasName;

        //打包小图到大纹理并返回uv信息 
        Texture2D atlasTexture = new Texture2D(1, 1);
        Rect[] rs = atlasTexture.PackTextures(texs, (int)padding, (int)matAtlasSize);

        //把图集写入到磁盘文件
        File.WriteAllBytes(absoluteTexPath, atlasTexture.EncodeToPNG());

        //刷新图片 
        AssetDatabase.Refresh();
        AssetDatabase.ImportAsset(relativeTexPath);

        //记录图片的名字，只是用于输出日志用;  
        StringBuilder names = new StringBuilder();

        //SpriteMetaData结构可以让我们编辑图片的一些信息,想图片的name,包围盒border,在图集中的区域rect等  
        SpriteMetaData[] sheet = new SpriteMetaData[rs.Length];
        for (var i = 0; i < sheet.Length; i++)
        {
            SpriteMetaData meta = new SpriteMetaData();
            meta.name = texs[i].name;
            //这里的rect记录的是单个图片在图集中的uv坐标值             
            meta.rect = rs[i];
            meta.rect.Set(
                meta.rect.x * atlasTexture.width,
                meta.rect.y * atlasTexture.height,
                meta.rect.width * atlasTexture.width,
                meta.rect.height * atlasTexture.height
            );


            TextureImporter texImp = imps[i];
            //如果图片有包围盒信息的话  
            if (texImp != null)
            {
                meta.border = texImp.spriteBorder;
                meta.pivot = texImp.spritePivot;
            }

            sheet[i] = meta;

            SpriteData sd = new SpriteData();
            sd.name = meta.name;
            sd.alignment = meta.alignment;
            sd.border = new Vector4(meta.border.x, meta.border.y, meta.border.z, meta.border.w);
            sd.pivot = new Vector2(meta.pivot.x, meta.pivot.y);
            sd.rect = new Rect(meta.rect);

            if (!atlasInfo.HasSpriteData(meta.name))
            {
                atlasInfo._uvs.Add(sd);
            }
            else
            {
                Debug.LogErrorFormat("{0}图集中存在相同名称的图片：{1}", atlasInfo.name,meta.name);
            }

            //打印日志用  
            names.Append(meta.name);
            if (i < sheet.Length - 1)
                names.Append(",");
        }

        //设置图集纹理信息  
        TextureImporter imp = TextureImporter.GetAtPath(relativeTexPath) as TextureImporter;
        imp.textureType = TextureImporterType.Sprite;//图集的类型  
        imp.textureCompression = TextureImporterCompression.Uncompressed;
        imp.alphaIsTransparency = true;
        //imp.SetPlatformTextureSettings("iPhone", 2048, TextureImporterFormat.AutomaticTruecolor);
        imp.SetPlatformTextureSettings("Android", 2048, TextureImporterFormat.ETC2_RGBA8);
        imp.ClearPlatformTextureSettings("iPhone");
        imp.textureFormat = TextureImporterFormat.AutomaticTruecolor;//TextureImporterFormat.AutomaticCompressed;//图集的格式  
        imp.spriteImportMode = SpriteImportMode.Multiple;//Multiple表示我们这个大图片(图集)中包含很多小图片  
        imp.mipmapEnabled = false;//是否开启mipmap  
        imp.isReadable = false;
        imp.spritesheet = sheet;//设置图集中小图片的信息(每个图片所在的区域rect等)  
        // 保存并刷新  
        imp.SaveAndReimport();

        //重新加载导出的纹理
        Texture2D texture2d = AssetDatabase.LoadAssetAtPath<Texture2D>(relativeTexPath);

        //填充图集信息
        atlasInfo._tex2d = texture2d;
        AssetDatabase.CreateAsset(atlasInfo, relativePath);
        GameObject.DestroyImmediate(atlasInfo);

        //输出日志  
        Debug.Log("Atlas create ok. " + names.ToString());
    }    
}