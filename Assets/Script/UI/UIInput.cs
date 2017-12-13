using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInput : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public Image m_kStartPoint = null;
    public Image m_kCurrentPoint = null;
    public float m_fRadius = 32f;

    private int m_nJoyId;
    private int m_nCameraId;
    private int m_nScaleId1;
    private int m_nScaleId2;

    private bool m_hasJoy;
    private bool m_hasCamera;
    private bool m_hasScale1;
    private bool m_hasScale2;

    private Vector2 m_vJoyStartPoint = Vector2.zero;
    private Vector2 m_vJoyCurrentPoint = Vector2.zero;
    private Vector2 m_vJoyDirection = Vector2.zero;
    private Vector2 m_vJoyDirectionNormalized = Vector2.zero;
    
    private Vector2 m_vCameraStartPoint = Vector2.zero;
    private Vector2 m_vCameraCurrentPoint = Vector2.zero;
    private Vector2 m_vCameraDirection = Vector2.zero;
    private Vector2 m_vCameraDirectionNormalized = Vector2.zero;

    private Vector2 m_vToGameDirection2D = Vector2.zero;
    private Vector3 m_vToGameDirection = Vector3.zero;

    private Vector2 m_vScalePoint1 = Vector2.zero;
    private Vector2 m_vScalePoint2 = Vector2.zero;
    private float m_fScaleDisSqr = 0f;

    private float m_fLength = 0f;
    private float m_fStrength = 0f;

    private bool m_needCalScale = false;
    private int m_nTouchCount = 0;

    void Awake()
    {
        _CalculateJoystick();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ++m_nTouchCount;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        --m_nTouchCount;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (m_nTouchCount - _ValidPointCount() > 1)
        {
            if (m_hasJoy == false && m_hasCamera == false)
            {
                if (!m_hasScale1)
                {
                    _ScaleBegin1(eventData);
                }
                else if (!m_hasScale2)
                {
                    _ScaleBegin2(eventData);
                }
                else if (InJoyArea(eventData.position))
                {
                    _JoyBegin(eventData);
                }
                else
                {
                    m_vCameraStartPoint.Set(eventData.position.x, eventData.position.y);
                    m_hasCamera = true;
                }
            }
        }
        else
        {
            if (InJoyArea(eventData.position))
            {
                _JoyBegin(eventData);
            }
            else
            {
                _CameraBegin(eventData);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(m_hasJoy && m_nJoyId == eventData.pointerId)
        {
            _JoyEnd(eventData);
        }
        else if(m_hasCamera && m_nCameraId == eventData.pointerId)
        {
            _CameraEnd(eventData);
        }
        else if (m_hasScale1 && m_nScaleId1 == eventData.pointerId)
        {
            _ScaleEnd1(eventData);
        }
        else if (m_hasScale2 && m_nScaleId2 == eventData.pointerId)
        {
            _ScaleEnd2(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (m_hasJoy && m_nJoyId == eventData.pointerId)
        {
            _JoyDrag(eventData);
        }
        else if (m_hasCamera && m_nCameraId == eventData.pointerId)
        {
            _CameraDrag(eventData);
        }
        else if (m_hasScale1 && m_nScaleId1 == eventData.pointerId)
        {
            _ScaleDrag1(eventData);
        }
        else if (m_hasScale2 && m_nScaleId2 == eventData.pointerId)
        {
            _ScaleDrag2(eventData);
        }
    }

    private void _CalculateJoystick()
    {
        if (m_kStartPoint != null)
            m_kStartPoint.gameObject.SetActive(m_hasJoy);
        if (m_kCurrentPoint != null)
            m_kCurrentPoint.gameObject.SetActive(m_hasJoy);

        if (!m_hasJoy)
        {
            return;
        }

        m_vJoyDirection = m_vJoyCurrentPoint - m_vJoyStartPoint;
        m_fLength = m_vJoyDirection.magnitude;
        if (m_fLength == 0)
        {
            m_vJoyDirectionNormalized = Vector2.zero;
        }
        else
        {
            m_vJoyDirectionNormalized = m_vJoyDirection / m_fLength;
        }

        if (m_fLength > m_fRadius)
        {
            m_vJoyStartPoint = m_vJoyCurrentPoint - m_vJoyDirectionNormalized * m_fRadius;
            m_fLength = m_fRadius;
        }

        m_fStrength = m_fLength / m_fRadius;

        if (m_kStartPoint != null)
        {
            m_kStartPoint.rectTransform.position = m_vJoyStartPoint;
        }

        if (m_kCurrentPoint != null)
        {
            m_kCurrentPoint.rectTransform.position = m_vJoyDirectionNormalized * m_fLength + m_vJoyStartPoint;
        }

        m_vToGameDirection2D = m_vJoyDirectionNormalized * m_fLength / m_fRadius;
        m_vToGameDirection.Set(m_vToGameDirection2D.x, 0f, m_vToGameDirection2D.y);
    }

    void Update()
    {
        if (m_hasJoy)
        {
            InputUtil.SetJoystick(m_vToGameDirection.x, m_vToGameDirection.z, m_fStrength);
        }

        /*
        if (m_needCalScale && m_hasScale1 && m_hasScale2)
        {
            m_needCalScale = false;
            float newDis = Vector2.SqrMagnitude(m_vScalePoint1 - m_vScalePoint2);
            if(newDis != m_fScaleDisSqr)
            {
                CameraRPG crpg = SceneMgr.I.kMainCamera.GetComponent<CameraRPG>();
                if (crpg != null)
                {
                    crpg.SetCameraScale(newDis > m_fScaleDisSqr);
                }
            }
        }*/
    }

    private bool InJoyArea(Vector2 pos)
    {
        RectTransform rectTransform = transform as RectTransform;
        
        if(rectTransform.rect.width * 0.3f >= pos.x)
        {
            return true;
        }
        return false;
    }

    private void _JoyBegin(PointerEventData eventData)
    {
        m_hasJoy = true;
        m_nJoyId = eventData.pointerId;

        m_vJoyStartPoint.Set(eventData.position.x, eventData.position.y);
        
        m_vJoyCurrentPoint.Set(eventData.position.x, eventData.position.y);
        _CalculateJoystick();
    }
    private void _CameraBegin(PointerEventData eventData)
    {
        m_hasCamera = true;
        m_nCameraId = eventData.pointerId;

        m_vCameraStartPoint.Set(eventData.position.x, eventData.position.y);
    }
    private void _ScaleBegin1(PointerEventData eventData)
    {
        m_hasScale1 = true;
        m_nScaleId1 = eventData.pointerId;

        m_vScalePoint1.Set(eventData.position.x, eventData.position.y);

        if(m_hasScale2)
        {
            m_fScaleDisSqr = Vector2.SqrMagnitude(m_vScalePoint1 - m_vScalePoint2);
        }
    }
    private void _ScaleBegin2(PointerEventData eventData)
    {
        m_hasScale2 = true;
        m_nScaleId2 = eventData.pointerId;

        m_vScalePoint2.Set(eventData.position.x, eventData.position.y);

        if (m_hasScale1)
        {
            m_fScaleDisSqr = Vector2.SqrMagnitude(m_vScalePoint1 - m_vScalePoint2);
        }
    }

    private void _JoyEnd(PointerEventData eventData)
    {
        m_hasJoy = false;
        m_nJoyId = 0;

        _CalculateJoystick();
        InputUtil.SetJoystick(0, 0, 0);
        //InputMgr.I.SetJoystickDir(Vector3.zero);
    }
    private void _CameraEnd(PointerEventData eventData)
    {
        m_hasCamera = false;
        m_nCameraId = 0;
    }
    private void _ScaleEnd1(PointerEventData eventData)
    {
        m_hasScale1 = false;
        m_nScaleId1 = 0;
    }
    private void _ScaleEnd2(PointerEventData eventData)
    {
        m_hasScale2 = false;
        m_nScaleId2 = 0;
    }

    private void _JoyDrag(PointerEventData eventData)
    {
        m_vJoyCurrentPoint.Set(eventData.position.x, eventData.position.y);
        _CalculateJoystick();
    }
    private void _CameraDrag(PointerEventData eventData)
    {
        /*
        if (SceneMgr.I.kMainCamera != null)
        {
            CameraRPG crpg = SceneMgr.I.kMainCamera.GetComponent<CameraRPG>();
            if (crpg != null)
            {
                if (eventData.delta.sqrMagnitude > 1f)
                {
                    crpg.AddRotation(eventData.delta);
                }
            }

        }
        */
    }
    private void _ScaleDrag1(PointerEventData eventData)
    {
        m_vScalePoint1.Set(eventData.position.x, eventData.position.y);
        m_needCalScale = true;
    }
    private void _ScaleDrag2(PointerEventData eventData)
    {
        m_vScalePoint2.Set(eventData.position.x, eventData.position.y);
        m_needCalScale = true;
    }

    private int _ValidPointCount()
    {
        if (m_hasJoy && m_hasCamera)
            return 2;

        if (m_hasJoy || m_hasCamera)
            return 1;

        return 0;
    }
}
