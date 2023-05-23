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
}
