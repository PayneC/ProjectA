using UnityEngine;
using System.Collections;

public class AppMain : LuaClient
{
    //public string ip = "124.78.206.254";//"127.0.0.1";//180.171.45.124
    //public int port = 2323;//2012;//2323    
    private bool isRun = false;

    private void OnDownloadLastVersion()
    {
        if(isRun)
        {
            ExitGame();
        }
        EnterGame();
    }

    protected override void LoadLuaFiles()
    {
        base.LoadLuaFiles();
    }

    new void Awake()
    {
        HotFixMgr.I.Check(OnDownloadLastVersion);
    }

    void EnterGame()
    {        
        StartCoroutine(CoroutineUpdate());
        isRun = true;
        Init();
    }

    void ExitGame()
    {
        Destroy();
        StopCoroutine(CoroutineUpdate());
        AssetUtil.I.ClearAssets();
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.unscaledDeltaTime;
        // loading 时表现处理
        // 贯穿全局的loading管理类
        // 进入appmain后默认开启
        Loading.I.Update(dt);
        AssetUtil.I.Update(dt);
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
}
