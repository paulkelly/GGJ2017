using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameController : MonoBehaviour
{
    private List<VolumePip> _pips = new List<VolumePip>();
    void Start()
    {
        foreach (VolumePip p in GetComponentsInChildren<VolumePip>())
        {
            _pips.Add(p);
        }

        for (int i = 0; i < _pips.Count; i++)
        {
            SetPipColor(_pips[i], 1 - ((float)i / (float)_pips.Count));
        }
    }

    private void SetPipColor(VolumePip pip, float normalPosition)
    {
        Color c = Color.yellow;
        float pos = normalPosition;
        if (normalPosition < 0.5f)
        {
            pos = normalPosition / 0.5f;
            c = Color.Lerp(Color.green, Color.yellow, normalPosition / 0.5f);
        }
        else if (normalPosition > 0.5f)
        {
            pos = (normalPosition / 0.5f) - 0.5f;
            c = Color.Lerp(Color.yellow, Color.red, (normalPosition / 0.5f) - 1);
        }
        pip.SetColor(c);
    }


    private const float AccelerationDampTime = 0.1f;

    public bool ManYelling;
    public bool GoatYelling;

    public float ManVolume;
    public float GoatVolume;

    private float _manAcceleration;
    private float _manAccDampVelocity;

    private float _goatAcceleration;
    private float _goatAccDampVelocity;

    private float _startGameValue;

    private float _startGameMulti = 0.4f;
    private readonly float _startGameMultiTarget = 5;
    private float _startGameMultiVel = 0;
    private readonly float _startGameMultiDampTime = 0.5f;

    private void Update()
    {
        if (!(GlobalMics.Instance.State == GameState.NotStarted || GlobalMics.Instance.State == GameState.Intro))
        {
            return;
        }

        ManYelling = GlobalMics.Instance.Player1Yelling;
        ManVolume = GlobalMics.Instance.Player1Volume;

        GoatYelling = GlobalMics.Instance.Player2Yelling;
        GoatVolume = GlobalMics.Instance.Player2Volume;

        if (ManYelling)
        {
            _manAcceleration = Mathf.SmoothDamp(_manAcceleration, ManVolume, ref _manAccDampVelocity, AccelerationDampTime, 1000, Time.deltaTime);
        }
        else
        {
            _manAcceleration = Mathf.SmoothDamp(_manAcceleration, 0, ref _manAccDampVelocity, AccelerationDampTime, 1000, Time.deltaTime);
        }

        if (GoatYelling)
        {
            _goatAcceleration = Mathf.SmoothDamp(_goatAcceleration, GoatVolume, ref _goatAccDampVelocity, AccelerationDampTime, 100, Time.deltaTime);
        }
        else
        {
            _goatAcceleration = Mathf.SmoothDamp(_goatAcceleration, 0, ref _goatAccDampVelocity, AccelerationDampTime, 100, Time.deltaTime);
        }

        float BaseThrust = Mathf.Min(_manAcceleration, _goatAcceleration) * ((_manAcceleration + _goatAcceleration) / 2);
        BaseThrust = GlobalMics.Instance.ThrustCurve.Evaluate(Mathf.Clamp01(BaseThrust));
        if (BaseThrust < 0.1f)
        {
            float bigger = Mathf.Max(_manAcceleration, _goatAcceleration);
            BaseThrust = Mathf.Min(bigger, 0.1f);
        }

        if (GlobalMics.Instance.State == GameState.NotStarted)
        {
            _startGameMulti = Mathf.SmoothDamp(_startGameMulti, _startGameMultiTarget, ref _startGameMultiVel,
                _startGameMultiDampTime, 100, Time.deltaTime);
            _startGameValue = _startGameValue + (BaseThrust * _startGameMulti * Time.deltaTime);

            if (_startGameValue >= 1)
            {
                GlobalMics.Instance.StartGame();
            }
        }
        else
        {
            BaseThrust = Mathf.Min(BaseThrust, 0.5f);
            _startGameValue = _startGameValue + (BaseThrust * _startGameMulti * Time.deltaTime);
        }

        if (_startGameValue > 0)
        {
            _startGameValue = Mathf.Clamp(_startGameValue - (Mathf.Clamp(_startGameValue, 0, 0.5f) * Time.deltaTime), 0, 1);
        }

        UpdatePips();
    }

    private void UpdatePips()
    {
        for (int i = 0; i < _pips.Count; i++)
        {
            //1-Volume because scripts are in back to front order
            bool on = ((float)i / (float)_pips.Count) >= (1 - _startGameValue);
            if (on)
            {
                _pips[i].SetAlpha(1);
            }
            else
            {
                _pips[i].SetAlpha(0);
            }
        }
    }
}
