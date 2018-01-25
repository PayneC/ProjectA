using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIGridElement : MonoBehaviour
{
    private UnityAction onIndexChange;

    public int index = -1;

    public void SetIndex(int _index)
    {
        if (index != _index)
        {
            index = _index;
            if (onIndexChange != null)
                onIndexChange();
        }
    }

    public int GetIndex()
    {
        return index;
    }

    public void AddIndexChangeListener(UnityAction func)
    {
        onIndexChange += func;
    }

    public void RemoveIndexChangeListener(UnityAction func)
    {
        if (onIndexChange != null)
            onIndexChange -= func;
    }
}
