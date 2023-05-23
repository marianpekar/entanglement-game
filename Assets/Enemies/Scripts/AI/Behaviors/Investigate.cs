using UnityEngine;

public class Investigate : AIBehaviour
{
    public Vector3 destination;
    public int investigateForSeconds = 3;
    public float agentSpeedMultiplier = 1.5f;

    private bool isInvestigating;
    private float investigationStartTime;

    public override void Activate(AIController controller)
    {
        controller.MultiplySpeed(agentSpeedMultiplier);
        controller.SetDestination(destination);
    }

    public override void UpdateStep(AIController controller)
    {
        if (controller.RemainingDistance <= controller.StoppingDistance && !isInvestigating)
        {
            isInvestigating = true;
            investigationStartTime = Time.time;
        }

        if (isInvestigating && Time.time > investigationStartTime + investigateForSeconds) 
        {
            isInvestigating = false;
            controller.Patrol();
        }
    }

    public override void Deactivate(AIController aIController)
    {
        isInvestigating = false;
    }
}
