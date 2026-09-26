using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PerformanceStatus : MonoBehaviour
{
    [Header("Game References")]
    public GameTimer gameTimer;
    public AmmoCount ammoCount;
    public EnemyRoundManager enemyRoundManager;

    [Header("Display")]
    public TextMeshProUGUI timerRemainingText;
    public TextMeshProUGUI ammoRemainingText;

    private void Awake()
    {
        if (gameTimer == null)
        {
            gameTimer = FindFirstObjectByType<GameTimer>();
        }

        if (ammoCount == null)
        {
            ammoCount = FindFirstObjectByType<AmmoCount>();
        }

        if (enemyRoundManager == null)
        {
            enemyRoundManager = FindFirstObjectByType<EnemyRoundManager>();
        }
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowPerformanceStatus()
    {
        Debug.Log("PERFORMANCE STATUS: SHOWING");

        if (gameTimer == null)
        {
            Debug.LogError("GameTimer is missing!");
            return;
        }

        if (ammoCount == null)
        {
            Debug.LogError("AmmoCount is missing!");
            return;
        }

        int minutes = Mathf.FloorToInt(gameTimer.timeRemaining / 60);
        int seconds = Mathf.FloorToInt(gameTimer.timeRemaining % 60);

        if (timerRemainingText != null)
        {
            timerRemainingText.text =
                string.Format("{0}:{1:00}", minutes, seconds);
        }

        if (ammoRemainingText != null)
        {
            ammoRemainingText.text =
                ammoCount.GetAmmoCount().ToString();
        }

        Time.timeScale = 0f;
        gameObject.SetActive(true);
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;

        if (enemyRoundManager == null)
        {
            enemyRoundManager =
                FindFirstObjectByType<EnemyRoundManager>();
        }

        if (enemyRoundManager != null &&
            enemyRoundManager.IsRound2Completed())
        {
            SceneManager.LoadScene("Credits");
            return;
        }

        if (enemyRoundManager != null)
        {
            enemyRoundManager.ActivateRound2();
        }

        gameObject.SetActive(false);
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}