using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 300f;
    public TextMeshProUGUI timerText;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "FarmRoom")
        {
            timeRemaining = 300f;
        }
    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
        }
        else
        {
            timeRemaining = 0;
        }

        UpdateTimerDisplay();
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);

        if (timerText != null)
        {
            timerText.text =
                string.Format(
                    "{0}:{1:00}",
                    minutes,
                    seconds
                );
        }
    }

    public void ResetTimer()
    {
        timeRemaining = 300f;
        UpdateTimerDisplay();
    }
}