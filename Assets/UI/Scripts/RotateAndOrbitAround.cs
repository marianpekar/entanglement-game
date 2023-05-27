using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAndOrbitAround : MonoBehaviour
{
    [SerializeField]
    Vector3 rotationSpeed;

    [SerializeField]
    float orbitSpeed;

    [SerializeField]
    Transform orbitCenter;

    void Update()
    {
        transform.RotateAround(Vector3.zero, orbitCenter.transform.position, orbitSpeed * Time.deltaTime);
        transform.Rotate(rotationSpeed * Time.deltaTime, Space.Self);
    }
}
