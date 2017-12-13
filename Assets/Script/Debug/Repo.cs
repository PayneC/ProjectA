using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct RepoEntry
{
    public LogType type;
    public string condition;
    public string stackTrace;
}

static public class Repo
{
    static public List<RepoEntry> repos { get; private set;}

    static private string _sLogFormat = "[{0}][{1}] {2}";
	static private string _sLogByIdFormat = "[{0}][{1}] {2}";

    static private byte _uTypeSwitchView = 0;
    static private byte _uTypeSwitchText = 0;

    static private int _iMaxLogCount = 200;

    static public void LogCallback(string condition, string stackTrace, LogType type)
    {
        if(null == repos)
        {
            repos = new List<RepoEntry>();
        }

        RepoEntry _e;
        if (_iMaxLogCount > repos.Count)
        {
            _e = new RepoEntry();
        }
        else
        {
            _e = repos[0];
            repos.RemoveAt(0);
        }
        _e.condition = condition;
        _e.stackTrace = stackTrace;
        _e.type = type;
        repos.Add(_e);
    }

    static public void AddLogToView(byte _type)
    {

    }

    static public void AddLogToText(byte _type)
    {

    }

    static public void RemoveLogToView(byte _type)
    {

    }

    static public void RemoveLogToText(byte _type)
    {

    }
}

