using UnityEngine;

public class Chase : AIBehaviour
{
    public Transform target;
    public float agentSpeedMultiplier = 2f;

    public override void Activate(AIController controller)
    {
        controller.MultiplySpeed(agentSpeedMultiplier);
        controller.IgnoreEars(true);
    }

    public override void UpdateStep(AIController controller)
    {
        if (controller.RemainingDistance <= controller.StoppingDistance)
            return;

        controller.SetDestination(target.position);
    }

    public override void Deactivate(AIController controller)
    {
        controller.IgnoreEars(false);
    }
}
