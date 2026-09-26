using UnityEngine;

public class TutorialEnemyAI : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Enemy Health")]
    public EnemyHealth enemyHealth;

    [Header("Tutorial")]
    public TutorialController tutorialController;

    [Header("Detection")]
    public float detectionRange = 14f;

    [Header("Hit Detection")]
    public float hitDetectionDuration = 5f;

    [Header("Enemy Bullets")]
    public int startingBulletCount = 0;

    [Tooltip("Current number of enemy bullets available.")]
    public int enemyBulletCount;

    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Retreat")]
    public float retreatRange = 4f;
    public float retreatSpeed = 3f;

    [Header("Rotation")]
    public float rotationSpeed = 8f;

    [Header("Return")]
    public float returnSpeed = 3f;
    public float returnDistance = 0.1f;

    private Vector3 originalPosition;

    private float hitDetectionTimer;
    private bool hitDetectionActive;

    private bool tutorialEnemyActive = false;
    private bool enemyDefeatedDetected = false;

    // Only starts checking health after the enemy has actually been hit
    private bool enemyHasBeenHit = false;

    // =========================================================
    // START
    // =========================================================

    void Start()
    {
        originalPosition = transform.position;

        hitDetectionTimer = 0f;
        hitDetectionActive = false;

        enemyBulletCount = startingBulletCount;

        // Find EnemyHealth on this GameObject
        if (enemyHealth == null)
        {
            enemyHealth = GetComponent<EnemyHealth>();
        }

        // Also check parent
        if (enemyHealth == null)
        {
            enemyHealth = GetComponentInParent<EnemyHealth>();
        }

        // Find TutorialController
        if (tutorialController == null)
        {
            tutorialController =
                FindFirstObjectByType<TutorialController>(
                    FindObjectsInactive.Include
                );
        }

        tutorialEnemyActive = true;

        Debug.Log(
            "TUTORIAL ENEMY 1 ACTIVATED. Waiting for player to shoot."
        );
    }

    // =========================================================
    // UPDATE
    // =========================================================

    void Update()
    {
        // IMPORTANT:
        // Do NOT check health until the enemy has actually
        // been hit by the player.
        if (enemyHasBeenHit)
        {
            CheckEnemyHealth();
        }

        if (enemyDefeatedDetected)
            return;

        if (player == null)
            return;

        // =====================================================
        // HIT DETECTION TIMER
        // =====================================================

        if (hitDetectionActive)
        {
            hitDetectionTimer -= Time.deltaTime;

            if (hitDetectionTimer <= 0f)
            {
                hitDetectionTimer = 0f;
                hitDetectionActive = false;
            }
        }

        // =====================================================
        // DISTANCE TO PLAYER
        // =====================================================

        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        // =====================================================
        // PLAYER DETECTION
        // =====================================================

        bool playerDetected =
            distance <= detectionRange;

        // If enemy was hit, force detection
        if (hitDetectionActive)
        {
            playerDetected = true;
        }

        // =====================================================
        // PLAYER NOT DETECTED
        // =====================================================

        if (!playerDetected)
        {
            ReturnToOriginalPosition();
            return;
        }

        // =====================================================
        // PLAYER DETECTED
        // =====================================================

        if (distance <= retreatRange)
        {
            RetreatFromPlayer();
        }
        else
        {
            ChasePlayer();
        }
    }

    // =========================================================
    // CHECK ENEMY HEALTH
    // =========================================================

    void CheckEnemyHealth()
    {
        if (enemyDefeatedDetected)
            return;

        if (enemyHealth == null)
            return;

        // Only check after the enemy has actually been hit
        if (enemyHealth.GetCurrentHealth() <= 0f)
        {
            EnemyDefeated();
        }
    }

    // =========================================================
    // ENEMY DEFEATED
    // =========================================================

    void EnemyDefeated()
    {
        if (enemyDefeatedDetected)
            return;

        enemyDefeatedDetected = true;

        Debug.Log(
            "TUTORIAL ENEMY 1 DEFEATED."
        );

        if (tutorialController == null)
        {
            tutorialController =
                FindFirstObjectByType<TutorialController>(
                    FindObjectsInactive.Include
                );
        }

        if (tutorialController != null)
        {
            tutorialController.Enemy1Defeated();
        }
        else
        {
            Debug.LogError(
                "TutorialEnemyAI: TutorialController NOT FOUND!"
            );
        }
    }

    // =========================================================
    // ENEMY HIT
    // =========================================================

    public void OnEnemyHit()
    {
        // The player has actually hit the enemy.
        // Health checking is now allowed.
        enemyHasBeenHit = true;

        hitDetectionActive = true;

        hitDetectionTimer =
            hitDetectionDuration;

        // Check immediately in case this shot killed the enemy.
        if (enemyHealth != null)
        {
            if (enemyHealth.GetCurrentHealth() <= 0f)
            {
                EnemyDefeated();
            }
        }
    }

    // =========================================================
    // ADD ENEMY BULLET
    // =========================================================

    public void AddEnemyBullet()
    {
        enemyBulletCount++;
    }

    // =========================================================
    // GET ENEMY BULLET COUNT
    // =========================================================

    public int GetEnemyBulletCount()
    {
        return enemyBulletCount;
    }

    // =========================================================
    // USE ENEMY BULLET
    // =========================================================

    public bool UseEnemyBullet()
    {
        if (enemyBulletCount <= 0)
        {
            return false;
        }

        enemyBulletCount--;

        return true;
    }

    // =========================================================
    // CHASE PLAYER
    // =========================================================

    void ChasePlayer()
    {
        Vector3 direction =
            player.position -
            transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.01f)
            return;

        direction.Normalize();

        transform.position +=
            direction *
            moveSpeed *
            Time.deltaTime;

        RotateTowards(direction);
    }

    // =========================================================
    // RETREAT FROM PLAYER
    // =========================================================

    void RetreatFromPlayer()
    {
        Vector3 direction =
            transform.position -
            player.position;

        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.01f)
            return;

        direction.Normalize();

        transform.position +=
            direction *
            retreatSpeed *
            Time.deltaTime;

        RotateTowards(direction);
    }

    // =========================================================
    // RETURN TO ORIGINAL POSITION
    // =========================================================

    void ReturnToOriginalPosition()
    {
        Vector3 direction =
            originalPosition -
            transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude <=
            returnDistance *
            returnDistance)
        {
            transform.position =
                new Vector3(
                    originalPosition.x,
                    transform.position.y,
                    originalPosition.z
                );

            return;
        }

        direction.Normalize();

        transform.position +=
            direction *
            returnSpeed *
            Time.deltaTime;

        RotateTowards(direction);
    }

    // =========================================================
    // ROTATION
    // =========================================================

    void RotateTowards(Vector3 direction)
    {
        if (direction.sqrMagnitude <= 0.01f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(direction);

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed *
                Time.deltaTime
            );
    }

    // =========================================================
    // DESTROY FALLBACK
    // =========================================================

    private void OnDestroy()
    {
        if (!tutorialEnemyActive)
            return;

        if (enemyDefeatedDetected)
            return;

        // IMPORTANT:
        // Only treat destruction as a tutorial defeat if
        // the enemy was actually hit by the player.
        if (!enemyHasBeenHit)
            return;

        if (tutorialController == null)
            return;

        enemyDefeatedDetected = true;

        tutorialController.Enemy1Defeated();

        Debug.Log(
            "TUTORIAL ENEMY 1 DEFEATED - DETECTED ON DESTROY."
        );
    }
}