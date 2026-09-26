using UnityEngine;

public class EnemyRoundManager : MonoBehaviour
{
    [Header("Round 1")]
    public EnemyCounter enemyCounter;

    [Header("Round 2")]
    public EnemyCounter2 enemyCounter2;

    [Header("Round 2 Map Entrance")]
    public GameObject invisibleWall;
    public GameObject mountain;

    [Header("Player")]
    public PlayerMusic playerMusic;

    [Header("Performance Status")]
    public PerformanceStatus performanceStatus;

    private bool round1Completed = false;
    private bool round2Activated = false;
    private bool round2Completed = false;

    private void Start()
    {
        round1Completed = false;
        round2Activated = false;
        round2Completed = false;

        if (enemyCounter2 != null)
        {
            enemyCounter2.gameObject.SetActive(false);
        }

        if (playerMusic == null)
        {
            playerMusic = FindFirstObjectByType<PlayerMusic>(
                FindObjectsInactive.Include
            );
        }

        if (performanceStatus == null)
        {
            performanceStatus = FindFirstObjectByType<PerformanceStatus>(
                FindObjectsInactive.Include
            );
        }
    }

    private void Update()
    {
        // ROUND 1
        if (!round1Completed)
        {
            if (enemyCounter != null &&
                enemyCounter.AreAllEnemiesDefeated())
            {
                round1Completed = true;

                Debug.Log("ROUND 1 COMPLETED!");

                if (performanceStatus != null)
                {
                    performanceStatus.ShowPerformanceStatus();
                }
                else
                {
                    Debug.LogError("PerformanceStatus NOT FOUND!");
                }
            }

            return;
        }

        // ROUND 2
        if (round2Activated && !round2Completed)
        {
            if (enemyCounter2 != null)
            {
                if (enemyCounter2.AreAllEnemiesDefeated())
                {
                    round2Completed = true;

                    Debug.Log("ROUND 2 COMPLETED!");

                    if (performanceStatus != null)
                    {
                        performanceStatus.ShowPerformanceStatus();
                    }
                    else
                    {
                        Debug.LogError("PerformanceStatus NOT FOUND!");
                    }
                }
            }
            else
            {
                Debug.LogError("EnemyCounter2 is NOT assigned!");
            }
        }
    }

    public void ActivateRound2()
    {
        if (round2Activated)
            return;

        round2Activated = true;

        Debug.Log("ROUND 2 ACTIVATED!");

        if (invisibleWall != null)
        {
            invisibleWall.SetActive(false);
        }

        if (mountain != null)
        {
            mountain.SetActive(false);
        }

        if (enemyCounter2 != null)
        {
            enemyCounter2.gameObject.SetActive(true);
        }

        if (playerMusic != null)
        {
            playerMusic.ChangeToRound2Music();
        }
    }

    public bool IsRound2Activated()
    {
        return round2Activated;
    }

    public bool IsRound2Completed()
    {
        return round2Completed;
    }
}