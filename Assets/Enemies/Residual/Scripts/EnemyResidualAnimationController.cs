using UnityEngine;
using UnityEngine.AI;

public class EnemyResidualAnimationController : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private Animator animator;

    void FixedUpdate()
    {
        animator.SetFloat("Blend", agent.velocity.magnitude);
    }
}