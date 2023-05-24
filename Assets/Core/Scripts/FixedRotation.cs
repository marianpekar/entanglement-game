using UnityEngine;

public class FixedRotation : MonoBehaviour
{
    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
