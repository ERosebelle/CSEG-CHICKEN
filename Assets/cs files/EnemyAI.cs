using UnityEngine;

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

    private Vector3 originalPosition;

    private float hitDetectionTimer;
    private bool hitDetectionActive;

    void Start()
    {
        originalPosition = transform.position;

        hitDetectionTimer = 0f;
        hitDetectionActive = false;

        enemyBulletCount = startingBulletCount;

        Debug.Log(
            "ENEMY BULLET COUNT | " +
            gameObject.name +
            " | START: " +
            enemyBulletCount
        );
    }

    void Update()
    {
        if (player == null)
            return;

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

                Debug.Log(
                    "ENEMY HIT DETECTION ENDED | " +
                    gameObject.name
                );
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

    // ==========================================
    // CALLED WHEN ENEMY IS HIT
    // ==========================================

    public void OnEnemyHit()
    {
        hitDetectionActive = true;

        hitDetectionTimer =
            hitDetectionDuration;

        Debug.Log(
            "ENEMY HIT | " +
            gameObject.name +
            " | PLAYER DETECTION FOR " +
            hitDetectionDuration +
            " SECONDS"
        );
    }

    // ==========================================
    // ADD ENEMY BULLET
    // ==========================================

    public void AddEnemyBullet()
    {
        enemyBulletCount++;

        Debug.Log(
            "WRONG COLOR PENALTY | " +
            gameObject.name +
            " | ENEMY BULLET COUNT: " +
            enemyBulletCount
        );
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
            Debug.Log(
                "NO ENEMY BULLETS AVAILABLE | " +
                gameObject.name
            );

            return false;
        }

        enemyBulletCount--;

        Debug.Log(
            "ENEMY BULLET USED | " +
            gameObject.name +
            " | REMAINING: " +
            enemyBulletCount
        );

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