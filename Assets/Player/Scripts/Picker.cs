using UnityEngine;
using UnityEngine.AI;

public class Picker: MonoBehaviour
{
    private GameObject pickedGO;
    private bool isPicked;
    private Rigidbody pickedRigidbody;
    private NavMeshObstacle pickedNavMeshObstacle;
    private readonly float minPickableDistance = 2f;
    private readonly float pickupSpeed = 10f;

    [SerializeField]
    private Transform socket;

    [SerializeField]
    private NavMeshAgent naveMeshAgent;

    private readonly float throwForce = 5f;

    private readonly float bobOffset = 0.12f;
    private readonly float bobSpeed = 2f;

    private float socketRotatedAngle = 0f;
    private readonly float maxRotatedAngle = 90f;

    void Update()
    {
        if (pickedGO && Vector3.Distance(socket.position, pickedGO.transform.position) <= minPickableDistance)
        {
            if (!isPicked)
            {
                pickedRigidbody = pickedGO.GetComponent<Rigidbody>();
                pickedRigidbody.useGravity = false;
                pickedNavMeshObstacle = pickedGO.GetComponent<NavMeshObstacle>();
                pickedNavMeshObstacle.enabled = false;
                naveMeshAgent.destination = naveMeshAgent.gameObject.transform.position;
                isPicked = true;
            }

            Vector3 targetPosition = new(socket.transform.position.x, socket.transform.position.y + Mathf.Sin(Time.time * bobSpeed) * bobOffset, socket.transform.position.z);
            pickedGO.transform.position = Vector3.Lerp(pickedGO.transform.position, targetPosition, Time.deltaTime * pickupSpeed);
            pickedGO.transform.rotation = socket.transform.rotation;
        }    
    }

    public void PickObject(GameObject gameObject)
    {
        if (pickedGO != null)
            return;

        pickedGO = gameObject;
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
        pickedRigidbody.useGravity = true;
        pickedRigidbody.AddForce((socket.forward + socket.up).normalized * throwForce, ForceMode.Impulse);
        pickedNavMeshObstacle.enabled = true;
        isPicked = false;
        pickedGO = null;
    }

    public void DropObject()
    {
        pickedRigidbody.useGravity = true;
        pickedNavMeshObstacle.enabled = true;
        isPicked = false;
        pickedGO = null;
    }
}