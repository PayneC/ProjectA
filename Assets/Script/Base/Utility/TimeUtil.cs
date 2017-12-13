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
}
