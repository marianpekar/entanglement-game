using UnityEngine;

public class Chase : AIBehaviour
{
    public Transform target;
    public float agentSpeedMultiplier = 2f;

    private float lastDestinationSetTime;
    private float setDestinationInterval = 1f;

    public override void Activate(AIController controller)
    {
        controller.MultiplySpeed(agentSpeedMultiplier);
        controller.IgnoreEars(true);
    }

    public override void UpdateStep(AIController controller)
    {
        if (controller.RemainingDistance <= controller.StoppingDistance &&
            Time.time + setDestinationInterval < lastDestinationSetTime)
            return;

        lastDestinationSetTime = Time.time;
        controller.SetDestination(target.position);
    }

    public override void Deactivate(AIController controller)
    {
        controller.IgnoreEars(false);
    }
}
