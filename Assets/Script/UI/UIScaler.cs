using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScaler : MonoBehaviour
{
    public Vector2 m_ReferenceResolution = new Vector2(720, 1280); //开发时分辨率宽
    private int width = 0;
    private int height = 0;
    private Vector2 scrSizeDelta = Vector2.zero;
    public bool useScale = false;

    void Start()
    {
        RectTransform rectTransform = this.transform as RectTransform;
        if (rectTransform != null)
        {
            scrSizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
        }

        DoScaler();
    }

    void LateUpdate()
    {
        if (width != Screen.width || height != Screen.height)
        {
            DoScaler();
        }
    }

    private void DoScaler()
    {
        width = Screen.width;
        height = Screen.height;

        float s1 = (float)m_ReferenceResolution.x / (float)m_ReferenceResolution.y;
        float s2 = (float)width / (float)height;

        float scaler = 1f;

        if (s1 < s2)
        {
            scaler = s2 / s1;
        }
        else if (s1 > s2)
        {
            scaler = (1f / s2) / (1f / s1);
        }

        if (useScale)
        {
            RectTransform rectTransform = this.transform as RectTransform;
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = scrSizeDelta;
                rectTransform.localScale = new Vector3(scaler, scaler, scaler);
            }
        }
        else
        {
            RectTransform rectTransform = this.transform as RectTransform;
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = scrSizeDelta * scaler;
                rectTransform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }
}
