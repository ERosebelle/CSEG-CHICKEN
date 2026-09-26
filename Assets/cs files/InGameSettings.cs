using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameSettings : MonoBehaviour
{
    public GameObject settingsImage;

    void Start()
    {
        if (settingsImage != null)
            settingsImage.SetActive(false);
    }

    public void ToggleSettings()
    {
        if (settingsImage != null)
        {
            settingsImage.SetActive(
                !settingsImage.activeSelf
            );
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        string currentScene =
            SceneManager.GetActiveScene().name;

        if (currentScene == "tutorialRoom")
        {
            SceneManager.LoadScene("tutorialRoom");
        }
        else
        {
            SceneManager.LoadScene(currentScene);
        }
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }
}