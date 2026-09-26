using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;

    [Tooltip("Health removed when hit by the correct color.")]
    public float damagePerHit = 20f;

    [Header("Heart Models")]
    public GameObject heartModel1;
    public GameObject heartModel2;
    public GameObject heartModel3;

    [Header("Regeneration")]
    public float regenerationPerSecond = 5f;
    public float regenerationRadius = 10f;

    // =========================================================
    // DROPS
    // =========================================================

    [Header("Drops")]

    [Tooltip("Mystery Potion prefab.")]
    public GameObject mysteryPotion;

    [Min(0)]
    [Tooltip("Number of Mystery Potions dropped.")]
    public int mysteryPotionAmount = 1;

    [Tooltip("Health Potion prefab.")]
    public GameObject healthPotion;

    [Min(0)]
    [Tooltip("Number of Health Potions dropped.")]
    public int healthPotionAmount = 1;

    [Tooltip("Ammo Potion prefab.")]
    public GameObject ammoPotion;

    [Min(0)]
    [Tooltip("Number of Ammo Potions dropped.")]
    public int ammoPotionAmount = 1;

    // =========================================================
    // DROP SPAWN SITE
    // =========================================================

    [Header("Drop Spawn Site")]
    public Transform spawnPotion;

    // =========================================================
    // ENEMY COUNTER
    // =========================================================

    [Header("Enemy Counter")]

    public bool isRound2Enemy = false;

    public EnemyCounter enemyCounter;
    public EnemyCounter2 enemyCounter2;

    private float currentHealth;
    private bool isDefeated;

    private bool wasRegenerating;
    private bool wasRegenerationBlocked;

    // =========================================================
    // TUTORIAL CHECK
    // =========================================================

    private bool IsTutorialScene()
    {
        return SceneManager.GetActiveScene().name == "Tutorial Scene";
    }

    // =========================================================
    // START
    // =========================================================

    void Start()
    {
        currentHealth = maxHealth;
        isDefeated = false;

        wasRegenerating = false;
        wasRegenerationBlocked = false;

        // ==========================================
        // DO NOT LOOK FOR ENEMY COUNTER IN TUTORIAL
        // ==========================================

        if (!IsTutorialScene())
        {
            // ==========================================
            // FIND CORRECT ENEMY COUNTER AUTOMATICALLY
            // INCLUDING INACTIVE OBJECTS
            // ==========================================

            if (isRound2Enemy)
            {
                if (enemyCounter2 == null)
                {
                    enemyCounter2 =
                        FindFirstObjectByType<EnemyCounter2>(
                            FindObjectsInactive.Include
                        );
                }
            }
            else
            {
                if (enemyCounter == null)
                {
                    enemyCounter =
                        FindFirstObjectByType<EnemyCounter>(
                            FindObjectsInactive.Include
                        );
                }
            }
        }

        UpdateHeartModels();
    }

    // =========================================================
    // UPDATE
    // =========================================================

    void Update()
    {
        if (isDefeated)
            return;

        CheckRegeneration();
    }

    // =========================================================
    // TAKE DAMAGE
    // =========================================================

    public void TakeDamage()
    {
        if (isDefeated)
            return;

        currentHealth -= damagePerHit;

        currentHealth =
            Mathf.Clamp(
                currentHealth,
                0f,
                maxHealth
            );

        UpdateHeartModels();

        if (currentHealth <= 0f)
        {
            Defeat();
        }
    }

    // =========================================================
    // REGENERATION
    // =========================================================

    void CheckRegeneration()
    {
        if (currentHealth >= maxHealth)
        {
            wasRegenerating = false;
            wasRegenerationBlocked = false;

            return;
        }

        EnemyColorWeakness enemyWeakness =
            GetComponent<EnemyColorWeakness>();

        if (enemyWeakness == null)
        {
            enemyWeakness =
                GetComponentInParent<EnemyColorWeakness>();
        }

        if (enemyWeakness == null)
            return;

        Collider[] nearbyObjects =
            Physics.OverlapSphere(
                transform.position,
                regenerationRadius
            );

        bool weaknessColorFound = false;

        foreach (Collider nearbyCollider in nearbyObjects)
        {
            if (nearbyCollider == null)
                continue;

            if (
                nearbyCollider.transform == transform ||
                nearbyCollider.transform.IsChildOf(transform)
            )
            {
                continue;
            }

            MapColorObject mapColorObject =
                nearbyCollider.GetComponent<MapColorObject>();

            if (mapColorObject == null)
            {
                mapColorObject =
                    nearbyCollider.GetComponentInParent<MapColorObject>();
            }

            if (mapColorObject == null)
                continue;

            if (
                enemyWeakness.IsWeaknessColor(
                    mapColorObject.GetCurrentColor()
                )
            )
            {
                weaknessColorFound = true;
                break;
            }
        }

        if (weaknessColorFound)
        {
            wasRegenerating = false;
            wasRegenerationBlocked = true;

            return;
        }

        wasRegenerationBlocked = false;
        wasRegenerating = true;

        currentHealth +=
            regenerationPerSecond *
            Time.deltaTime;

        currentHealth =
            Mathf.Clamp(
                currentHealth,
                0f,
                maxHealth
            );

        UpdateHeartModels();
    }

    // =========================================================
    // UPDATE HEART MODELS
    // =========================================================

    void UpdateHeartModels()
    {
        if (maxHealth <= 0f)
            return;

        float healthPercent =
            GetHealthPercent() * 100f;

        if (heartModel1 != null)
        {
            heartModel1.SetActive(
                healthPercent > 0f
            );
        }

        if (heartModel2 != null)
        {
            heartModel2.SetActive(
                healthPercent > 33.33f
            );
        }

        if (heartModel3 != null)
        {
            heartModel3.SetActive(
                healthPercent > 66.67f
            );
        }
    }

    // =========================================================
    // TUTORIAL TASK 8
    // FORCE DEFEAT
    // =========================================================

    public void DefeatFromTutorial()
    {
        // Only works inside Tutorial Scene
        if (!IsTutorialScene())
            return;

        if (isDefeated)
            return;

        Debug.Log(
            "TUTORIAL TASK 8: ENEMY FORCED TO DEFEAT AT PLAYER 50% HEALTH."
        );

        Defeat();
    }

    // =========================================================
    // DEFEAT
    // =========================================================

    void Defeat()
    {
        if (isDefeated)
            return;

        isDefeated = true;
        currentHealth = 0f;

        UpdateHeartModels();

        // ==========================================
        // TUTORIAL SCENE
        // ==========================================

        // Tutorial enemies do not use EnemyCounter.
        if (IsTutorialScene())
        {
            SpawnDrops();

            Destroy(gameObject);

            return;
        }

        // ==========================================
        // NORMAL GAMEPLAY
        // ==========================================

        if (isRound2Enemy)
        {
            if (enemyCounter2 == null)
            {
                enemyCounter2 =
                    FindFirstObjectByType<EnemyCounter2>(
                        FindObjectsInactive.Include
                    );
            }

            if (enemyCounter2 != null)
            {
                enemyCounter2.EnemyDefeated();
            }
            else
            {
                Debug.LogError(
                    "EnemyHealth: EnemyCounter2 NOT FOUND!"
                );
            }
        }
        else
        {
            if (enemyCounter == null)
            {
                enemyCounter =
                    FindFirstObjectByType<EnemyCounter>(
                        FindObjectsInactive.Include
                    );
            }

            if (enemyCounter != null)
            {
                enemyCounter.EnemyDefeated();
            }
            else
            {
                Debug.LogError(
                    "EnemyHealth: EnemyCounter NOT FOUND!"
                );
            }
        }

        // ==========================================
        // SPAWN DROPS
        // ==========================================

        SpawnDrops();

        // ==========================================
        // DESTROY ENEMY
        // ==========================================

        Destroy(gameObject);
    }

    // =========================================================
    // SPAWN DROPS
    // =========================================================

    void SpawnDrops()
    {
        if (spawnPotion == null)
        {
            Debug.LogWarning(
                "EnemyHealth: Drop Spawn Site is not assigned."
            );

            return;
        }

        // Mystery Potion
        SpawnDropsAmount(
            mysteryPotion,
            mysteryPotionAmount
        );

        // Health Potion
        SpawnDropsAmount(
            healthPotion,
            healthPotionAmount
        );

        // Ammo Potion
        SpawnDropsAmount(
            ammoPotion,
            ammoPotionAmount
        );
    }

    // =========================================================
    // SPAWN MULTIPLE DROPS
    // =========================================================

    void SpawnDropsAmount(
        GameObject dropPrefab,
        int amount
    )
    {
        if (dropPrefab == null)
            return;

        if (amount <= 0)
            return;

        for (int i = 0; i < amount; i++)
        {
            GameObject spawnedDrop =
                Instantiate(
                    dropPrefab,
                    spawnPotion.position,
                    spawnPotion.rotation
                );

            spawnedDrop.SetActive(true);
        }
    }

    // =========================================================
    // GET CURRENT HEALTH
    // =========================================================

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    // =========================================================
    // GET HEALTH PERCENT
    // =========================================================

    public float GetHealthPercent()
    {
        if (maxHealth <= 0f)
            return 0f;

        return currentHealth / maxHealth;
    }

    // =========================================================
    // CHECK DEFEATED
    // =========================================================

    public bool IsDefeated()
    {
        return isDefeated;
    }

    // =========================================================
    // REGENERATION GIZMO
    // =========================================================

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            regenerationRadius
        );
    }
}