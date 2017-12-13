using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGrid : MonoBehaviour
{
    public UIGridElement _element = null;
    public ScrollRect _scrollRect = null;
    [SerializeField]
    [HideInInspector]
    private int _elementCount;
    [SerializeField]
    [HideInInspector]
    private float _elementWidth = 1;
    [SerializeField]
    [HideInInspector]
    private float _elementHeight = 1;
    [SerializeField]
    [HideInInspector]
    private int _constraintCount = 1;
    [SerializeField]
    [HideInInspector]
    private bool _isVertical = false;

    private int _columnCount = 0;
    private int _rowCount = 0;
    private int _indexStart = -1;
    private int _indexEnd = -1;
    private int _showCount = 0;
    private List<UIGridElement> _elements = new List<UIGridElement>();
    private Queue<UIGridElement> _excess = new Queue<UIGridElement>();
    private Queue<int> _lack = new Queue<int>();

    public void Awake()
    {
        _scrollRect = GetComponent<ScrollRect>();
        if (null != _scrollRect)
        {
            _scrollRect.onValueChanged.AddListener(OnValueChanged);
            _scrollRect.content.anchorMin = new Vector2(0f, 1f);//new Vector2(0.5f, 1f);
            _scrollRect.content.anchorMax = new Vector2(0f, 1f);//new Vector2(0.5f, 1f);
            _scrollRect.content.pivot = new Vector2(0f, 1f);//new Vector2(0.5f, 1f);
        }
    }

    public void SetElementCount(int _count)
    {
        _elementCount = _count;
    }

    public void SetElementSize(float width, float height)
    {
        _elementWidth = Mathf.Max(1f, width);
        _elementHeight = Mathf.Max(1f, height);
    }

    public void SetIsVertical(bool _b)
    {
        _isVertical = _b;
    }

    public void SetConstraintCount(int _count)
    {
        _constraintCount = _count;
        if (_constraintCount < 1)
        {
            _constraintCount = 1;
        }
    }

    private void OnValueChanged(Vector2 v)
    {
        if (_showCount < 1)
        {
            return;
        }
        if (_isVertical)
        {
            _CalVertical(_scrollRect.content.anchoredPosition.y);
        }
        else
        {
            _CalHorizontal(_scrollRect.content.anchoredPosition.x);
        }
    }

    private void _CalHorizontal(float x)
    {
        int startColumn = (int)(-x / _elementWidth);
        int startIndex = startColumn * _rowCount;

        if (startIndex > _elementCount - _showCount)
        {
            startIndex = _elementCount - _showCount;
        }
        else if (startIndex < 0)
        {
            startIndex = 0;
        }
        int endIndex = _showCount + startIndex - 1;

        if (_indexStart != startIndex || _indexEnd != endIndex)
        {
            for (int i = 0; i < _showCount; ++i)
            {
                if (_elements[i].GetIndex() < startIndex || _elements[i].GetIndex() > endIndex)
                {
                    _excess.Enqueue(_elements[i]);
                }
            }

            if (startIndex < _indexStart)
            {
                for (int i = startIndex; i < _indexStart && i <= endIndex; ++i)
                {
                    _lack.Enqueue(i);
                }
            }
            if (endIndex > _indexEnd)
            {
                for (int i = endIndex; i > _indexEnd && i >= startIndex; --i)
                {
                    _lack.Enqueue(i);
                }
            }

            int _needIndex = 0;
            UIGridElement _e = null;
            while (_lack.Count > 0 & _excess.Count > 0)
            {
                _needIndex = _lack.Dequeue();
                _e = _excess.Dequeue();
                if (_e != null)
                {
                    int _x = _needIndex / _rowCount;
                    int _y = _needIndex - _x * _rowCount;
                    _e.transform.localPosition = new Vector3(_x * _elementWidth, -_y * _elementHeight, 0);
                    _e.SetIndex(_needIndex);
                }
            }

            _indexStart = startIndex;
            _indexEnd = endIndex;
        }
    }

    private void _CalVertical(float y)
    {
        int startRow = (int)(y / _elementHeight);
        int startIndex = startRow * _columnCount;

        if (startIndex > _elementCount - _showCount)
        {
            startIndex = _elementCount - _showCount;
        }
        else if (startIndex < 0)
        {
            startIndex = 0;
        }
        int endIndex = _showCount + startIndex - 1;

        if (_indexStart != startIndex || _indexEnd != endIndex)
        {
            for (int i = 0; i < _showCount; ++i)
            {
                if (_elements[i].GetIndex() < startIndex || _elements[i].GetIndex() > endIndex)
                {
                    _excess.Enqueue(_elements[i]);
                }
            }

            if (startIndex < _indexStart)
            {
                for (int i = startIndex; i < _indexStart && i <= endIndex; ++i)
                {
                    _lack.Enqueue(i);
                }
            }
            if (endIndex > _indexEnd)
            {
                for (int i = endIndex; i > _indexEnd && i >= startIndex; --i)
                {
                    _lack.Enqueue(i);
                }
            }

            int _needIndex = 0;
            UIGridElement _e = null;
            while (_lack.Count > 0 & _excess.Count > 0)
            {
                _needIndex = _lack.Dequeue();
                _e = _excess.Dequeue();
                if (_e != null)
                {
                    int _y = _needIndex / _columnCount;
                    int _x = _needIndex - _y * _columnCount;
                    _e.transform.localPosition = new Vector3(_x * _elementWidth, -_y * _elementHeight, 0);
                    _e.SetIndex(_needIndex);
                }
            }

            _indexStart = startIndex;
            _indexEnd = endIndex;
        }
    }

    public void ApplySetting()
    {
        if (null == _scrollRect)
        {
            if (null == GetScrollRect())
            {
                Debug.LogError("UIGrid ApplySetting ScrollRect is null");
                return;
            }
        }


        //_scrollRect.content.anchoredPosition = Vector2.zero;

        if (_isVertical)
        {
            _columnCount = _constraintCount;
            _rowCount = Mathf.CeilToInt((float)_elementCount / _constraintCount);
            _showCount = (int)((_scrollRect.viewport.rect.height / _elementHeight) + 2) * _columnCount;
        }
        else
        {
            _rowCount = _constraintCount;
            _columnCount = Mathf.CeilToInt((float)_elementCount / _constraintCount);
            _showCount = (int)((_scrollRect.viewport.rect.width / _elementWidth) + 2) * _rowCount;
        }
        _scrollRect.content.sizeDelta = new Vector2(_columnCount * _elementWidth, _rowCount * _elementHeight);
        _scrollRect.normalizedPosition = new Vector2(0, 1);

        _showCount = Mathf.Min(_showCount, _elementCount);

        for (int i = 0; i < _elements.Count; ++i)
        {
            _elements[i].gameObject.SetActive(true);
            _elements[i].index = -1;
        }

        if (_elements.Count < _showCount)
        {
            for (int i = _elements.Count; i < _showCount; ++i)
            {
                if (_element != null)
                {
                    UIGridElement _ge = Object.Instantiate(_element);
                    if (_ge != null)
                    {
                        _elements.Add(_ge);
                        _ge.transform.SetParent(_scrollRect.content);
                        _ge.transform.localScale = Vector3.one;
                        _ge.gameObject.SetActive(true);
                    }
                }
            }
        }
        else if (_elements.Count > _showCount)
        {
            for (int i = _showCount; i < _elements.Count; ++i)
            {
                _elements[i].gameObject.SetActive(false);
            }
        }
        _indexStart = -1;
        _indexEnd = -1;

        OnValueChanged(new Vector2(0, 1));
    }

    public void ClearSetting()
    {
        for (int i = 0; i < _elements.Count; ++i)
        {
            if (_elements[i])
            {
                DestroyImmediate(_elements[i].gameObject);
            }
        }

        _elements = new List<UIGridElement>();
    }


    public int GetElementCount() { return _elementCount; }
    public float GetElementWidth() { return _elementWidth; }
    public float GetElementHeight() { return _elementHeight; }
    public int GetColumnCount() { return _columnCount; }
    public int GetRowCount() { return _rowCount; }
    public int GetConstraintCount() { return _constraintCount; }
    public bool IsVertical() { return _isVertical; }
    public ScrollRect GetScrollRect()
    {
        if (null == _scrollRect)
        {
            _scrollRect = GetComponent<ScrollRect>();
        }
        return _scrollRect;
    }
    public int GetIndexStart() { return _indexStart; }
    public int GetIndexEnd() { return _indexEnd; }
    public int GetShowCount() { return _showCount; }

    public float GetCurrentPos()
    {
        if (_isVertical)
        {
            return _scrollRect.verticalNormalizedPosition;
        }
        else
        {
            return _scrollRect.horizontalNormalizedPosition;
        }
    }

    public void SetCurrentPos(float pos)
    {
        if (_isVertical)
        {
            _scrollRect.verticalNormalizedPosition = pos;
            OnValueChanged(_scrollRect.normalizedPosition);
        }
        else
        {
            _scrollRect.horizontalNormalizedPosition = pos;
            OnValueChanged(_scrollRect.normalizedPosition);
        }
    }
}