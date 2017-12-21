using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGesture : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private int m_nTouchCount = 0;

    void Awake()
    {
        _CalculateJoystick();
    }

    public void OnPointerDown(PointerEventData eventData)
    {   
        //eventData.pointerId
        ++m_nTouchCount;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        --m_nTouchCount;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
      
    }

    public void OnDrag(PointerEventData eventData)
    {
      
    }

    private void _CalculateJoystick()
    {
      
    }

    void Update()
    {
      
    }
}
