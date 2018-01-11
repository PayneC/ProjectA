using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeUtil
{
    private static float clientTime = 0;
    private static double serverTime = 0;

    public static void SetServerTime(double _time)
    {
        clientTime = Time.realtimeSinceStartup;
        serverTime = _time;
    }

    public static double GetServerTime()
    {
        return Time.realtimeSinceStartup - clientTime + serverTime;
    }

    /// <summary>
    /// 获取当前时间戳
    /// </summary>
    /// <param name="bflag">为真时获取10位时间戳,为假时获取13位时间戳.</param>
    /// <returns></returns>
    public static double GetTimeStamp()
    {
        TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return ts.TotalSeconds;
        /*
         long ret;
                if (bflag)
                    ret = Convert.ToInt64(ts.TotalSeconds);
                else
                    ret = Convert.ToInt64(ts.TotalMilliseconds);
                return ret;
                */
    }
}
