using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class LevelManager : MonoBehaviour
{
    private int numPlayersOnExitPad = 0;

    [SerializeField]
    private string levelTitle;

    [SerializeField]
    private string nextLevelSceneName;

    [SerializeField]
    private PlayerController[] players;

    [SerializeField]
    private Image fader;

    [SerializeField]
    private TextMeshProUGUI text;

    private readonly float fadeSpeed = 1.5f;
    private readonly float beforeFadeStartsDelay = 0.8f;
    private float currentFadeAlphaChannel = 0f;

    private readonly float delayBeforeTitleType = 0.8f;
    private readonly float levelTitleTypeCharDelay = 0.08f;
    private readonly float levelTitleKeepTime = 2f;
    private float currentTextFadeAlphaChannel;
    private float textFadeOutSpeed = 0.8f;

    private void Awake()
    {
        currentFadeAlphaChannel = 1f;
        fader.color = new Color(0f, 0f, 0f, currentFadeAlphaChannel);
        DisablePlayerControllers();
    }

    private void Start()
    {
        StartCoroutine(FadeInAndEnableFirstPlayerControllers());
    }

    private IEnumerator FadeInAndEnableFirstPlayerControllers()
    {
        yield return new WaitForEndOfFrame();
        while (fader.color.a > 0f)
        {
            currentFadeAlphaChannel -= fadeSpeed * Time.deltaTime;
            fader.color = new Color(0f, 0f, 0f, currentFadeAlphaChannel);

            yield return new WaitForEndOfFrame();
        }

        players[0].SetActive(true);
        StartCoroutine(TypeLevelTitle());
    }

    private IEnumerator TypeLevelTitle()
    {
        yield return new WaitForSeconds(delayBeforeTitleType);
        foreach (char c in levelTitle)
        {
            text.text += c;
            if (c != ' ')
            {
                yield return new WaitForSeconds(levelTitleTypeCharDelay);
            }
            else 
            {
                yield return new WaitForEndOfFrame();
            }
        }
        yield return new WaitForSeconds(levelTitleKeepTime);

        StartCoroutine(FadeOutTitle());
    }

    private IEnumerator FadeOutTitle()
    {
        currentTextFadeAlphaChannel = text.color.a;
        while (text.color.a > 0f)
        {
            currentTextFadeAlphaChannel -= textFadeOutSpeed * Time.deltaTime;
            text.color = new Color(text.color.r, text.color.g, text.color.b, currentTextFadeAlphaChannel);

            yield return new WaitForEndOfFrame();
        }

        text.text = string.Empty;
    }

    public void AddPlayerOnExitPad()
    {
        numPlayersOnExitPad++;

        if (numPlayersOnExitPad < players.Length)
            return;

        DisablePlayerControllers();
        LoadNextLevel();
    }

    public void RemovePlayerFromExitPad()
    {
        numPlayersOnExitPad--;
    }

    private void DisablePlayerControllers()
    {
        foreach (var player in players)
        {
            player.SetActive(false);
        }
    }

    private void LoadNextLevel()
    {
        StartCoroutine(FadeOutAndLoadScene(nextLevelSceneName));
    }

    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        yield return new WaitForSeconds(beforeFadeStartsDelay);
        while (fader.color.a < 1f)
        {
            currentFadeAlphaChannel += fadeSpeed * Time.deltaTime;
            fader.color = new Color(0f, 0f, 0f, currentFadeAlphaChannel);

            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene(sceneName);
    }

    public void RestartCurrentLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        StartCoroutine(FadeOutAndLoadScene(scene.name));
    }
}
