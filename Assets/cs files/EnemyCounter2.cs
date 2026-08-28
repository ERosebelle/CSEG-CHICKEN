using UnityEngine;

public class EnemyCounter2 : MonoBehaviour
{
    [Header("Enemies")]
    public GameObject enemy1;
    public GameObject enemy2;

    private bool allEnemiesDefeated;

    private void Update()
    {
        if (allEnemiesDefeated)
            return;

        CheckEnemies();
    }

    private void CheckEnemies()
    {
        bool enemy1Dead =
            enemy1 == null || !enemy1.activeInHierarchy;

        bool enemy2Dead =
            enemy2 == null || !enemy2.activeInHierarchy;

        if (
            enemy1Dead &&
            enemy2Dead
        )
        {
            allEnemiesDefeated = true;

            AllEnemiesDefeated();
        }
    }

    private void AllEnemiesDefeated()
    {
        // Both enemies are dead.
    }

    public bool AreAllEnemiesDefeated()
    {
        return allEnemiesDefeated;
    }
}