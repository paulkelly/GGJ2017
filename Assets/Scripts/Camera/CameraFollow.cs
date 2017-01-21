using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;

    private const float SmoothTime = 3f;

    private Vector3 _initalOffset;
    private Vector3 _velocity;

    private void Awake()
    {
        _initalOffset = new Vector3(transform.position.x - Target.position.x, 0, 0);
    }

    private void Update()
    {
        Vector3 target = new Vector3(Target.position.x, transform.position.y, Target.position.z) + _initalOffset;
        float maxSpeed = Mathf.Max(0.5f, 2*Vector3.Distance(transform.position, target));
        transform.position = Vector3.SmoothDamp(transform.position, target, ref _velocity, SmoothTime/maxSpeed, maxSpeed, Time.deltaTime);
    }
}
