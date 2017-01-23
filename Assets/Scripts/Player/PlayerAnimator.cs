using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator ManAnimator;
    public Animator GoatAnimator;


    public bool ManYelling;
    public bool GoatYelling;

    public float ManVolume;
    public float GoatVolume;

    private const float AnimationDampTime = 0.1f;

    private float _manAnimationAcc;
    private float _manAnimationDampVelocity;

    private float _goatAnimationAcc;
    private float _goatAnimationDampVelocity;

    private void Update()
    {

        ManYelling = GlobalMics.Instance.Player1Yelling;
        ManVolume = GlobalMics.Instance.Player1Volume;

        GoatYelling = GlobalMics.Instance.Player2Yelling;
        GoatVolume = GlobalMics.Instance.Player2Volume;

        if (ManYelling)
        {
            _manAnimationAcc = Mathf.SmoothDamp(_manAnimationAcc, ManVolume, ref _manAnimationDampVelocity, AnimationDampTime, 1000, Time.deltaTime);
        }
        else
        {
            _manAnimationAcc = Mathf.SmoothDamp(_manAnimationAcc, 0, ref _manAnimationDampVelocity, AnimationDampTime, 1000, Time.deltaTime);
        }

        if (GoatYelling)
        {
            _goatAnimationAcc = Mathf.SmoothDamp(_goatAnimationAcc, GoatVolume, ref _goatAnimationDampVelocity, AnimationDampTime, 100, Time.deltaTime);
        }
        else
        {
            _goatAnimationAcc = Mathf.SmoothDamp(_goatAnimationAcc, 0, ref _goatAnimationDampVelocity, AnimationDampTime, 100, Time.deltaTime);
        }

        ManAnimator.SetFloat("Scream", _manAnimationAcc);
        GoatAnimator.SetFloat("Scream", _goatAnimationAcc);
    }
}
