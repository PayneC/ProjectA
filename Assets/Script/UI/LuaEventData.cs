using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaEventData
{
    public int button;
    public int clickCount;
    public float clickTime;
    public float delta_x;
    public float delta_y;
    public bool dragging;
    public bool eligibleForClick;
    public int pointerId;
    public float position_x;
    public float position_y;
    public float pressPosition_x;
    public float pressPosition_y;
    public float scrollDelta_x;
    public float scrollDelta_y;
    public bool useDragThreshold;
    public Camera camera;

    private bool _isPointerMoving;
    private bool _isScrolling;
    public bool IsPointerMoving()
    {
        return _isPointerMoving;
    }
    public bool IsScrolling()
    {
        return _isScrolling;
    }
    public void SetIsPointMoving(bool v)
    {
        _isPointerMoving = v;
    }
    public void SetIsScrolling(bool v)
    {
        _isScrolling = v;
    }
}
