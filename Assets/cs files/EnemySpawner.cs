using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    public GameObject enemyPrefab;

    [Header("Enemy Spawn Site")]
    public Transform enemySpawnPoint;

    private GameObject spawnedEnemy;
    private EnemyHealth enemyHealth;

    private void Start()
    {
        SpawnEnemy();
    }

    private void Update()
    {
        if (enemyHealth == null)
            return;

        if (enemyHealth.GetCurrentHealth() <= 0f)
        {
            EnemyDefeated();
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null)
            return;

        if (enemySpawnPoint == null)
            return;

        spawnedEnemy =
            Instantiate(
                enemyPrefab,
                enemySpawnPoint.position,
                enemySpawnPoint.rotation
            );

        spawnedEnemy.SetActive(true);

        enemyHealth =
            spawnedEnemy.GetComponent<EnemyHealth>();

        if (enemyHealth == null)
        {
            enemyHealth =
                spawnedEnemy.GetComponentInChildren<EnemyHealth>();
        }
    }

    private void EnemyDefeated()
    {
        enemyHealth = null;
        spawnedEnemy = null;
    }
}