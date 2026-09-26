using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseScreen;

    void Start()
    {
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pauseScreen.SetActive(true);
    }

    public void Unpause()
    {
        Time.timeScale = 1f;
        pauseScreen.SetActive(false);
    }
}