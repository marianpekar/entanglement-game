using UnityEngine;

public class Pickable : MonoBehaviour 
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    public void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public void ResetPosition()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;
    }
}
