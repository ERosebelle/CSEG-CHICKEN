using UnityEngine;

public class EnemyCounter2 : MonoBehaviour
{
    [Header("Required Defeated Enemies")]
    public int requiredEnemies = 2;

    private int defeatedEnemies = 0;
    private bool allEnemiesDefeated = false;

    private int startingEnemyCount = 0;

    // ==========================================
    // START
    // ==========================================

    private void Start()
    {
        EnemyHealth[] enemies =
            FindObjectsByType<EnemyHealth>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        foreach (EnemyHealth enemy in enemies)
        {
            if (enemy != null && enemy.isRound2Enemy)
            {
                startingEnemyCount++;
            }
        }

        Debug.Log(
            "ROUND 2 STARTING ENEMIES: " + startingEnemyCount
        );

        if (startingEnemyCount == 0)
        {
            Debug.LogWarning(
                "EnemyCounter2 found NO Round 2 enemies!"
            );
        }
    }

    // ==========================================
    // UPDATE
    // ==========================================

    private void Update()
    {
        if (allEnemiesDefeated)
            return;

        EnemyHealth[] enemies =
            FindObjectsByType<EnemyHealth>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        int aliveRound2Enemies = 0;

        foreach (EnemyHealth enemy in enemies)
        {
            if (enemy != null && enemy.isRound2Enemy)
            {
                aliveRound2Enemies++;
            }
        }

        defeatedEnemies =
            startingEnemyCount - aliveRound2Enemies;

        if (defeatedEnemies >= requiredEnemies)
        {
            defeatedEnemies = requiredEnemies;
            allEnemiesDefeated = true;

            Debug.Log("ROUND 2 ENEMIES BOTH DEFEATED!");
        }
    }

    // ==========================================
    // OLD METHOD
    // KEPT FOR COMPATIBILITY WITH ENEMY HEALTH
    // ==========================================

    public void EnemyDefeated()
    {
        if (allEnemiesDefeated)
            return;

        defeatedEnemies++;

        if (defeatedEnemies >= requiredEnemies)
        {
            defeatedEnemies = requiredEnemies;
            allEnemiesDefeated = true;

            Debug.Log("ROUND 2 ENEMIES BOTH DEFEATED!");
        }
    }

    // ==========================================
    // CHECK
    // ==========================================

    public bool AreAllEnemiesDefeated()
    {
        return allEnemiesDefeated;
    }

    // ==========================================
    // GET DEFEATED COUNT
    // ==========================================

    public int GetDefeatedEnemies()
    {
        return defeatedEnemies;
    }
}