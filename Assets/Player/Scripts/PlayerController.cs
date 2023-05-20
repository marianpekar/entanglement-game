using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField]
    private Camera playerCamera;

    private bool isActive = true;

    public void SetActive(bool isActive)
    {
        this.isActive = isActive;
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (!isActive ||
            !Input.GetMouseButton(0) ||
            !Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out var hit, int.MaxValue)) 
                return; 

        Debug.DrawLine(playerCamera.ScreenToWorldPoint(Input.mousePosition), hit.point, Color.red);
 
        SetTarget(hit.point);
    }

    private void SetTarget(Vector3 target)
    {
        agent.destination = target;
    }
}