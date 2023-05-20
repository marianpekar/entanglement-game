using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DimensionSwitcher : MonoBehaviour
{
    [SerializeField]
    private Camera cameraAlpha;

    [SerializeField]
    private Camera cameraBeta;

    [SerializeField]
    private PlayerController playerAlpha;

    [SerializeField]
    private PlayerController playerBeta;

    private bool isAlpha;
    private bool isLocked;

    [SerializeField]
    private Image fader;

    private readonly float fadeSpeed = 3f;
    private float currentFadeAlphaChannel = 0f;
    private readonly float inbetweenFadeDelay = 0.2f;

    private void Awake()
    {
        cameraBeta.gameObject.SetActive(false);
        playerBeta.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isLocked)
        {
            isLocked = true;
            Switch();
        }
    }

    private void Switch()
    {
        isAlpha = !isAlpha;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        while (fader.color.a < 1f)
        {
            currentFadeAlphaChannel += fadeSpeed * Time.deltaTime;
            fader.color = new Color(0f, 0f, 0f, currentFadeAlphaChannel);

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(inbetweenFadeDelay);

        SwitchCamera();
        SwitchPlayers();
        StartCoroutine(FadeIn());
    }

    private void SwitchCamera()
    {
        cameraAlpha.gameObject.SetActive(!isAlpha);
        cameraBeta.gameObject.SetActive(isAlpha);
    }

    private void SwitchPlayers()
    {
        playerAlpha.SetActive(!isAlpha);
        playerBeta.SetActive(isAlpha);
    }

    private IEnumerator FadeIn()
    {
        while (fader.color.a >= 0f)
        {
            currentFadeAlphaChannel -= fadeSpeed * Time.deltaTime;
            fader.color = new Color(0f, 0f, 0f, currentFadeAlphaChannel);
            yield return new WaitForEndOfFrame();
        }

        isLocked = false;
    }
}
