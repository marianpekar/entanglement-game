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
        OnTurnOff?.Invoke();
        field.SetActive(false);
    }

    public void TurnOn()
    {
        OnTurnOn?.Invoke();
        field.SetActive(true);
    }
}
