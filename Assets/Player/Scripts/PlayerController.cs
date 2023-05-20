using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private Picker picker;

    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private Transform playerTransform;

    private bool isActive = true;

    private float pickerSocketRoationSpeed = 600f;

    public void SetActive(bool isActive)
    {
        this.isActive = isActive;
    }

    private enum MouseButton 
    {
        Left = 0,
        Right = 1,
        Middle = 2,
    }

    void Update()
    {
        if (!isActive)
            return;

        if (Input.GetMouseButton((int)MouseButton.Right))
        {
            if (picker.HasPickable())
            {
                picker.DropObject();
                picker.ResetSocketSotation(playerTransform.position, Vector3.up);
            }
        }

        if (Input.GetMouseButton((int)MouseButton.Middle))
        {
            if (picker.HasPickable())
            {
                picker.ThrowObject();
                picker.ResetSocketSotation(playerTransform.position, Vector3.up);
            }
        }

        float scrollDelta = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scrollDelta) > 0.001f && picker.HasPickable())
        {
            picker.RotateSocketAround(playerTransform.position, Vector3.up, pickerSocketRoationSpeed * Time.deltaTime * scrollDelta);         
        }

        if (!Input.GetMouseButton((int)MouseButton.Left) || !Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out var hit, int.MaxValue)) 
                return;

        picker.ResetSocketSotation(playerTransform.position, Vector3.up);

        Debug.DrawLine(playerCamera.ScreenToWorldPoint(Input.mousePosition), hit.point, Color.red);

        GameObject gameObject = hit.collider.gameObject;
        Pickable pickable = gameObject.GetComponent<Pickable>();
        if (pickable != null)
        {
            picker.PickObject(gameObject);
        }

        SetTarget(hit.point);   
    }

    private void SetTarget(Vector3 target)
    {
        agent.destination = target;
    }
}