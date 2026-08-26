using UnityEngine;

public class PotionDropSpawner : MonoBehaviour
{
    [Header("Potion Prefab")]
    public GameObject potionPrefab;

    [Header("Ground Potion Limit")]
    public int maxGroundPotions = 5;

    [Header("Timed Spawn")]
    public bool enableTimedSpawn = true;

    [Tooltip("Time in seconds between free potion spawns.")]
    public float spawnInterval = 30f;

    [Header("Free Potion Spawn Point")]
    public Transform freePotionSpawnPoint;

    [Header("Enemy Defeat Drop")]
    public bool enableEnemyDrops = true;

    [Tooltip("Minimum number of potions dropped by an enemy.")]
    public int enemyDropMinimum = 2;

    [Tooltip("Maximum number of potions dropped by an enemy.")]
    public int enemyDropMaximum = 3;

    [Tooltip("Random distance around the enemy where dropped potions can appear.")]
    public float enemyDropRadius = 1.5f;

    private float spawnTimer;

    private int currentGroundPotions;


    // ==========================================
    // START
    // ==========================================

    void Start()
    {
        spawnTimer = spawnInterval;

        currentGroundPotions = 0;

        Debug.Log(
            "POTION SPAWNER READY | " +
            "Maximum Ground Potions: " +
            maxGroundPotions
        );
    }


    // ==========================================
    // UPDATE
    // ==========================================

    void Update()
    {
        if (!enableTimedSpawn)
            return;

        if (potionPrefab == null)
            return;

        if (freePotionSpawnPoint == null)
            return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnTimedPotion();

            spawnTimer = spawnInterval;
        }
    }


    // ==========================================
    // TIMED POTION
    // ==========================================

    void SpawnTimedPotion()
    {
        if (currentGroundPotions >= maxGroundPotions)
        {
            Debug.Log(
                "TIMED POTION NOT SPAWNED | " +
                "Ground Potion Count: " +
                currentGroundPotions +
                " / " +
                maxGroundPotions
            );

            return;
        }

        GameObject newPotion =
            Instantiate(
                potionPrefab,
                freePotionSpawnPoint.position,
                freePotionSpawnPoint.rotation
            );

        currentGroundPotions++;

        Debug.Log(
            "TIMED POTION SPAWNED | " +
            "Ground Potions: " +
            currentGroundPotions +
            " / " +
            maxGroundPotions
        );
    }


    // ==========================================
    // ENEMY DEFEAT DROP
    // CALL THIS FROM ENEMY HEALTH
    // ==========================================

    public void SpawnEnemyDrops(Vector3 enemyPosition)
    {
        if (!enableEnemyDrops)
            return;

        if (potionPrefab == null)
        {
            Debug.LogError(
                "PotionDropSpawner: Potion Prefab is NOT assigned!"
            );

            return;
        }

        if (currentGroundPotions >= maxGroundPotions)
        {
            Debug.Log(
                "ENEMY DROP NOT SPAWNED | " +
                "Ground Potion Count already MAX: " +
                currentGroundPotions
            );

            return;
        }

        int dropAmount =
            Random.Range(
                enemyDropMinimum,
                enemyDropMaximum + 1
            );

        int availableSpace =
            maxGroundPotions -
            currentGroundPotions;

        int actualDropAmount =
            Mathf.Min(
                dropAmount,
                availableSpace
            );


        // ==========================================
        // SPAWN POTIONS
        // ==========================================

        for (int i = 0; i < actualDropAmount; i++)
        {
            Vector2 randomOffset =
                Random.insideUnitCircle *
                enemyDropRadius;

            Vector3 spawnPosition =
                enemyPosition +
                new Vector3(
                    randomOffset.x,
                    0f,
                    randomOffset.y
                );

            Instantiate(
                potionPrefab,
                spawnPosition,
                Quaternion.identity
            );

            currentGroundPotions++;
        }


        Debug.Log(
            "ENEMY POTION DROP | " +
            "Dropped: " +
            actualDropAmount +
            " | Ground Potions: " +
            currentGroundPotions +
            " / " +
            maxGroundPotions
        );
    }


    // ==========================================
    // POTION PICKED UP
    // CALL THIS WHEN A POTION IS COLLECTED
    // ==========================================

    public void PotionCollected()
    {
        if (currentGroundPotions <= 0)
            return;

        currentGroundPotions--;

        Debug.Log(
            "POTION COLLECTED | " +
            "Ground Potions: " +
            currentGroundPotions +
            " / " +
            maxGroundPotions
        );
    }


    // ==========================================
    // GET CURRENT GROUND POTIONS
    // ==========================================

    public int GetCurrentGroundPotions()
    {
        return currentGroundPotions;
    }
}