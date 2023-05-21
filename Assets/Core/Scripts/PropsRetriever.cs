using UnityEngine;

public class PropsRetriever : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        Pickable pickable = col.gameObject.GetComponent<Pickable>();
        if (pickable)
        {
            pickable.ResetPosition();
        }

        Rigidbody rigidbody = col.gameObject.GetComponent<Rigidbody>();
        if (rigidbody)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }
    }
}
