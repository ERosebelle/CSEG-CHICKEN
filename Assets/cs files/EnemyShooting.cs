using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Enemy Target")]
    public Transform enemyTarget;

    [Header("Shooting")]
    public GameObject bullet;
    public Transform firePoint;
    public float shootInterval = 2f;

    [Header("Enemy Bullet Count")]
    public int startingBullets = 1;
    public int maxBullets = 5;

    [Header("Detection")]
    public float shootingRange = 14f;

    [Header("Aiming")]
    public float rotationSpeed = 10f;

    private float shootTimer;
    private int currentBullets;

    void Start()
    {
        currentBullets = Mathf.Clamp(
            startingBullets,
            1,
            maxBullets
        );

        shootTimer = 0f;

        Debug.Log(
            "Enemy Bullet Count: " +
            currentBullets
        );
    }

    void Update()
    {
        if (player == null || enemyTarget == null)
            return;

        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        if (distance <= shootingRange)
        {
            FaceTarget();

            shootTimer -= Time.deltaTime;

            if (shootTimer <= 0f)
            {
                Shoot();

                shootTimer = shootInterval;
            }
        }
        else
        {
            shootTimer = 0f;
        }
    }

    // ==========================================
    // ADD BULLET
    // WRONG COLOR HIT
    // ==========================================

    public void AddBullet()
    {
        if (currentBullets >= maxBullets)
        {
            Debug.Log(
                "Enemy Bullet Count already MAX: " +
                currentBullets
            );

            return;
        }

        currentBullets++;

        Debug.Log(
            "WRONG COLOR HIT | " +
            "Enemy Bullet Count +1 | " +
            "Current Count: " +
            currentBullets
        );
    }

    // ==========================================
    // GET BULLET COUNT
    // ==========================================

    public int GetCurrentBullets()
    {
        return currentBullets;
    }

    // ==========================================
    // FACE TARGET
    // ==========================================

    void FaceTarget()
    {
        Vector3 direction =
            enemyTarget.position -
            transform.position;

        if (direction.sqrMagnitude <= 0.01f)
            return;

        Quaternion targetRotation =
            Quaternion.LookRotation(
                direction
            );

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed *
                Time.deltaTime
            );
    }

    // ==========================================
    // SHOOT
    // ==========================================

void Shoot()
{
    if (bullet == null)
    {
        Debug.LogError(
            "EnemyShooting: Bullet is NOT assigned!"
        );

        return;
    }

    if (firePoint == null)
    {
        Debug.LogError(
            "EnemyShooting: Fire Point is NOT assigned!"
        );

        return;
    }

    Vector3 shootDirection =
        enemyTarget.position -
        firePoint.position;

    if (shootDirection.sqrMagnitude <= 0.01f)
        return;

    shootDirection.Normalize();

    Debug.Log(
        "ENEMY ATTACK | " +
        "Bullet Count: " +
        currentBullets
    );

    for (int i = 0; i < currentBullets; i++)
    {
        Vector3 direction =
            shootDirection;

        // Spread multiple bullets
        if (currentBullets > 1)
        {
            float spread =
                (i - (currentBullets - 1) / 2f) * 5f;

            direction =
                Quaternion.AngleAxis(
                    spread,
                    Vector3.up
                ) *
                shootDirection;
        }

        GameObject newBullet =
            Instantiate(
                bullet,
                firePoint.position,
                Quaternion.LookRotation(
                    direction
                )
            );

        newBullet.transform.localScale =
            bullet.transform.localScale;

        newBullet.SetActive(true);

        Debug.Log(
            "Bullet " +
            (i + 1) +
            " / " +
            currentBullets +
            " spawned"
        );
    }
}}