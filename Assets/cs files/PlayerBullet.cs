using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;

    [Header("Bullet Front Reference")]
    public Transform bulletPeak;

    [Header("Bullet Color")]
    public Color bulletColor = Color.red;

    [Header("Enemy Detection")]
    public string enemyTag = "Enemy";

    private Vector3 previousPosition;

    void OnEnable()
    {
        previousPosition = transform.position;
    }

    void Update()
    {
        previousPosition = transform.position;

        transform.position +=
            transform.forward *
            speed *
            Time.deltaTime;

        Vector3 movement =
            transform.position -
            previousPosition;

        float distance =
            movement.magnitude;

        if (distance <= 0f)
            return;

        RaycastHit hit;

        if (Physics.Raycast(
            previousPosition,
            movement.normalized,
            out hit,
            distance))
        {
            GameObject hitObject =
                hit.collider.gameObject;

            // ==========================================
            // CUBE DETECTION
            // ==========================================

            CubeHealth cubeHealth =
                hitObject.GetComponent<CubeHealth>();

            if (cubeHealth == null)
            {
                cubeHealth =
                    hitObject.GetComponentInParent<CubeHealth>();
            }

            if (cubeHealth != null)
            {
                CubeColorWeakness cubeWeakness =
                    cubeHealth.GetComponent<CubeColorWeakness>();

                if (cubeWeakness == null)
                {
                    cubeWeakness =
                        cubeHealth.GetComponentInParent<CubeColorWeakness>();
                }

                if (cubeWeakness != null)
                {
                    bool correctColor =
                        cubeWeakness.IsWeaknessColor(
                            bulletColor
                        );

                    if (correctColor)
                    {
                        cubeHealth.TakeDamage();

                        cubeWeakness.RegisterHit();
                    }
                    else
                    {
                        CubeSpawner spawner =
                            FindFirstObjectByType<CubeSpawner>();

                        if (spawner != null)
                        {
                            spawner.SpawnExtraCube();
                        }
                    }
                }

                gameObject.SetActive(false);

                return;
            }

            // ==========================================
            // ORIGINAL ENEMY SYSTEM
            // ==========================================

            GameObject enemyObject = null;

            if (hitObject.CompareTag(enemyTag))
            {
                enemyObject = hitObject;
            }

            if (enemyObject == null)
            {
                Transform parent =
                    hitObject.transform.parent;

                while (parent != null)
                {
                    if (parent.CompareTag(enemyTag))
                    {
                        enemyObject =
                            parent.gameObject;

                        break;
                    }

                    parent =
                        parent.parent;
                }
            }

            if (enemyObject != null)
            {
                EnemyHealth enemyHealth =
                    enemyObject.GetComponent<EnemyHealth>();

                if (enemyHealth == null)
                {
                    enemyHealth =
                        enemyObject.GetComponentInParent<EnemyHealth>();
                }

                if (enemyHealth != null)
                {
                    EnemyColorWeakness enemyWeakness =
                        enemyHealth.GetComponent<EnemyColorWeakness>();

                    if (enemyWeakness == null)
                    {
                        enemyWeakness =
                            enemyHealth.GetComponentInParent<EnemyColorWeakness>();
                    }

                    if (enemyWeakness != null)
                    {
                        bool correctColor =
                            enemyWeakness.IsWeaknessColor(
                                bulletColor
                            );

                        if (correctColor)
                        {
                            enemyHealth.TakeDamage();
                        }
                        else
                        {
                            EnemyShooting enemyShooting =
                                enemyHealth.GetComponent<EnemyShooting>();

                            if (enemyShooting == null)
                            {
                                enemyShooting =
                                    enemyHealth.GetComponentInParent<EnemyShooting>();
                            }

                            if (enemyShooting != null)
                            {
                                enemyShooting.AddBullet();
                            }
                        }
                    }

                    EnemyAI enemyAI =
                        enemyHealth.GetComponent<EnemyAI>();

                    if (enemyAI == null)
                    {
                        enemyAI =
                            enemyHealth.GetComponentInParent<EnemyAI>();
                    }

                    if (enemyAI != null)
                    {
                        enemyAI.OnEnemyHit();
                    }
                }

                gameObject.SetActive(false);

                return;
            }

            // ==========================================
            // NON-ENEMY OBJECT
            // ==========================================

            gameObject.SetActive(false);
        }
    }

    // ==========================================
    // BULLET MODEL ROTATION
    // ==========================================

    public void SetModelRotation(
        Vector3 shootDirection
    )
    {
        if (shootDirection.sqrMagnitude <= 0.01f)
            return;

        if (bulletPeak == null)
            return;

        Vector3 localPeakDirection =
            transform.InverseTransformDirection(
                bulletPeak.position -
                transform.position
            );

        if (localPeakDirection.sqrMagnitude <= 0.01f)
            return;

        localPeakDirection.Normalize();

        Quaternion targetRotation =
            Quaternion.FromToRotation(
                localPeakDirection,
                shootDirection.normalized
            );

        transform.rotation =
            targetRotation *
            transform.rotation;
    }
}