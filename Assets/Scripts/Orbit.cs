using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public GameObject OrbitTarget;
    public Vector3 RotationAxis = Vector3.up;
    public float Speed = 10f;

    void FixedUpdate()
    {
        var lastRotation = transform.rotation;
        transform.RotateAround(OrbitTarget.transform.position, RotationAxis.normalized, Speed * Time.deltaTime);
        transform.rotation = lastRotation;
    }
}
