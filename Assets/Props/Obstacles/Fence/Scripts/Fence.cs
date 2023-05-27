using UnityEngine;
using UnityEngine.Events;

public class Fence : MonoBehaviour
{
    [SerializeField]
    private GameObject field;

    public UnityEvent OnTurnOn;
    public UnityEvent OnTurnOff;

    public void TurnOff()
    {
        if (!field.activeInHierarchy)
            return;

        OnTurnOff?.Invoke();
        field.SetActive(false);
    }

    public void TurnOn()
    {
        if (field.activeInHierarchy)
            return;

        OnTurnOn?.Invoke();
        field.SetActive(true);
    }
}
