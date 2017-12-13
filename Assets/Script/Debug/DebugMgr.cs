using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DebugMgr : MonoBehaviour
{
    enum DebugMode
    {
        None,
        Stats,
        StatsAndDebug,
    }

    public static DebugMgr I = null;

    private string[] _consoleType = null;
    private GUI.WindowFunction[] _consoleFunc = null;

    private StringBuilder _sbLine = null;
    private Rect _rect = new Rect();

    private int _defaultFontSize = 20;
    private int _defaultLineHeight = 28;

    private int _lineHeight = 20;
    private GUIStyle _btnStyle = null;
    private GUIStyle _commandBtnStyle = null;
    private GUIStyle _boxStyle = null;
    private GUIStyle _titleStyle = null;
    private GUIStyle _textAreaStyle = null;

    private DebugMode _debugMode = DebugMode.None;
    private bool _isDisplayConsole = false;
    private int _currentConsoleType = 0;
    private string _defaultConsoleTitle = "console";
    private Vector2 _consoleTypeView = Vector2.zero;

    private bool _isShowMenu;

    private bool _isHorizontal = false;

    private bool _isDisabledInput = false;
    private GameObject _eventSystem = null;

    void Awake()
    {
        I = this;
        _sbLine = new StringBuilder();
        //gameObject.AddComponent<FrameShow>();

#if UNITY_EDITOR
        _defaultFontSize = 20;
        _defaultLineHeight = 30;
        _lineHeight = 20;

        _debugMode = DebugMode.StatsAndDebug;
#else
        _defaultFontSize = 20;
        _defaultLineHeight = 30;
        _lineHeight = 30;
        _debugMode = DebugMode.StatsAndDebug;
#endif

        //控制台类型
        string[] types =
        {
            "main",
            "log",
            "command",
        };
        //对应控制台类型的窗口绘制函数
        GUI.WindowFunction[] cFunc =
        {
            Console_Main,
            Console_Log,
            Console_Command,
        };

        _consoleType = types;
        _consoleFunc = cFunc;

        _eventSystem = GameObject.Find("EventSystem");
    }

    void ResetStyle()
    {
        float scaleX = 1;
        float scaleY = 1;
        if (Screen.width < Screen.height)
        {
            scaleX = Screen.width / 720f;
            scaleY = Screen.height / 1280f;
        }
        else
        {
            scaleX = Screen.height / 720f;
            scaleY = Screen.width / 1280f;
        }

        _lineHeight = (int)(_defaultLineHeight * scaleY);

        if (_btnStyle == null)
            _btnStyle = new GUIStyle(GUI.skin.button);
        _btnStyle.fontSize = (int)(_defaultFontSize * scaleX);
        _btnStyle.alignment = TextAnchor.MiddleCenter;
        _btnStyle.fixedHeight = _lineHeight * 1.8f;
        _btnStyle.fixedWidth = _lineHeight * 3.6f;



        if (_commandBtnStyle == null)
            _commandBtnStyle = new GUIStyle(GUI.skin.button);
        _commandBtnStyle.fontSize = (int)(_defaultFontSize * scaleX);
        _commandBtnStyle.alignment = TextAnchor.MiddleCenter;
        _commandBtnStyle.fixedHeight = _lineHeight * 1.8f;

        if (_boxStyle == null)
            _boxStyle = new GUIStyle(GUI.skin.box);
        _boxStyle.fontSize = (int)(_defaultFontSize * scaleX);
        _boxStyle.alignment = TextAnchor.UpperLeft;
        _boxStyle.wordWrap = true;

        if (_titleStyle == null)
            _titleStyle = new GUIStyle(GUI.skin.box);
        _titleStyle.alignment = TextAnchor.MiddleCenter;
        _titleStyle.fontSize = (int)(_defaultFontSize * scaleX);

        if (_textAreaStyle == null)
            _textAreaStyle = new GUIStyle(GUI.skin.textArea);
        _textAreaStyle.alignment = TextAnchor.UpperLeft;
        _textAreaStyle.fontSize = (int)(_defaultFontSize * scaleX * 1.5f);

    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.F5))
        {
            switch (_debugMode)
            {
                case DebugMode.None:
                    _debugMode = DebugMode.Stats;
                    break;
                case DebugMode.Stats:
                    _debugMode = DebugMode.StatsAndDebug;
                    break;
                case DebugMode.StatsAndDebug:
                    _debugMode = DebugMode.None;
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// 界面按钮
    /// </summary>
    void OnGUI()
    {
        if (_debugMode == DebugMode.None)
            return;

        ResetStyle();

        switch (_debugMode)
        {
            case DebugMode.Stats:
                DisplayStatsMode();
                break;
            case DebugMode.StatsAndDebug:
                DisplayDebugMode();
                break;
            default:
                break;
        }
    }

    private void DrawLabel(string format, params object[] args)
    {
        if (string.IsNullOrEmpty(format))
            return;

        string line = null;

        if (args != null && args.Length > 0)
        {
            _sbLine.Remove(0, _sbLine.Length);
            _sbLine.AppendFormat(null, format, args);
            line = _sbLine.ToString();
        }
        else
        {
            line = format;
        }

        GUILayout.Label(line, _boxStyle);
    }

    private bool DrawButton(string format, params object[] args)
    {
        string line = null;

        if (_sbLine != null && !string.IsNullOrEmpty(format) && args != null && args.Length > 0)
        {
            _sbLine.Remove(0, _sbLine.Length);
            _sbLine.AppendFormat(null, format, args);
            line = _sbLine.ToString();
        }
        else
        {
            line = format;
        }

        return GUILayout.Button(line, _btnStyle);//, GUILayout.Height(_lineHeight * 2f));
    }

    private bool DrawToggle(bool value, string text)
    {
        if (value)
        {
            GUI.contentColor = Color.green;
        }
        else
        {
            GUI.contentColor = Color.white;
        }

        bool rlt = value;

        if (GUILayout.Button(text, _btnStyle))//, GUILayout.Height(_lineHeight * 1.8f), GUILayout.MinWidth(_lineHeight * 4f)))
        {
            rlt = !value;
        }

        GUI.contentColor = Color.white;

        return rlt;
    }

    Color GetColorByLogType(byte b)
    {
        switch (b)
        {
            case 1:
                return Color.white;
            case 2:
                return Color.yellow;
            case 4:
                return Color.red;
            default:
                return Color.white;
        }
    }

    private void CalculateRectByScreenRatio(ref Rect rt, float x, float y, float width, float height)
    {
        rt.Set(x * Screen.width, y * Screen.height, width * Screen.width, height * Screen.height);
    }

    private void DisplayStatsMode()
    {
        //DrawLabel(FrameShow.fps.ToString("f2"));
    }

    private void DisplayDebugMode()
    {
        if (DrawButton("{0}\nconsole"))
        {
            _isDisplayConsole = !_isDisplayConsole;
            if (_isDisplayConsole)
            {
                _isDisabledInput = true;
                _eventSystem.SetActive(false);
            }
            else
            {
                _isDisabledInput = false;
                _eventSystem.SetActive(true);
            }
        }

        if (!_isDisplayConsole)
        {
            return;
        }

        if (_consoleType == null || _consoleType.Length <= 0)
        {
            return;
        }

        //title
        string title = _defaultConsoleTitle;
        if (_consoleType != null && _currentConsoleType < _consoleType.Length)
        {
            title = _consoleType[_currentConsoleType];
        }

        _rect.Set(_btnStyle.fixedWidth, 0, _btnStyle.fixedWidth, _btnStyle.fixedHeight);
        _colorBack = GUI.contentColor;
        if (_isShowMenu)
        {
            GUI.contentColor = Color.green;
        }
        else
        {
            GUI.contentColor = Color.white;
        }
        if (GUI.Button(_rect, title, _btnStyle))
        {
            _isShowMenu = !_isShowMenu;
        }
        GUI.contentColor = _colorBack;

        title = "!Exit";
        _rect.Set(_btnStyle.fixedWidth * 3, 0, _btnStyle.fixedWidth, _btnStyle.fixedHeight);
        if (GUI.Button(_rect, title, _btnStyle))
        {
            _debugMode = DebugMode.None;
            _isDisabledInput = false;
            _eventSystem.SetActive(!_isDisabledInput);
        }

        title = "热重启";
        _rect.Set(_btnStyle.fixedWidth * 4, 0, _btnStyle.fixedWidth, _btnStyle.fixedHeight);
        if (GUI.Button(_rect, title, _btnStyle))
        {
            _debugMode = DebugMode.StatsAndDebug;
            _isDisplayConsole = !_isDisplayConsole;
            _isDisabledInput = false;
            _eventSystem.SetActive(true);
        }

        title = "禁用输入";
        _rect.Set(_btnStyle.fixedWidth * 5, 0, _btnStyle.fixedWidth, _btnStyle.fixedHeight);
        if (_isDisabledInput)
        {
            GUI.contentColor = Color.green;
        }
        else
        {
            GUI.contentColor = Color.white;
        }

        if (GUI.Button(_rect, title, _btnStyle))
        {
            _isDisabledInput = !_isDisabledInput;
            _eventSystem.SetActive(!_isDisabledInput);
        }

        GUI.contentColor = _colorBack;
        //draw window
        _rect.Set(0f, _btnStyle.fixedHeight, Screen.width, Screen.height - _btnStyle.fixedHeight * 2);
        if (_isShowMenu)
        {
            GUILayout.BeginArea(_rect, GUI.skin.box);
            int index = GUILayout.SelectionGrid(_currentConsoleType, _consoleType, 5, _btnStyle, GUILayout.Height(_lineHeight * _consoleType.Length * 2), GUILayout.Width(Screen.width * 0.7f));
            if (index != _currentConsoleType)
            {
                _isShowMenu = false;
                _currentConsoleType = index;
            }
            GUILayout.EndArea();
        }
        else
        {
            GUI.WindowFunction func = null;
            if (_consoleFunc != null && _currentConsoleType < _consoleFunc.Length)
            {
                func = _consoleFunc[_currentConsoleType];
            }
            if (func != null)
            {
                GUILayout.BeginArea(_rect, GUI.skin.box);
                func(0);
                GUILayout.EndArea();
            }
        }
    }

    #region Main Window
    private void Console_Main(int id)
    {
        MainOpenDebug();
        MainShowAccount();
    }

    private void RightToolbar_Main(int id)
    {

    }

    private string _ip = "localhost";
    private string _port = "7003";

    private void MainOpenDebug()
    {
        _ip = GUILayout.TextField(_ip, _textAreaStyle);
        _port = GUILayout.TextField(_port, _textAreaStyle);
        if (DrawButton("Debug"))
        {
            int rlt = 0;
            if (int.TryParse(_port, out rlt))
            {
            }
        }
    }

    private string _sAccount = string.Empty;
    private string _newAccount = string.Empty;
    private void MainShowAccount()
    {
        if (string.IsNullOrEmpty(_sAccount))
        {
            _sAccount = "";
            _newAccount = "";
        }

        GUILayout.Label("当前账号:", _boxStyle);
        GUILayout.TextField(_sAccount, _textAreaStyle);
        _newAccount = GUILayout.TextField(_newAccount, _textAreaStyle);
        if (DrawButton("更换账号"))
        {

        }
    }

    #endregion

    #region Log Window
    private Vector2 _logView = Vector2.zero;
    private Vector2 _logTypeView = Vector2.zero;

    private Color _colorBack = Color.clear;
    private bool _isUnlock = false;

    private bool _isShowControl = true;

    private void Console_Log(int id)
    {
        //GUIStyle gs = GUI.skin.verticalScrollbar;
        //GUIStyle gs1 = GUI.skin.verticalScrollbarThumb;

        //gs.fixedWidth = 30;
        //gs1.fixedWidth = 30;

        _logView = GUILayout.BeginScrollView(_logView, false, false);

        GUILayout.EndScrollView();

        //gs.fixedWidth = 0;
        //gs1.fixedWidth = 0;
    }

    private void _Log_TypeSelectToolbar()
    {
        _rect.Set(0f, _btnStyle.fixedHeight, _btnStyle.fixedWidth, Screen.height - _btnStyle.fixedHeight * 2);
        GUILayout.BeginArea(_rect, GUI.skin.box);

        if (DrawButton("  上翻  "))
        {
            _logTypeView.Set(_logTypeView.x, _logTypeView.y - _lineHeight);
        }
        _logTypeView = GUILayout.BeginScrollView(_logTypeView, false, false);

        GUILayout.EndScrollView();
        if (DrawButton("  下翻  "))
        {            
            _logTypeView.Set(_logTypeView.x, _logTypeView.y + _lineHeight);
        }

        GUILayout.EndArea();
    }

    private void _log_Control()
    {
        _rect.Set(Screen.width - _btnStyle.fixedWidth, _btnStyle.fixedHeight, _btnStyle.fixedWidth, Screen.height - _btnStyle.fixedHeight * 2);
        GUILayout.BeginArea(_rect, GUI.skin.box);
        //==================

        if (DrawButton("Clear"))
        {
            
        }

        _isUnlock = DrawToggle(_isUnlock, "解锁滚动");

        GUILayout.Space(10f);

        if (DrawButton("  上一页  "))
        {
            _isUnlock = true;
            _logView.Set(_logView.x, _logView.y - Screen.height * 0.7f);
        }
        if (DrawButton("  下一页  "))
        {
            _isUnlock = true;
            _logView.Set(_logView.x, _logView.y + Screen.height * 0.7f);
        }

        GUILayout.Space(10f);

        if (DrawButton("  上一条  "))
        {
            _isUnlock = true;
            _logView.Set(_logView.x, _logView.y - _lineHeight);
        }

        if (DrawButton("  下一条  "))
        {
            _isUnlock = true;
            _logView.Set(_logView.x, _logView.y + _lineHeight);
        }

        GUILayout.Space(10f);

        //=====================
        GUILayout.EndArea();
    }

    private void Log_Custom(int id)
    {
        string title = null;
        _rect.Set(_btnStyle.fixedWidth * 2, 0, _btnStyle.fixedWidth, _btnStyle.fixedHeight);
        if (_isShowControl)
        {
            title = "隐藏控制";
        }
        else
        {
            title = "显示控制";
        }

        if (GUI.Button(_rect, title, _btnStyle))
        {
            _isShowControl = !_isShowControl;
        }

        if (_isShowControl)
        {
            _Log_TypeSelectToolbar();
            _log_Control();
        }
    }

    private void _ShowRepoEntity(RepoEntry entity)
    {

    }
    #endregion

    #region Command Window

    private void Console_Command(int id)
    {
        Command_LUA();
    }

    string lua_debugInfo = "debug_test()";
    bool useCustom = false;
    private void Command_LUA()
    {
        GUILayout.BeginHorizontal(GUILayout.Height(Screen.height * 0.3f));

        GUILayout.BeginVertical(GUILayout.Height(Screen.height * 0.3f));
        _debugButtonView = GUILayout.BeginScrollView(_debugButtonView);
        for (int i = 0; i < luaCommands.Count; ++i)
        {
            if (GUILayout.Button(luaCommands[i], _commandBtnStyle))
            {

            }
        }

        GUILayout.EndScrollView();
        GUILayout.EndVertical();

        GUILayout.BeginVertical(GUILayout.Height(Screen.height * 0.3f));
        if (GUILayout.Button("上一页", _commandBtnStyle))
        {
            _debugButtonView -= new Vector2(0, 200);
        }
        if (GUILayout.Button("下一页", _commandBtnStyle))
        {
            _debugButtonView += new Vector2(0, 200);
        }
        if (GUILayout.Button("Clear", _commandBtnStyle))
        {
            luaContent.Remove(0, luaContent.Length);
        }
        useCustom = DrawToggle(useCustom, "自定义");

        if (GUILayout.Button("上翻", _commandBtnStyle))
        {
            _DebugContentView -= new Vector2(0, 200);
        }
        if (GUILayout.Button("下翻", _commandBtnStyle))
        {
            _DebugContentView += new Vector2(0, 200);
        }


        GUILayout.EndVertical();

        GUILayout.EndHorizontal();

        GUILayout.BeginVertical(GUILayout.Height(Screen.height * 0.3f));
        _DebugContentView = GUILayout.BeginScrollView(_DebugContentView);
        GUILayout.Label(luaContent.ToString(), _boxStyle);
        GUILayout.EndScrollView();

        GUILayout.EndVertical();

        if (useCustom)
        {
            GUILayout.BeginHorizontal();
            lua_debugInfo = GUILayout.TextArea(lua_debugInfo, _textAreaStyle);
            if (DrawButton("执行"))
            {

            }
            GUILayout.EndHorizontal();
        }
    }

    //主动输入文本调试lua
    private Vector2 _debugButtonView = Vector2.zero;
    private Vector2 _DebugContentView = Vector2.zero;
    List<string> luaCommands = new List<string>();
    StringBuilder luaContent = new StringBuilder();

    public void AddCommand(string _function)
    {
        if (!string.IsNullOrEmpty(_function))
        {
            luaCommands.Add(_function);
        }
    }

    public void SetContent(string _content)
    {
        luaContent.AppendLine(_content);
    }
    #endregion
}
