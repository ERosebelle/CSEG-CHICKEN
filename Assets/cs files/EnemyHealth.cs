using UnityEngine;

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

    [Header("Drops")]
    public GameObject mysteryPotion;
    public GameObject healthPotion;
    public GameObject ammoPotion;

    [Header("Drop Spawn Site")]
    public Transform spawnPotion;

    [Header("Enemy Counter")]
    public EnemyCounter enemyCounter;

    private float currentHealth;
    private bool isDefeated;

    private bool wasRegenerating;
    private bool wasRegenerationBlocked;

    void Start()
    {
        currentHealth = maxHealth;
        isDefeated = false;

        wasRegenerating = false;
        wasRegenerationBlocked = false;

        // ==========================================
        // FIND ENEMY COUNTER AUTOMATICALLY
        // ==========================================

        if (enemyCounter == null)
        {
            enemyCounter =
                FindFirstObjectByType<EnemyCounter>();
        }

        UpdateHeartModels();
    }

    void Update()
    {
        if (isDefeated)
            return;

        CheckRegeneration();
    }

    // ==========================================
    // TAKE DAMAGE
    // ==========================================

    public void TakeDamage()
    {
        if (isDefeated)
            return;

        currentHealth -= damagePerHit;

        currentHealth = Mathf.Clamp(
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

    // ==========================================
    // REGENERATION
    // ==========================================

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

        currentHealth = Mathf.Clamp(
            currentHealth,
            0f,
            maxHealth
        );

        UpdateHeartModels();
    }

    // ==========================================
    // UPDATE HEART MODELS
    // ==========================================

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

    // ==========================================
    // DEFEAT
    // ==========================================

    void Defeat()
    {
        if (isDefeated)
            return;

        isDefeated = true;
        currentHealth = 0f;

        UpdateHeartModels();

        // ==========================================
        // SEND DEFEAT TO ENEMY COUNTER
        // ==========================================

        if (enemyCounter == null)
        {
            enemyCounter =
                FindFirstObjectByType<EnemyCounter>();
        }

        if (enemyCounter != null)
        {
            enemyCounter.EnemyDefeated();
        }

        SpawnDrops();

        Destroy(gameObject);
    }

    // ==========================================
    // SPAWN DROPS
    // ==========================================

    void SpawnDrops()
    {
        if (spawnPotion == null)
            return;

        SpawnDrop(mysteryPotion);
        SpawnDrop(healthPotion);
        SpawnDrop(ammoPotion);
    }

    void SpawnDrop(GameObject dropPrefab)
    {
        if (dropPrefab == null)
            return;

        GameObject spawnedDrop =
            Instantiate(
                dropPrefab,
                spawnPotion.position,
                spawnPotion.rotation
            );

        spawnedDrop.SetActive(true);
    }

    // ==========================================
    // GET CURRENT HEALTH
    // ==========================================

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    // ==========================================
    // GET HEALTH PERCENT
    // ==========================================

    public float GetHealthPercent()
    {
        if (maxHealth <= 0f)
            return 0f;

        return currentHealth / maxHealth;
    }

    // ==========================================
    // CHECK DEFEATED
    // ==========================================

    public bool IsDefeated()
    {
        return isDefeated;
    }

    // ==========================================
    // REGENERATION GIZMO
    // ==========================================

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            regenerationRadius
        );
    }
}