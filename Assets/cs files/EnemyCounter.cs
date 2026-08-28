using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    [Header("Required Defeated Enemies")]
    public int requiredEnemies = 4;

    private int defeatedEnemies = 0;

    private bool allEnemiesDefeated = false;

    // ==========================================
    // ENEMY DEFEATED
    // CALLED BY ENEMY HEALTH
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