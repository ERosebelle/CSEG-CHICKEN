using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FarmPerformanceStatus : MonoBehaviour
{
    // ==========================================
    // GAME REFERENCES
    // ==========================================

    [Header("Game References")]
    public GameTimer gameTimer;
    public AmmoCount ammoCount;
    public AmmoFiredCount ammoFiredCount;
    public FarmExtraRoundManager extraRoundManager;

    // ==========================================
    // NEXT SCENE
    // ==========================================

    [Header("Next Scene")]
    public string gameplayScene = "gameplay";

    // ==========================================
    // PERFORMANCE DISPLAY
    // ==========================================

    [Header("Display")]
    public TextMeshProUGUI timerRemainingText;
    public TextMeshProUGUI ammoRemainingText;
    public TextMeshProUGUI ammoFiredText;

    // ==========================================
    // UI TO TEMPORARILY DISABLE
    // ==========================================

    [Header("UI To Disable While Performance Status Is Active")]
    public GameObject uiToDisable;

    // ==========================================
    // AWAKE
    // ==========================================

    private void Awake()
    {
        if (gameTimer == null)
        {
            gameTimer =
                FindFirstObjectByType<GameTimer>();
        }

        if (ammoCount == null)
        {
            ammoCount =
                FindFirstObjectByType<AmmoCount>();
        }

        if (ammoFiredCount == null)
        {
            ammoFiredCount =
                FindFirstObjectByType<AmmoFiredCount>();
        }

        if (extraRoundManager == null)
        {
            extraRoundManager =
                FindFirstObjectByType<FarmExtraRoundManager>();
        }
    }

    // ==========================================
    // START
    // ==========================================

    private void Start()
    {
        gameObject.SetActive(false);
    }

    // ==========================================
    // SHOW PERFORMANCE
    // ==========================================

    public void ShowFarmPerformanceStatus()
    {
        if (gameTimer == null)
        {
            Debug.LogError(
                "FarmPerformanceStatus: GameTimer missing."
            );

            return;
        }

        // ==========================================
        // TIMER
        // ==========================================

        int minutes =
            Mathf.FloorToInt(
                gameTimer.timeRemaining / 60f
            );

        int seconds =
            Mathf.FloorToInt(
                gameTimer.timeRemaining % 60f
            );

        if (timerRemainingText != null)
        {
            timerRemainingText.text =
                string.Format(
                    "{0}:{1:00}",
                    minutes,
                    seconds
                );
        }

        // ==========================================
        // AMMO
        // ==========================================

        if (ammoRemainingText != null &&
            ammoCount != null)
        {
            ammoRemainingText.text =
                ammoCount
                    .GetAmmoCount()
                    .ToString();
        }

        // ==========================================
        // SHOTS FIRED
        // ==========================================

        if (ammoFiredText != null &&
            ammoFiredCount != null)
        {
            ammoFiredText.text =
                ammoFiredCount
                    .GetAmmoFired()
                    .ToString();
        }

        // ==========================================
        // FREEZE GAME
        // ==========================================

        Time.timeScale = 0f;

        // ==========================================
        // TEMPORARILY DISABLE BLOCKING UI
        // ==========================================

        if (uiToDisable != null)
        {
            uiToDisable.SetActive(false);
        }

        // ==========================================
        // SHOW PERFORMANCE STATUS
        // ==========================================

        gameObject.SetActive(true);

        // ==========================================
        // DEBUG
        // ==========================================

        if (extraRoundManager != null)
        {
            FarmExtraRoundManager.RoundState state =
                extraRoundManager.GetCurrentRound();

            if (state ==
                FarmExtraRoundManager.RoundState.Farm)
            {
                Debug.Log(
                    "PERFORMANCE SCREEN: ROUND 1"
                );
            }
            else if (state ==
                     FarmExtraRoundManager.RoundState.ExtraRoundCompleted)
            {
                Debug.Log(
                    "PERFORMANCE SCREEN: ROUND 2"
                );
            }
        }
    }

    // ==========================================
    // CONTINUE
    // ==========================================

    public void ContinueFarm()
    {
        if (extraRoundManager == null)
        {
            Debug.LogError(
                "FarmPerformanceStatus: FarmExtraRoundManager missing."
            );

            return;
        }

        FarmExtraRoundManager.RoundState state =
            extraRoundManager.GetCurrentRound();

        // ==========================================
        // ROUND 1 COMPLETE
        // ==========================================

        if (state ==
            FarmExtraRoundManager.RoundState.Farm)
        {
            Debug.Log(
                "CONTINUE: ROUND 1"
            );

            Time.timeScale = 1f;

            // Restore temporarily disabled UI
            if (uiToDisable != null)
            {
                uiToDisable.SetActive(true);
            }

            // Go to Extra Area
            extraRoundManager.UnlockExtraArea();

            // Hide Performance Status
            gameObject.SetActive(false);

            return;
        }

        // ==========================================
        // ROUND 2 COMPLETE
        // ==========================================

        if (state ==
            FarmExtraRoundManager.RoundState.ExtraRoundCompleted)
        {
            Debug.Log(
                "CONTINUE: ROUND 2"
            );

            Time.timeScale = 1f;

            // Restore temporarily disabled UI
            if (uiToDisable != null)
            {
                uiToDisable.SetActive(true);
            }

            // Hide Performance Status
            gameObject.SetActive(false);

            // ==========================================
            // DIRECTLY LOAD FINAL GAMEPLAY SCENE
            // ==========================================

            Debug.Log(
                "LOADING GAMEPLAY SCENE: "
                + gameplayScene
            );

            SceneManager.LoadScene(
                gameplayScene
            );

            return;
        }

        // ==========================================
        // INVALID STATE
        // ==========================================

        Debug.LogWarning(
            "FarmPerformanceStatus: Continue pressed in invalid state: "
            + state
        );
    }

    // ==========================================
    // EXIT TO MAIN MENU
    // ==========================================

    public void ExitToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            "MainMenu"
        );
    }
}