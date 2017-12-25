using UnityEngine;
using System.Collections;

public class AppMain : MonoBehaviour
{
    public const int AppStateHotFix = 1;
    public const int AppStateGame = 2;

    static private int currentState = 0;
    static private int nextState = 0;

    static private LuaClient luaClient = null;

    static public void SetNextState(int _state)
    {
        if (_state > 0 && _state <= 2)
        {
            nextState = _state;
        }
    }
    //public string ip = "124.78.206.254";//"127.0.0.1";//180.171.45.124
    //public int port = 2323;//2012;//2323

    void Awake()
    {
        Application.logMessageReceived += Repo.LogCallback;

        luaClient = gameObject.AddComponent<LuaClient>();
    }

    void Start()
    {
        SetNextState(AppStateHotFix);
    }

    void StateUpdate()
    {
        if (nextState != 0 && nextState != currentState)
        {
            switch (currentState)
            {
                case AppStateHotFix:
                    // 退出热更新
                    break;
                case AppStateGame:
                    // 退出游戏
                    ExitGame();
                    break;
                default:
                    break;
            }

            currentState = nextState;
            nextState = 0;

            switch (currentState)
            {
                case AppStateHotFix:
                    SetNextState(AppStateGame);//temp
                    // 进入资源准备
                    break;
                case AppStateGame:
                    // 进入游戏
                    EnterGame();
                    break;
                default:
                    break;
            }
        }
    }

    void EnterGame()
    {        
        StartCoroutine(CoroutineUpdate());
        luaClient.Init();
    }

    void ExitGame()
    {
        luaClient.Destroy();
        StopCoroutine(CoroutineUpdate());
        AssetUtil.I.ClearAssets();
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.unscaledDeltaTime;
        StateUpdate();

        // loading 时表现处理
        // 贯穿全局的loading管理类
        // 进入appmain后默认开启
        Loading.I.Update(dt);

        if (currentState == AppStateHotFix)
        {
            HotFixMgr.I.Update(dt);
        }
        else if (currentState == AppStateGame)
        {
            AssetUtil.I.Update(dt);
        }        
    }

    IEnumerator CoroutineUpdate()
    {
#if UNITY_EDITOR && !TEST_AB
#else
        yield return AssetUtil.I.AsyncLoadAssetManifest();
#endif
        while (true)
        {
            yield return AssetUtil.I.CoroutineUpdate();
        }        
    }

    void OnDestroy()
    {
        //ExitGame();
    }
}
