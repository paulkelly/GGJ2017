using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaBar : MonoBehaviour
{
    public int Player;
    private bool _tryDisable;
    private bool _tryDisableRunning;
    private bool _enabled;
    public RectTransform _transform;
    public CanvasGroup CanvasGroup;
    private float _alphaTarget = 1;
    private float _startValue = 1;
    private float _lerpTime;
    private readonly float _maxLerpTime = 0.3f;

    private bool StaminaBarEnabled
    {
        get { return _enabled; }
        set
        {
            if (_enabled != value)
            {
                _enabled = value;
                _lerpTime = 0;
                _startValue = CanvasGroup.alpha;
                if (_enabled)
                {
                    _alphaTarget = 1;
                }
                else
                {
                    _alphaTarget = 0;
                }
            }
        }
    }

    private void Update()
    {
        if (Player == 1)
        {
            if (GlobalMics.Instance.Player1UsingController)
            {
                _tryDisable = false;
                StaminaBarEnabled = true;
            }
            else
            {
                _tryDisable = true;
            }
            _transform.localScale = new Vector3(_transform.localScale.x, GlobalMics.Instance.Player1Stamina, _transform.localScale.z);
        }
        if (Player == 2)
        {
            if (GlobalMics.Instance.Player2UsingController)
            {
                _tryDisable = false;
                StaminaBarEnabled = true;
            }
            else
            {
                _tryDisable = true;
            }
            _transform.localScale = new Vector3(_transform.localScale.x, GlobalMics.Instance.Player2Stamina, _transform.localScale.z);
        }

        if (StaminaBarEnabled)
        {
            if (_tryDisable)
            {
                if (!_tryDisableRunning)
                {
                    StartCoroutine(TryDisable());
                }
            }
            else
            {
                if (_tryDisableRunning)
                {
                    _tryDisableRunning = false;
                    StopAllCoroutines();
                }
            }
        }

        if (_lerpTime < _maxLerpTime)
        {
            _lerpTime += Time.deltaTime;
            float percComplete = _lerpTime/_maxLerpTime;
            CanvasGroup.alpha = Mathf.Lerp(_startValue, _alphaTarget, percComplete);
        }
        else
        {
            CanvasGroup.alpha = _alphaTarget;
        }
    }

    private IEnumerator TryDisable()
    {
        _tryDisableRunning = true;

        yield return new WaitForSeconds(3);

        _tryDisableRunning = false;
        StaminaBarEnabled = false;
    }
}
