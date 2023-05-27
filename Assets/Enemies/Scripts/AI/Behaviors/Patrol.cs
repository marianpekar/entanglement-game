using System.Collections;
using UnityEngine;

public class Patrol : AIBehaviour
{
    public Transform[] patrolPoints;
    private int currentPPIndex;

    public int delayAtPatrolPoint = 2;

    private bool isWaitingOnPatrolPoint;

    public override void Activate(AIController controller)
    {
        controller.SetDefaultSpeed();
        controller.SetDestination(patrolPoints[currentPPIndex].position);    
    }

    public override void UpdateStep(AIController controller)
    {
        if (controller.RemainingDistance <= controller.StoppingDistance && !isWaitingOnPatrolPoint) 
        {
            isWaitingOnPatrolPoint = true;
            StartCoroutine(WaitAndMoveToNextDestination(controller));
        }
    }

    public IEnumerator WaitAndMoveToNextDestination(AIController controller)
    {
        yield return new WaitForSeconds(delayAtPatrolPoint);
        currentPPIndex = currentPPIndex < patrolPoints.Length - 1 ? currentPPIndex + 1 : 0;
        controller.SetDestination(patrolPoints[currentPPIndex].position);
        isWaitingOnPatrolPoint = false;
        yield return new WaitForEndOfFrame();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        for (int i = 0; i < patrolPoints.Length - 1; i++)
        {
            Transform patrolPoint = patrolPoints[i];
            Transform nextPatrolPoint = patrolPoints[i + 1];
    
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(patrolPoint.position, nextPatrolPoint.position);
            Gizmos.DrawSphere(patrolPoint.position, 0.125f);
            Gizmos.DrawSphere(nextPatrolPoint.position, 0.125f);
        }
    }
#endif
}
