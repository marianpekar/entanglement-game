using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ExitPad : MonoBehaviour
{
    private readonly float maxLightIntensity = 8f;
    private readonly float lightChangeIntensityStep = 16f;
    private readonly float beforeIntensityStartsToChangeDelay = 0.2f;

    private new Light light;
    
    public UnityEvent OnPlayerEnter;
    public UnityEvent OnPlayerExit;
    public UnityEvent OnPlayerEnterCoroutineFinished;
    public UnityEvent OnPlayerExitCoroutineFinished;

    Coroutine playerEnterCoroutine = null;
    Coroutine playerExitCoroutine = null;

    private void Awake()
    {
        light = GetComponentInChildren<Light>();
    }

    private IEnumerator PlayerEnter()
    {
        if (playerExitCoroutine != null)
        {
            StopCoroutine(playerExitCoroutine);
        }

        yield return new WaitForSeconds(beforeIntensityStartsToChangeDelay);
        while (light.intensity < maxLightIntensity)
        {
            light.intensity += lightChangeIntensityStep * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        playerEnterCoroutine = null;
        OnPlayerEnterCoroutineFinished?.Invoke();
    }

    private IEnumerator PlayerExit()
    {
        if (playerEnterCoroutine != null)
        {
            StopCoroutine(playerEnterCoroutine);
        }

        yield return new WaitForSeconds(beforeIntensityStartsToChangeDelay);
        while (light.intensity > 0)
        {
            light.intensity -= lightChangeIntensityStep * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        playerExitCoroutine = null;
        OnPlayerExitCoroutineFinished?.Invoke();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            playerEnterCoroutine = StartCoroutine(PlayerEnter());
            OnPlayerEnter?.Invoke();
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            playerExitCoroutine = StartCoroutine(PlayerExit());
            OnPlayerExit?.Invoke();
        }
    }
}
