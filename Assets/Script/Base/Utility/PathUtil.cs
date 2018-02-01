using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PathUtil : Singleton<PathUtil>
{
    public string GetStreamingAssesPath()
    {
#if UNITY_EDITOR && !TEST_AB
        return "Assets/ResourcesAB/";
#else
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            return Application.streamingAssetsPath + "/";
        }
        else
        {
            return Application.streamingAssetsPath + "/";
        }
#endif
        //Application.persistentDataPath
    }

    public string GetRuntimePlatform()
    {
        string platform = "";
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            platform = "StandaloneWindows";
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            platform = "Android";
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            platform = "iOS";
        }
        else if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXEditor)
        {
			platform = "StandaloneOSXUniversal";
        }
        return platform;
    }
}
