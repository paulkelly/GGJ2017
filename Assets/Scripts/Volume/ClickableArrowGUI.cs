using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickableArrowGUI : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Action OnClick = () => { };

    private Image _image;
    private Color _original;
    private Color _disabled = Color.gray;
    private Vector3 _targetSize = Vector3.one;
    private bool _enabled = true;
    private float _overSize = 1.3f;
    private float _downSize = 0.8f;

    public void SetEnabled(bool enabled)
    {
        _enabled = enabled;
        if (_enabled)
        {
            _image.color = _original;
        }
        else
        {
            _image.color = _disabled;
        }
    }

    private Vector3 Size
    {
        get
        {
            return _lerpEndSize;
        }
        set
        {
            _lerpStartSize = transform.localScale;
            _lerpEndSize = value;
            _lerpTime = 0;
        }
    }

    private void Awake()
    {
        _image = GetComponent<Image>();
        _original = _image.color;
        _targetSize = transform.localScale;
        _lerpStartSize = transform.localScale;
        _lerpEndSize = transform.localScale;
    }

    private Vector3 _lerpStartSize;
    private Vector3 _lerpEndSize;
    private float _lerpTime = 0.1f;
    private float _lerpMaxTime = 0.1f;

    private bool _isOver = false;
    private bool _isDown = false;


    private void Update()
    {
        if (!_enabled)
        {
            if (Size != _targetSize)
            {
                Size = _targetSize;
            }
        }
        else if (_isDown)
        {
            if (Size != _targetSize* _downSize)
            {
                Size = _targetSize* _downSize;
            }
        }
        else if (_isOver)
        {
            if (Size != _targetSize * _overSize)
            {
                Size = _targetSize * _overSize;
            }
        }
        else
        {
            if(Size != _targetSize)
            {
                Size = _targetSize;
            }
        }


        if (_lerpTime < _lerpMaxTime)
        {
            _lerpTime += Time.unscaledDeltaTime;
            float percComplete = _lerpTime / _lerpMaxTime;
            transform.localScale = Vector3.Lerp(_lerpStartSize, _lerpEndSize, percComplete);
        }
        else
        {
            transform.localScale = _lerpEndSize;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_enabled)
        {
            OnClick.Invoke();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isOver = false;
        _isDown = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDown = false;
    }
}
