using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ButtonPushable : MonoBehaviour
{
    private float pushOffset = 0.06f;
    private float movementSpeed = 0.5f;

    private bool isPushed;

    private Vector3 targetPosition;
    private Vector3 initialPosition;

    public UnityEvent OnPressed;

    [SerializeField]
    public float unlockDelay = 1f;

    [SerializeField]
    GameObject buttonTop;

    public void Push()
    {
        if (isPushed)
            return;

        isPushed = true;

        initialPosition = buttonTop.transform.position;
        targetPosition = new Vector3(buttonTop.transform.position.x, buttonTop.transform.position.y - pushOffset, buttonTop.transform.position.z);

        StartCoroutine(MoveTo(targetPosition, MoveBack, true));
    }

    private IEnumerator MoveTo(Vector3 position, Action onComplete, bool invokeOnPressed)
    {
        while (Vector3.Distance(buttonTop.transform.position, position) > 0.001f)
        {
            buttonTop.transform.position = Vector3.MoveTowards(buttonTop.transform.position, position, movementSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        onComplete?.Invoke();

        if (invokeOnPressed)
        {
            OnPressed?.Invoke();
        }
    }

    private void MoveBack()
    {
        StartCoroutine(MoveTo(initialPosition, Unlock, false));
    }

    private void Unlock()
    {
        StartCoroutine(WaitAndUnlock());
    }

    private IEnumerator WaitAndUnlock()
    {
        yield return new WaitForSeconds(unlockDelay);
        isPushed = false;
    }
}