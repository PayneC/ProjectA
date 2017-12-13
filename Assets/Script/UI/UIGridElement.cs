using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGridElement : MonoBehaviour
{
    public int index = -1;

    public void SetIndex(int _index)
    {
        if(index != _index)
        {
            index = _index;
        }
    }

    public int GetIndex()
    {
        return index;
    }
}
