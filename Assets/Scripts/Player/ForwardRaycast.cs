using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardRaycast : MonoBehaviour
{
    public LayerMask Mask;

    public bool HasHit;
    public Vector3 HitPoint;

    private const float Max = 4;

	private void FixedUpdate ()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        HasHit = Physics.Raycast(ray, out hit, Max, Mask);
        if(HasHit)
        {
            HitPoint = hit.point;
        }
	}
}
