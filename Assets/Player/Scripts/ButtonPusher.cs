using UnityEngine;
using UnityEngine.AI;

public class ButtonPusher : MonoBehaviour
{
    private ButtonPushable buttonToPush;
    private readonly float minimalPushableDistance = 1.5f;

    [SerializeField]
    private NavMeshAgent navMeshAgent;

    [SerializeField]
    private Transform playerTransform;

    void Update()
    {
        if (buttonToPush && Vector3.Distance(playerTransform.position, buttonToPush.transform.position) <= minimalPushableDistance)
        {
            StopNavMeshAgent();
            buttonToPush.Push();
            buttonToPush = null;
        }
    }

    private void StopNavMeshAgent()
    {
        navMeshAgent.destination = navMeshAgent.gameObject.transform.position;
    }

    public void PushButton(ButtonPushable button)
    {
        if (buttonToPush != null)
        {
            StopNavMeshAgent();
        }

        buttonToPush = button;
        navMeshAgent.SetDestination(buttonToPush.transform.position);
    }
}