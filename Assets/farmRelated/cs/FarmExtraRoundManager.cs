using UnityEngine;
using UnityEngine.SceneManagement;

public class FarmExtraRoundManager : MonoBehaviour
{
    public enum RoundState
    {
        Farm,
        ExtraAreaUnlocked,
        ExtraRound,
        ExtraRoundCompleted
    }

    [Header("Current Round")]
    [SerializeField]
    private RoundState currentRound = RoundState.Farm;

    [Header("Performance Status")]
    public FarmPerformanceStatus farmPerformanceStatus;

    [Header("SHARED ROUND UI")]
    public GameObject timerUI;
    public GameTimer gameTimer;
    public GameObject objectiveUI;
    public GameObject objectiveExplanation;
    public GameObject cubeCountText;

    [Header("ROUND 2 - TIME")]
    public float extraRoundTime = 300f;

    [Header("Music Manager")]
    public FarmMusicManager musicManager;

    [Header("Cube Spawner")]
    public CubeSpawner cubeSpawner;

    [Header("Round 2 Cube Settings")]
    public int extraRoundRequiredCubes = 10;
    public float extraRoundCubeSpawnInterval = 5f;

    [Header("Ammo Spawner")]
    public AmmoSpawner ammoSpawner;

    [Header("Round 2 Ammo Settings")]
    public float extraRoundAmmoSpawnInterval = 30f;

    [Header("Extra Round Map")]
    public GameObject extraRoundMap;

    [Header("Entrance Wall")]
    public GameObject entranceWall;

    [Header("Extra Round Detection Wall")]
    public GameObject extraRoundWall;
    public Collider extraRoundWallCollider;

    private bool farmRoundCompleted = false;

    private void Start()
    {
        currentRound = RoundState.Farm;
        farmRoundCompleted = false;

        SetRoundUI(true);

        if (gameTimer != null)
        {
            gameTimer.timeRemaining = 300f;
            gameTimer.enabled = true;
        }

        if (extraRoundMap != null)
            extraRoundMap.SetActive(false);

        if (entranceWall != null)
            entranceWall.SetActive(true);

        if (extraRoundWall != null)
            extraRoundWall.SetActive(false);

        if (extraRoundWallCollider != null)
            extraRoundWallCollider.isTrigger = true;
    }

    private void SetRoundUI(bool active)
    {
        if (timerUI != null)
            timerUI.SetActive(active);

        if (gameTimer != null)
            gameTimer.enabled = active;

        if (objectiveUI != null)
            objectiveUI.SetActive(active);

        if (objectiveExplanation != null)
            objectiveExplanation.SetActive(active);

        if (cubeCountText != null)
            cubeCountText.SetActive(active);
    }

    public bool IsFarmRound()
    {
        return currentRound == RoundState.Farm;
    }

    public void NotifyRoundCompleted(bool taskCompleted)
    {
        if (!taskCompleted)
            return;

        if (currentRound == RoundState.Farm)
        {
            farmRoundCompleted = true;

            if (farmPerformanceStatus != null)
                farmPerformanceStatus.ShowFarmPerformanceStatus();

            return;
        }

        if (currentRound == RoundState.ExtraRound)
        {
            currentRound =
                RoundState.ExtraRoundCompleted;

            if (farmPerformanceStatus != null)
                farmPerformanceStatus.ShowFarmPerformanceStatus();

            return;
        }
    }

    public void UnlockExtraArea()
    {
        if (currentRound != RoundState.Farm)
            return;

        if (!farmRoundCompleted)
            return;

        currentRound =
            RoundState.ExtraAreaUnlocked;

        SetRoundUI(false);

        if (extraRoundMap != null)
            extraRoundMap.SetActive(true);

        if (entranceWall != null)
            entranceWall.SetActive(false);

        if (extraRoundWall != null)
            extraRoundWall.SetActive(true);

        if (extraRoundWallCollider != null)
            extraRoundWallCollider.isTrigger = true;
    }

    public void StartExtraRound()
    {
        if (currentRound !=
            RoundState.ExtraAreaUnlocked)
            return;

        currentRound =
            RoundState.ExtraRound;

        if (extraRoundWallCollider != null)
            extraRoundWallCollider.isTrigger = false;

        if (gameTimer != null)
        {
            gameTimer.timeRemaining =
                extraRoundTime;

            gameTimer.enabled = true;
        }

        SetRoundUI(true);

        if (musicManager != null)
            musicManager.PlayRound2Music();

        if (cubeSpawner != null)
        {
            cubeSpawner.StartExtraRound(
                extraRoundRequiredCubes,
                extraRoundCubeSpawnInterval
            );
        }

        if (ammoSpawner != null)
        {
            ammoSpawner.StartExtraRound(
                extraRoundAmmoSpawnInterval
            );
        }
    }

    public bool IsExtraRoundStarted()
    {
        return currentRound ==
                   RoundState.ExtraRound ||
               currentRound ==
                   RoundState.ExtraRoundCompleted;
    }

    public bool IsExtraRoundCompleted()
    {
        return currentRound ==
               RoundState.ExtraRoundCompleted;
    }

    public bool IsFarmRoundCompleted()
    {
        return farmRoundCompleted;
    }

    public RoundState GetCurrentRound()
    {
        return currentRound;
    }
}