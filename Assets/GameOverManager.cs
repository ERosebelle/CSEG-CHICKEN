using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public GameObject defeatedUI;

    public void Defeated()
    {
        if (defeatedUI != null)
        {
            defeatedUI.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void BackToHome()
    {
        Time.timeScale = 1f;
        
        // Temporary: reload current game
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("MainMenu");
    }
}