using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public UnityEngine.UI.Button[] buttons;

    private void Awake()
    {
        foreach (var button in buttons)
        {
            button.onClick.AddListener(() => SceneManager.LoadScene(button.name));
            if (button.name == "Level_01")
                continue;

            if (PlayerPrefs.HasKey(button.gameObject.name))
            {
                button.interactable = true;
            }
            else 
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = "???";
            }
        }
    }

    private void OnDestroy()
    {
        foreach (var button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }
    }
}
