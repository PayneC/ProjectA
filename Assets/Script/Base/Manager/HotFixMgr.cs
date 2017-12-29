using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HotFixMgr : Singleton<HotFixMgr>
{
    private string lastVersion;
    public string LastVersion
    {
        get { return lastVersion; }
    }

    private string currenrVersion;
    public string CurrenrVersion
    {
        get { return currenrVersion; }
    }

    private bool isChecking = false;

    private bool isDownloading = false;

    public void Check(UnityAction call)
    {
        call();
    }

    public void Download(UnityAction call)
    {
        call();
    }
}
