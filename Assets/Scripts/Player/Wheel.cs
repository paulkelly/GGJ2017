using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public Rigidbody Player;
    public bool CrazyWheel = false;

    private float crazyOffset = 0;

    private void Update()
    {
        if (Player.velocity.magnitude > 0.05f)
        {
            Quaternion target = Quaternion.LookRotation(Player.velocity);
            if (CrazyWheel)
            {
                Vector3 newRotation = target.eulerAngles;
                newRotation += Vector3.up*crazyOffset;

                crazyOffset += (CrazyWheelSpinSpeed * Time.deltaTime)%360;
                target = Quaternion.Euler(newRotation);
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, target, 10* Time.deltaTime);
        }
    }

    private float CrazyWheelSpinSpeed
    {
        get { return 180*Player.velocity.magnitude*Mathf.Max(Player.angularVelocity.magnitude, 0.1f); }
    }
}
