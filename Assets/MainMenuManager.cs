using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Main Menu")]
    public GameObject mainMenuPanel;

    [Header("Tutorial")]
    public GameObject tutorialPanel;

    [Header("Settings")]
    public GameObject settingsPanel;

    // PLAY BUTTON
    public void PlayGame()
    {
        SceneManager.LoadScene("Egagamao, Reyes");
    }

    // TUTORIAL BUTTON
    public void OpenTutorial()
    {
        mainMenuPanel.SetActive(false);
        tutorialPanel.SetActive(true);
    }
    public void CloseTutorial()
    {
        tutorialPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    // SETTINGS BUTTON
    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    // QUIT BUTTON
    public void QuitGame()
    {
        Debug.Log("Quit button clicked.");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}