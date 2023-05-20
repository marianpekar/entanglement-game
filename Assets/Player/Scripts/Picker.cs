using UnityEngine;
using UnityEngine.AI;

public class Picker: MonoBehaviour
{
    private GameObject pickedGO;
    private Rigidbody pickedRigidbody;
    private NavMeshObstacle pickedNavMeshObstacle;
    private readonly float minPickableDistance = 2f;
    private readonly float pickupSpeed = 10f;

    [SerializeField]
    private Transform socket;

    private readonly float throwForce = 5f;

    private readonly float bobOffset = 0.12f;
    private readonly float bobSpeed = 2f;

    private float socketRotatedAngle = 0f;
    private readonly float maxRotatedAngle = 60f;

    void Update()
    {
        if (pickedGO && Vector3.Distance(socket.position, pickedGO.transform.position) <= minPickableDistance)
        {
            Vector3 targetPosition = new(socket.transform.position.x, socket.transform.position.y + Mathf.Sin(Time.time * bobSpeed) * bobOffset, socket.transform.position.z);
            pickedGO.transform.position = Vector3.Lerp(pickedGO.transform.position, targetPosition, Time.deltaTime * pickupSpeed);
            pickedGO.transform.rotation = socket.transform.rotation;
        }    
    }

    public void PickObject(GameObject gameObject)
    {
        pickedGO = gameObject;
        pickedRigidbody = gameObject.GetComponent<Rigidbody>();
        pickedRigidbody.isKinematic = true;
        pickedNavMeshObstacle = gameObject.GetComponent<NavMeshObstacle>();
        pickedNavMeshObstacle.enabled = false;
    }

    public void RotateSocketAround(Vector3 point, Vector3 axis, float angle)
    {
        if (socketRotatedAngle + angle > maxRotatedAngle || socketRotatedAngle + angle < -maxRotatedAngle)
            return;

        socket.RotateAround(point, axis, angle);
        socketRotatedAngle += angle;
    }

    public void ResetSocketSotation(Vector3 point, Vector3 axis)
    {
        socket.RotateAround(point, axis, -socketRotatedAngle);
        socketRotatedAngle = 0;
    }

    public bool HasPickable()
    {
        return pickedGO != null;
    }

    public void ThrowObject()
    {
        pickedRigidbody.isKinematic = false;
        pickedRigidbody.AddForce((socket.forward + socket.up).normalized * throwForce, ForceMode.Impulse);
        pickedNavMeshObstacle.enabled = true;
        pickedGO = null;
    }

    public void DropObject()
    {
        pickedRigidbody.isKinematic = false;
        pickedNavMeshObstacle.enabled = true;
        pickedGO = null;
    }
}