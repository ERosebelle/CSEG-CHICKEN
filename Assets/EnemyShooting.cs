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

    [Header("Detection")]
    public float shootingRange = 14f;

    [Header("Aiming")]
    public float rotationSpeed = 10f;

    private float shootTimer;

    void Update()
    {
        if (player == null || enemyTarget == null)
            return;

        // Detection uses the Player
        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        if (distance <= shootingRange)
        {
            // Enemy faces the target
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

    void FaceTarget()
    {
        Vector3 direction =
            enemyTarget.position -
            transform.position;

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

        // Spawn bullet at FirePoint
        bullet.transform.position =
            firePoint.position;

        // Capture the target position NOW
        Vector3 targetPosition =
            enemyTarget.position;

        // Calculate direction from FirePoint
        // to the target
        Vector3 shootDirection =
            targetPosition -
            firePoint.position;

        if (shootDirection.sqrMagnitude <= 0.01f)
            return;

        shootDirection.Normalize();

        // Aim bullet at the target
        bullet.transform.rotation =
            Quaternion.LookRotation(
                shootDirection
            );

        // Activate bullet
        bullet.SetActive(true);
    }
}