using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatScreen : MonoBehaviour
{
    public GameObject defeatScreen;
    public PlayerHealth playerHealth;
    public GameTimer gameTimer;
    public EnemyCounter enemyCounter;
    public EnemyCounter2 enemyCounter2;

    private bool defeatShown = false;

    void Start()
    {
        Time.timeScale = 1f;
        defeatScreen.SetActive(false);
    }

    void Update()
    {
        if (defeatShown)
            return;

        // Player dies
        if (playerHealth != null &&
            playerHealth.GetCurrentHealth() <= 0f)
        {
            ShowDefeatScreen();
            return;
        }

        // Timer reaches 0 while enemies are still alive
        if (gameTimer != null &&
            gameTimer.timeRemaining <= 0f)
        {
            bool enemiesStillAlive = false;

            if (enemyCounter2 != null &&
                enemyCounter2.gameObject.activeInHierarchy)
            {
                enemiesStillAlive =
                    !enemyCounter2.AreAllEnemiesDefeated();
            }
            else if (enemyCounter != null)
            {
                enemiesStillAlive =
                    !enemyCounter.AreAllEnemiesDefeated();
            }

            if (enemiesStillAlive)
            {
                ShowDefeatScreen();
            }
        }
    }

    public void ShowDefeatScreen()
    {
        if (defeatShown)
            return;

        defeatShown = true;

        Time.timeScale = 0f;

        defeatScreen.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        // Restart whatever scene the player is currently in
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }
}