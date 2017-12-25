using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputUtil
{
    public delegate void Joystick(float x, float y, float s);
   
    public delegate void OnAction(int id);

    public static Joystick onLeftJoystick;

    public static OnAction onAction;
    public static void SetJoystick(float x, float y, float s)
    {
        if(onLeftJoystick != null)
        {
            onLeftJoystick(x, y, s);
        }
    }

    public static void AddJoystick(Joystick _func)
    {
        onLeftJoystick += _func;
    }

    public static void AddAction(OnAction _func)
    {
        onAction += _func;
    }

    public static void DoAction(int id)
    {
        if(onAction != null)
        {
            onAction(id);
        }
    }
}
