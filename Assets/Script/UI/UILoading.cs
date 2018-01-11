using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoading : MonoBehaviour
{
    public RawImage tex_bg;
    public Slider sli_progress;
    public Text txt_progress;

    private float _time;
    private float _progress;
    private bool _autoClose;

    private float _currentProgress;

    private string _format = "{0:p0}";

    private bool _isFirst = true;

	// Update is called once per frame
	void Update ()
    {
        if (_isFirst)
        {
            _isFirst = false;
            return;
        }            

        if(_time > 0)
        {
            float _speed = (_progress - _currentProgress) / _time;
            _currentProgress = _speed * Time.unscaledDeltaTime + _currentProgress;
            _currentProgress = Mathf.Clamp(_currentProgress, 0f, _progress); //Mathf.Clamp01(_currentProgress);

            _time -= Time.unscaledDeltaTime;
            _time = Mathf.Max(0, _time);
        }
        else if(_autoClose)
        {
            gameObject.SetActive(false);
        }
        sli_progress.value = _currentProgress;
        txt_progress.text = string.Format(_format, _currentProgress);
    }

    void OnEnable()
    {
        _currentProgress = 0;    
    }

    public void SetLoading(float t, float p, bool autoClose)
    {
        _progress = Mathf.Clamp01(p);
        _time = Mathf.Max(0, t);
        _autoClose = autoClose;

        _currentProgress = Mathf.Min(_currentProgress, _progress);
    }

    public void OpenLoading()
    {
        gameObject.SetActive(true);
    }
}
