using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManGoat : MonoBehaviour
{
    public Transform CameraFollowTarget;
    public ForwardRaycast ForwardRay;

    public Animator ManAnimator;
    public Animator GoatAnimator;

    public ParticleSystem ManParticles;
    public ParticleSystem GoatParticles;

    public Transform StartTransform;

    public CrashSFX Crash;

    public bool ManYelling;
    public bool GoatYelling;

    public float ManVolume;
    public float GoatVolume;

    public float MinTorque = 80000f;
    public float MaxTorque = 200000f;

    public float MinForce = 80000f;
    public float MaxForce = 200000f;

    private const float AnimationDampTime = 0.1f;
    private const float AccelerationDampTime = 0.02f;
    private const float CameraTargetDampTimeOut = 10f;
    private const float CameraTargetDampTimeIn = 2f;
    private const float CameraMaxDistance = 4;

    private Rigidbody _rigidbody;

    private float _cameraZ;
    private float _cameraTarget;
    private float _cameraDampVelocity;

    private float _manAnimationAcc;
    private float _manAnimationDampVelocity;

    private float _goatAnimationAcc;
    private float _goatAnimationDampVelocity;

    private float _manAcceleration;
    private float _manAccDampVelocity;

    private float _goatAcceleration;
    private float _goatAccDampVelocity;

    private bool _started;
    private bool _startForceApplied;
    private float _startTimer;
    private float _startTime = 0.1f;
    private bool _dead;
    private bool _win;

    private float _deadTimer = 0;
    private float _deadTime = 0.3f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
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

        if (Time.timeScale > 0)
        {
            ManAnimator.SetFloat("Scream", _manAnimationAcc);
            GoatAnimator.SetFloat("Scream", _goatAnimationAcc);
        }

        if (!_started)
        {
            if (GlobalMics.Instance.GameStarted)
            {
                //_startTimer += Time.deltaTime;
                //if (_startTimer > _startTime)
                //{
                //    _started = true;
                //}

                _started = true;
            }

            return;
        }

        ManParticles.emissionRate = _manAnimationAcc*15;
        GoatParticles.emissionRate = _goatAnimationAcc*15;
    }

    private void FixedUpdate()
    {
        if (!_startForceApplied)
        {
            if (GlobalMics.Instance.GameStarted)
            {
                _rigidbody.MovePosition(StartTransform.position);
                _rigidbody.AddForce(StartTransform.forward * Random.Range(4000f, 6000f));
                _rigidbody.AddTorque((_rigidbody.rotation * Vector3.up) * Random.Range(-1f, 1f) * 800f);
                _startForceApplied = true;
            }
        }

        if (!_started || _dead || _win)
        {
            _cameraTarget = 0;
            _cameraZ = Mathf.SmoothDamp(_cameraZ, _cameraTarget, ref _cameraDampVelocity, CameraTargetDampTimeIn, 2f, Time.fixedDeltaTime);

            CameraFollowTarget.localPosition = new Vector3(0, 0, _cameraTarget);

            return;
        }

        Vector3 forwardOnPlane = Vector3.ProjectOnPlane(_rigidbody.rotation * Vector3.forward, Vector3.up);
        Vector3 sidewaysOnPlane = Vector3.ProjectOnPlane(_rigidbody.rotation * Vector3.right, Vector3.up);
        bool deadRotation = false;
        if(forwardOnPlane.magnitude > 0)
        {
            deadRotation |= Vector3.Angle(_rigidbody.rotation * Vector3.forward, forwardOnPlane) > 15;
        }
        else
        {
            deadRotation = true;
        }
        if(sidewaysOnPlane.magnitude > 0)
        {
            deadRotation |= Vector3.Angle(_rigidbody.rotation * Vector3.right, sidewaysOnPlane) > 15;
        }
        else
        {
            deadRotation = true;
        }
        //Debug.Log(Vector3.Angle(_rigidbody.rotation * Vector3.forward, forwardOnPlane) + " : " + Vector3.Angle(_rigidbody.rotation * Vector3.right, sidewaysOnPlane));

        if (transform.position.y < -0.2f || deadRotation)
        {
            _deadTimer += Time.fixedDeltaTime;
            if (_deadTimer > _deadTime)
            {
                Kill();
            }
        }
        else
        {
            _deadTimer = 0;
        }

        if (ManYelling)
        {
            _manAcceleration = Mathf.SmoothDamp(_manAcceleration, ManVolume, ref _manAccDampVelocity, AccelerationDampTime, 1000, Time.fixedDeltaTime);
        }
        else
        {
            _manAcceleration = Mathf.SmoothDamp(_manAcceleration, 0, ref _manAccDampVelocity, AccelerationDampTime, 1000, Time.fixedDeltaTime);
        }

        if(GoatYelling)
        {
            _goatAcceleration = Mathf.SmoothDamp(_goatAcceleration, GoatVolume, ref _goatAccDampVelocity, AccelerationDampTime, 100, Time.fixedDeltaTime);
        }
        else
        {
            _goatAcceleration = Mathf.SmoothDamp(_goatAcceleration, 0, ref _goatAccDampVelocity, AccelerationDampTime, 100, Time.fixedDeltaTime);
        }


        float TurnAmount = _manAcceleration - _goatAcceleration;
        float torqueAmount = 0;
        float normalTurning = GlobalMics.Instance.TorqueCurve.Evaluate(Mathf.Abs(TurnAmount));
        if (Mathf.Abs(TurnAmount) > 0.07f)
        {
            torqueAmount = Mathf.Lerp(MinTorque, MaxTorque, normalTurning);
        }
        else
        {
            torqueAmount = Mathf.Lerp(0, MinTorque, Mathf.Abs(TurnAmount/0.07f));
        }
        if(TurnAmount < 0)
        {
            torqueAmount *= -1;
        }
        Vector3 torque = (_rigidbody.rotation * Vector3.up) * torqueAmount * Time.fixedDeltaTime;
        //Vector3 torque = Vector3.up * torqueAmount * Time.fixedDeltaTime;
        _rigidbody.AddTorque(torque);

        float maxCameraDist = CameraMaxDistance + _rigidbody.velocity.magnitude;

        if(ForwardRay.HasHit)
        {
            maxCameraDist = Mathf.Min(Vector3.Distance(ForwardRay.HitPoint, ForwardRay.transform.position), CameraMaxDistance);
        }
        _cameraTarget = Mathf.Lerp(maxCameraDist, 0, Mathf.Abs(TurnAmount));
        if (_cameraTarget > _cameraZ)
        {
            _cameraZ = Mathf.SmoothDamp(_cameraZ, _cameraTarget, ref _cameraDampVelocity, CameraTargetDampTimeOut, 0.2f, Time.fixedDeltaTime);
        }
        else
        {
            _cameraZ = Mathf.SmoothDamp(_cameraZ, _cameraTarget, ref _cameraDampVelocity, CameraTargetDampTimeIn, 0.8f, Time.fixedDeltaTime);
        }

        CameraFollowTarget.localPosition = new Vector3(0, 0, _cameraTarget);

        float BaseThrust = Mathf.Min(_manAcceleration, _goatAcceleration) * ((_manAcceleration + _goatAcceleration) / 2);
        BaseThrust = GlobalMics.Instance.ThrustCurve.Evaluate(BaseThrust);
        if (BaseThrust < 0.05f)
        {
            float bigger = Mathf.Max(_manAcceleration, _goatAcceleration);
            BaseThrust = Mathf.Min(bigger, 0.05f);
        }
        else
        {
            _rigidbody.angularVelocity = _rigidbody.angularVelocity - (_rigidbody.angularVelocity * BaseThrust * 2 * Time.fixedDeltaTime);
        }

        float thrust = 0;
        if (Mathf.Abs(BaseThrust) > 0.07f)
        {
            thrust = Mathf.Lerp(MinForce, MaxForce, Mathf.Abs(BaseThrust));
        }
        else
        {
            thrust = Mathf.Lerp(0, MinForce, Mathf.Abs(BaseThrust / 0.07f));
        }

        Vector3 forwardThrust = (_rigidbody.rotation * Vector3.forward) * thrust * Time.fixedDeltaTime;
        _rigidbody.AddForce(forwardThrust);
    }

    private void Kill()
    {
        if (!_dead)
        {
            _dead = true;
            GlobalMics.Instance.State = GameState.Finished;
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Kill")
        {
            //Kill();
            _rigidbody.AddForceAtPosition(col.contacts[0].normal * 1000, col.contacts[0].point);
            Vector3 normal =  Quaternion.FromToRotation(Vector3.forward, Vector3.right) * col.contacts[0].normal;
            _rigidbody.AddTorque(normal * 2000);

            Crash.Crash();
        }
        else
        {
            Crash.Crash(col.impulse.magnitude / 2);
        }


    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Goal")
        {
            _win = true;
            GlobalMics.Instance.State = GameState.Win;
        }
    }
}
