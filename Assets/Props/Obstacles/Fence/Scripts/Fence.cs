using UnityEngine;

public class Fence : MonoBehaviour
{
    [SerializeField]
    private GameObject field;

    public void TurnOff()
    {
        field.SetActive(false);
    }

    public void TurnOn()
    {
        field.SetActive(true);
    }
}
