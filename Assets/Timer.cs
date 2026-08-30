using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 300f; // 5 minutes
    public bool timerIsRunning = true;

    public TextMeshProUGUI timerText;

    // Defeated / Game Over UI
    public GameObject defeatedBackground;

    void Start()
    {
        // Make sure Defeated UI is hidden at the start
        if (defeatedBackground != null)
        {
            defeatedBackground.SetActive(false);
        }

        UpdateTimerDisplay();
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;

                if (timeRemaining <= 0)
                {
                    timeRemaining = 0;
                    timerText.color = Color.red;
                    timerIsRunning = false;

                    TimerFinished();
                }
            }

            UpdateTimerDisplay();
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void TimerFinished()
    {
        Debug.Log("TIME'S UP!");

        // Show Defeated / Game Over background
        if (defeatedBackground != null)
        {
            defeatedBackground.SetActive(true);
        }

        // Stop the game
        Time.timeScale = 0f;
    }

    public void StartTimer()
    {
        timerIsRunning = true;
    }

    public void PauseTimer()
    {
        timerIsRunning = false;
    }

    public void ResetTimer(float newTime)
    {
        timeRemaining = newTime;
        timerIsRunning = true;

        timerText.color = Color.white;

        if (defeatedBackground != null)
        {
            defeatedBackground.SetActive(false);
        }

        Time.timeScale = 1f;

        UpdateTimerDisplay();
    }
}