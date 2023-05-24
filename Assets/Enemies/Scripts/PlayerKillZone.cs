using UnityEngine;
using UnityEngine.Events;

public class PlayerKillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponentInParent<PlayerController>().Kill();
        }
    }
}
