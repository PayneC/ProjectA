using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LuaEventTrigger : EventTrigger
{
    private GameObject _go = null;
    public GameObject go
    {
        get
        {
            if (_go == null)
                _go = gameObject;
            return _go;
        }
    }

    private LuaEventData _luaEventData = new LuaEventData();
    //private LuaFunction _luaFunction;
    private int _eventTypes = 0;

    private void CallLuaFunction(EventTriggerType t, PointerEventData eventData)
    {
        //if (_luaFunction == null)
        //    return;

        int type = 1 << (int)t;
        if ((type & _eventTypes) == 0)
            return;

        if (_luaEventData == null)
            _luaEventData = new LuaEventData();

        _luaEventData.button = (int)eventData.button;
        _luaEventData.clickCount = eventData.clickCount;
        _luaEventData.clickTime = eventData.clickTime;
        _luaEventData.delta_x = eventData.delta.x;
        _luaEventData.delta_y = eventData.delta.y;
        _luaEventData.dragging = eventData.dragging;
        _luaEventData.eligibleForClick = eventData.eligibleForClick;
        _luaEventData.pointerId = eventData.pointerId;
        _luaEventData.position_x = eventData.position.x;
        _luaEventData.position_y = eventData.position.y;
        _luaEventData.pressPosition_x = eventData.pressPosition.x;
        _luaEventData.pressPosition_y = eventData.pressPosition.y;
        _luaEventData.scrollDelta_x = eventData.scrollDelta.x;
        _luaEventData.scrollDelta_y = eventData.scrollDelta.y;
        _luaEventData.SetIsPointMoving(eventData.IsPointerMoving());
        _luaEventData.SetIsScrolling(eventData.IsScrolling());
        _luaEventData.camera = eventData.enterEventCamera;

        //_luaFunction.Call(type, _luaEventData);

        if (_luaEventData.button >= 0 && _luaEventData.button <= 2)
        {
            eventData.button = (PointerEventData.InputButton)_luaEventData.button;
        }
        eventData.clickCount = _luaEventData.clickCount;
        eventData.clickTime = _luaEventData.clickTime;
        eventData.delta.Set(_luaEventData.delta_x, _luaEventData.delta_y);

        eventData.dragging = _luaEventData.dragging;
        eventData.eligibleForClick = _luaEventData.eligibleForClick;
        eventData.pointerId = _luaEventData.pointerId;

        eventData.position.Set(_luaEventData.position_x, _luaEventData.position_y);
        eventData.pressPosition.Set(_luaEventData.pressPosition_x, _luaEventData.pressPosition_y);
        eventData.scrollDelta.Set(_luaEventData.scrollDelta_x, _luaEventData.scrollDelta_y);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        CallLuaFunction(EventTriggerType.BeginDrag, eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        CallLuaFunction(EventTriggerType.Drag, eventData);
    }

    public override void OnDrop(PointerEventData eventData)
    {
        CallLuaFunction(EventTriggerType.Drop, eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        CallLuaFunction(EventTriggerType.EndDrag, eventData);
    }

    public override void OnInitializePotentialDrag(PointerEventData eventData)
    {
        CallLuaFunction(EventTriggerType.InitializePotentialDrag, eventData);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        CallLuaFunction(EventTriggerType.PointerClick, eventData);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        CallLuaFunction(EventTriggerType.PointerDown, eventData);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        CallLuaFunction(EventTriggerType.PointerEnter, eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        CallLuaFunction(EventTriggerType.PointerExit, eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        CallLuaFunction(EventTriggerType.PointerUp, eventData);
    }

    public override void OnScroll(PointerEventData eventData)
    {
        CallLuaFunction(EventTriggerType.Scroll, eventData);
    }
   //public void SetLuaFunction(LuaFunction func, int eventTypes)
   //{
   //    if (_luaFunction != null)
   //        _luaFunction.Dispose();
   //
   //    _luaFunction = func;
   //    _eventTypes = eventTypes;
   //}

    void OnDestroy()
    {
       // if (_luaFunction != null)
       //     _luaFunction.Dispose();
       // _luaFunction = null;
    }
}
