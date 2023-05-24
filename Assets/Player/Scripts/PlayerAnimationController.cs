using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private Animator animator;

    void FixedUpdate()
    {
        animator.SetBool("isRunning", !(agent.remainingDistance <= 0.1));
    }

    public void TriggerDie()
    {
        animator.SetTrigger("die");
    }
}