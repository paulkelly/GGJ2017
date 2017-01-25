using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;
    public AnimationCurve FollowCurve;

    private const float SmoothTime = 3f;

    private const float MaxDistance = 3;

    private float _initalZ;
    private Vector3 _initalOffset;
    private Vector3 _velocity;

    private bool _startCamera = false;
    private float _startTimer = 0;
    private float _startTime = 2f;

    private void Awake()
    {
        _initalZ = transform.position.z - 0.4f;
        _initalOffset = new Vector3(transform.position.x - Target.position.x, 0, 0);
    }

    private void Update()
    {
        if (!_startCamera && GlobalMics.Instance.State == GameState.Started)
        {
            _startTimer += Time.deltaTime;
            if (_startTimer > _startTime)
            {
                _startCamera = true;
            }

            Vector3 target = new Vector3(Target.position.x, transform.position.y, Target.position.z) + _initalOffset;

            if (target.z < _initalZ)
            {
                target.z = _initalZ;
            }

            float distance = Vector3.Distance(transform.position, target);
            float maxSpeed = (distance * FollowCurve.Evaluate(Mathf.Clamp((distance / MaxDistance), 0, 1)) + 0.2f);
            transform.position = Vector3.SmoothDamp(transform.position, target, ref _velocity, SmoothTime, maxSpeed, Time.deltaTime);
        }

        if (_startCamera && GlobalMics.Instance.State != GameState.Win)
        {
            Vector3 target = new Vector3(Target.position.x, transform.position.y, Target.position.z) + _initalOffset;

            if (target.z < _initalZ)
            {
                target.z = _initalZ;
            }

            float distance = Vector3.Distance(transform.position, target);
            float maxSpeed = (distance * FollowCurve.Evaluate(Mathf.Clamp((distance / MaxDistance), 0, 1)) + 0.2f);
            transform.position = Vector3.SmoothDamp(transform.position, target, ref _velocity, SmoothTime / maxSpeed, maxSpeed, Time.deltaTime);
        }
    }
}
