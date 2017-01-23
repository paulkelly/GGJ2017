using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingyThing : MonoBehaviour
{
    public AnimationCurve Curve;
    public bool In;
    public float Phase = 2;
    public float SwingTime;

    public Vector3 Start;
    public Vector3 End;

    private float _time = 0;
    private Vector3 _startPosition;

    private Rigidbody _rigidBody;
    private void Awake()
    {
        _startPosition = transform.position;
        _rigidBody = GetComponent<Rigidbody>();
        _time = Phase;
    }

	void FixedUpdate ()
    {
		if(In)
        {
            _time += Time.deltaTime;

            if(_time > SwingTime)
            {
                In = false;
            }
            else
            {
                float position = Curve.Evaluate(_time / SwingTime);
                _rigidBody.MoveRotation(Quaternion.Lerp(Quaternion.Euler(Start), Quaternion.Euler(End), position));
            }
        }
        else
        {
            _time -= Time.deltaTime;

            if (_time < 0)
            {
                In = true;
            }
            else
            {
                float position = Curve.Evaluate(_time / SwingTime);
                _rigidBody.MoveRotation(Quaternion.Lerp(Quaternion.Euler(Start), Quaternion.Euler(End), position));
            }
        }
        //_rigidBody.velocity = Vector3.zero;
        //_rigidBody.MovePosition(_startPosition);
    }
}
