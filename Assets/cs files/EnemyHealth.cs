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

    private float currentHealth;
    private bool isDefeated;

    private bool wasRegenerating;
    private bool wasRegenerationBlocked;

    // ==========================================
    // START
    // ==========================================

    void Start()
    {
        currentHealth = maxHealth;
        isDefeated = false;

        wasRegenerating = false;
        wasRegenerationBlocked = false;

        UpdateHeartModels();

        Debug.Log(
            "ENEMY HEALTH START | " +
            gameObject.name +
            " | HEALTH: " +
            GetHealthPercent().ToString("F2") +
            "%"
        );
    }

    // ==========================================
    // UPDATE
    // ==========================================

    void Update()
    {
        if (isDefeated)
            return;

        CheckRegeneration();
    }

    // ==========================================
    // DAMAGE
    // ==========================================

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

        Debug.Log(
            "ENEMY DAMAGE | " +
            gameObject.name +
            " | HEALTH: " +
            GetHealthPercent().ToString("F2") +
            "%"
        );

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

        // ==========================================
        // FIND ENEMY WEAKNESS
        // ==========================================

        EnemyColorWeakness enemyWeakness =
            GetComponent<EnemyColorWeakness>();

        if (enemyWeakness == null)
        {
            enemyWeakness =
                GetComponentInParent<EnemyColorWeakness>();
        }

        if (enemyWeakness == null)
        {
            Debug.LogWarning(
                "REGENERATION FAILED | EnemyColorWeakness NOT FOUND | " +
                gameObject.name
            );

            return;
        }

        // ==========================================
        // FIND NEARBY OBJECTS
        // ==========================================

        Collider[] nearbyObjects =
            Physics.OverlapSphere(
                transform.position,
                regenerationRadius
            );

        bool weaknessColorFound = false;

        MapColorObject blockingObject = null;

        // ==========================================
        // CHECK EACH OBJECT
        // ==========================================

        foreach (
            Collider nearbyCollider
            in nearbyObjects
        )
        {
            if (nearbyCollider == null)
                continue;

            // ==========================================
            // IGNORE ENEMY
            // ==========================================

            if (
                nearbyCollider.transform == transform ||
                nearbyCollider.transform.IsChildOf(transform)
            )
            {
                continue;
            }

            // ==========================================
            // FIND MAP COLOR OBJECT
            // ==========================================

            MapColorObject mapColorObject =
                nearbyCollider.GetComponent<MapColorObject>();

            if (mapColorObject == null)
            {
                mapColorObject =
                    nearbyCollider.GetComponentInParent<MapColorObject>();
            }

            if (mapColorObject == null)
                continue;

            // ==========================================
            // CHECK COLOR
            // ==========================================

            if (
                enemyWeakness.IsWeaknessColor(
                    mapColorObject.GetCurrentColor()
                )
            )
            {
                weaknessColorFound = true;

                blockingObject =
                    mapColorObject;

                break;
            }
        }

        // ==========================================
        // WEAKNESS COLOR FOUND
        // REGENERATION BLOCKED
        // ==========================================

        if (weaknessColorFound)
        {
            wasRegenerating = false;

            if (!wasRegenerationBlocked)
            {
                Debug.Log(
                    "REGENERATION BLOCKED | " +
                    "Enemy: " +
                    gameObject.name +
                    " | Weakness: " +
                    enemyWeakness.GetWeaknessName() +
                    " | Object: " +
                    blockingObject.gameObject.name +
                    " | Color: " +
                    blockingObject.GetColorName()
                );

                wasRegenerationBlocked = true;
            }

            return;
        }

        // ==========================================
        // NO WEAKNESS COLOR FOUND
        // REGENERATE
        // ==========================================

        wasRegenerationBlocked = false;

        if (!wasRegenerating)
        {
            Debug.Log(
                "REGENERATION STARTED | " +
                "Enemy: " +
                gameObject.name +
                " | Weakness: " +
                enemyWeakness.GetWeaknessName() +
                " | Rate: " +
                regenerationPerSecond +
                " HP/SECOND"
            );

            wasRegenerating = true;
        }

        // ==========================================
        // APPLY REGENERATION
        // ==========================================

        float previousHealth =
            currentHealth;

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

        // ==========================================
        // LOG EVERY 1 HP
        // ==========================================

        if (
            Mathf.FloorToInt(previousHealth) !=
            Mathf.FloorToInt(currentHealth)
        )
        {
            Debug.Log(
                "ENEMY REGENERATING | " +
                gameObject.name +
                " | HEALTH: " +
                GetHealthPercent().ToString("F2") +
                "%"
            );
        }
    }

    // ==========================================
    // HEART MODELS
    // ==========================================

    void UpdateHeartModels()
    {
        if (maxHealth <= 0f)
            return;

        float healthPercent =
            GetHealthPercent() * 100f;

        // ==========================================
        // HEART 1
        // ==========================================

        if (heartModel1 != null)
        {
            heartModel1.SetActive(
                healthPercent > 0f
            );
        }

        // ==========================================
        // HEART 2
        // ==========================================

        if (heartModel2 != null)
        {
            heartModel2.SetActive(
                healthPercent > 33.33f
            );
        }

        // ==========================================
        // HEART 3
        // ==========================================

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
        isDefeated = true;
        currentHealth = 0f;

        UpdateHeartModels();

        Debug.Log(
            "ENEMY DEFEATED | " +
            gameObject.name
        );

        Destroy(gameObject);
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
    // REGENERATION RANGE VISUAL
    // ==========================================

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            regenerationRadius
        );
    }
}