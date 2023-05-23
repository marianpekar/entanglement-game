using UnityEngine;

public class Ears : Sense
{
    protected override bool HasDetected(Detectable detectable)
    {
        return Vector3.Distance(detectable.transform.position, transform.position) <= distance && detectable.CanBeHear;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = DebugDrawColor;
        Gizmos.DrawWireSphere(transform.position, distance);
    }
#endif
}
