using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

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

    // =========================================================
    // TUTORIAL ONLY
    // =========================================================

    [Header("Tutorial Scene Attack")]
    public float tutorialDamage = 10f;
    public float tutorialAttackInterval = 1f;

    private float tutorialAttackTimer = 0f;

    // =========================================================
    // EXISTING LOGIC
    // =========================================================

    private Vector3 originalPosition;

    private float hitDetectionTimer;
    private bool hitDetectionActive;

    void Start()
    {
        originalPosition = transform.position;

        hitDetectionTimer = 0f;
        hitDetectionActive = false;

        enemyBulletCount = startingBulletCount;

        // =====================================================
        // TUTORIAL SCENE ONLY
        // =====================================================

        if (SceneManager.GetActiveScene().name == "Tutorial Scene")
        {
            tutorialAttackTimer = 0f;
        }
    }

    void Update()
    {
        if (player == null)
            return;

        // =====================================================
        // TUTORIAL SCENE ONLY
        // =====================================================

        if (SceneManager.GetActiveScene().name == "Tutorial Scene")
        {
            HandleTutorialAttack();
        }

        // ==========================================
        // HIT DETECTION TIMER
        // ==========================================

        if (hitDetectionActive)
        {
            hitDetectionTimer -= Time.deltaTime;

            if (hitDetectionTimer <= 0f)
            {
                hitDetectionTimer = 0f;
                hitDetectionActive = false;
            }
        }

        // ==========================================
        // DISTANCE TO PLAYER
        // ==========================================

        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        // ==========================================
        // DETECTION
        // ==========================================

        bool playerDetected =
            distance <= detectionRange;

        if (hitDetectionActive)
        {
            playerDetected = true;
        }

        // ==========================================
        // PLAYER NOT DETECTED
        // ==========================================

        if (!playerDetected)
        {
            ReturnToOriginalPosition();
            return;
        }

        // ==========================================
        // PLAYER DETECTED
        // ==========================================

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
    // TUTORIAL SCENE ATTACK
    // =========================================================

    private void HandleTutorialAttack()
    {
        if (player == null)
            return;

        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        if (distance > detectionRange)
            return;

        tutorialAttackTimer -= Time.deltaTime;

        if (tutorialAttackTimer > 0f)
            return;

        tutorialAttackTimer =
            tutorialAttackInterval;

        PlayerHealth playerHealth =
            player.GetComponent<PlayerHealth>();

        if (playerHealth == null)
        {
            playerHealth =
                player.GetComponentInParent<PlayerHealth>();
        }

        if (playerHealth == null)
        {
            playerHealth =
                player.GetComponentInChildren<PlayerHealth>();
        }

        if (playerHealth == null)
            return;

        playerHealth.TakeDamage(tutorialDamage);

        Debug.Log(
            "TUTORIAL CORRUPTED PLANT ATTACKED PLAYER: -"
            + tutorialDamage
            + " HEALTH"
        );
    }

    // ==========================================
    // CALLED WHEN ENEMY IS HIT
    // ==========================================

    public void OnEnemyHit()
    {
        hitDetectionActive = true;

        hitDetectionTimer =
            hitDetectionDuration;
    }

    // ==========================================
    // ADD ENEMY BULLET
    // ==========================================

    public void AddEnemyBullet()
    {
        enemyBulletCount++;
    }

    // ==========================================
    // GET ENEMY BULLET COUNT
    // ==========================================

    public int GetEnemyBulletCount()
    {
        return enemyBulletCount;
    }

    // ==========================================
    // REMOVE / USE ENEMY BULLET
    // ==========================================

    public bool UseEnemyBullet()
    {
        if (enemyBulletCount <= 0)
        {
            return false;
        }

        enemyBulletCount--;

        return true;
    }

    // ==========================================
    // CHASE
    // ==========================================

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
    }

    // ==========================================
    // RETREAT
    // ==========================================

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
    }

    // ==========================================
    // RETURN
    // ==========================================

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
    }
}