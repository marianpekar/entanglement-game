using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private int numPlayersOnExitPad = 0;

    [SerializeField]
    private string nextLevelSceneName;

    [SerializeField]
    private PlayerController[] players;

    [SerializeField]
    private Image fader;

    private readonly float fadeSpeed = 1.5f;
    private readonly float beforeFadeStartsDelay = 0.8f;
    private float currentFadeAlphaChannel = 0f;

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
        StartCoroutine(FadeOutAndLoadScene());
    }

    private IEnumerator FadeOutAndLoadScene()
    {
        yield return new WaitForSeconds(beforeFadeStartsDelay);
        while (fader.color.a < 1f)
        {
            currentFadeAlphaChannel += fadeSpeed * Time.deltaTime;
            fader.color = new Color(0f, 0f, 0f, currentFadeAlphaChannel);

            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene(nextLevelSceneName);
    }
}
