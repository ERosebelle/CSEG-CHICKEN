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

            GameObject enemyObject = null;

            // ==========================================
            // CHECK HIT OBJECT
            // ==========================================

            if (hitObject.CompareTag(enemyTag))
            {
                enemyObject = hitObject;
            }

            // ==========================================
            // CHECK PARENTS
            // ==========================================

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

            // ==========================================
            // ENEMY FOUND
            // ==========================================

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
                    // ==========================================
                    // FIND ENEMY COLOR WEAKNESS
                    // ==========================================

                    EnemyColorWeakness enemyWeakness =
                        enemyHealth.GetComponent<EnemyColorWeakness>();

                    if (enemyWeakness == null)
                    {
                        enemyWeakness =
                            enemyHealth.GetComponentInParent<EnemyColorWeakness>();
                    }

                    // ==========================================
                    // CHECK BULLET COLOR
                    // ==========================================

                    if (enemyWeakness != null)
                    {
                        bool correctColor =
                            enemyWeakness.IsWeaknessColor(
                                bulletColor
                            );

                        // ==========================================
                        // CORRECT COLOR
                        // ==========================================

                        if (correctColor)
                        {
                            Debug.Log(
                                "CORRECT COLOR HIT | " +
                                "Enemy: " +
                                enemyHealth.gameObject.name +
                                " | Bullet: " +
                                GetBulletColorName() +
                                " | Weakness: " +
                                enemyWeakness.GetWeaknessName()
                            );

                            enemyHealth.TakeDamage();
                        }

                        // ==========================================
                        // WRONG COLOR
                        // ==========================================

                        else
                        {
                            Debug.Log(
                                "WRONG COLOR HIT | " +
                                "Enemy: " +
                                enemyHealth.gameObject.name +
                                " | Bullet: " +
                                GetBulletColorName() +
                                " | Weakness: " +
                                enemyWeakness.GetWeaknessName()
                            );

                            // ==========================================
                            // ADD ENEMY BULLET
                            // ==========================================

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
                            else
                            {
                                Debug.LogWarning(
                                    "EnemyShooting NOT FOUND on: " +
                                    enemyHealth.gameObject.name
                                );
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning(
                            "EnemyColorWeakness NOT FOUND on: " +
                            enemyHealth.gameObject.name
                        );
                    }

                    // ==========================================
                    // ACTIVATE ENEMY AI HIT DETECTION
                    // ==========================================

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

                // ==========================================
                // REMOVE BULLET
                // ==========================================

                gameObject.SetActive(false);

                return;
            }

            // ==========================================
            // NON-ENEMY OBJECT
            // ==========================================

            ChangeHitObjectColor(hitObject);

            gameObject.SetActive(false);
        }
    }

    // ==========================================
    // GET BULLET COLOR NAME
    // ==========================================

    string GetBulletColorName()
    {
        if (ApproximatelyColor(
            bulletColor,
            Color.red))
        {
            return "RED";
        }

        if (ApproximatelyColor(
            bulletColor,
            Color.blue))
        {
            return "BLUE";
        }

        if (ApproximatelyColor(
            bulletColor,
            Color.green))
        {
            return "GREEN";
        }

        if (ApproximatelyColor(
            bulletColor,
            Color.yellow))
        {
            return "YELLOW";
        }

        return "OTHER";
    }

    // ==========================================
    // COLOR COMPARISON
    // ==========================================

    bool ApproximatelyColor(
        Color a,
        Color b
    )
    {
        return Mathf.Abs(a.r - b.r) < 0.05f &&
               Mathf.Abs(a.g - b.g) < 0.05f &&
               Mathf.Abs(a.b - b.b) < 0.05f;
    }

    // ==========================================
    // CHANGE OBJECT COLOR
    // ==========================================

    void ChangeHitObjectColor(
        GameObject hitObject
    )
    {
        // ==========================================
        // FIND EXISTING MAP COLOR OBJECT
        // ==========================================

        MapColorObject mapColorObject =
            hitObject.GetComponent<MapColorObject>();

        if (mapColorObject == null)
        {
            mapColorObject =
                hitObject.GetComponentInParent<MapColorObject>();
        }

        // ==========================================
        // EXISTING MAP COLOR OBJECT FOUND
        // ==========================================

        if (mapColorObject != null)
        {
            Debug.Log(
                "MAP COLOR OBJECT FOUND: " +
                mapColorObject.gameObject.name
            );

            mapColorObject.ApplyColor(
                bulletColor
            );

            Debug.Log(
                "MAP OBJECT COLOR: " +
                mapColorObject.GetColorName()
            );

            return;
        }

        // ==========================================
        // FIND OBJECT THAT OWNS RENDERER
        // ==========================================

        GameObject colorObject =
            hitObject;

        Renderer renderer =
            colorObject.GetComponent<Renderer>();

        // ==========================================
        // CHECK PARENTS
        // ==========================================

        if (renderer == null)
        {
            Transform parent =
                hitObject.transform.parent;

            while (parent != null)
            {
                renderer =
                    parent.GetComponent<Renderer>();

                if (renderer != null)
                {
                    colorObject =
                        parent.gameObject;

                    break;
                }

                parent =
                    parent.parent;
            }
        }

        // ==========================================
        // CHECK CHILDREN
        // ==========================================

        if (renderer == null)
        {
            Renderer[] childRenderers =
                hitObject.GetComponentsInChildren<Renderer>();

            if (childRenderers.Length > 0)
            {
                colorObject =
                    hitObject;
            }
        }

        // ==========================================
        // ADD MAP COLOR OBJECT
        // ==========================================

        mapColorObject =
            colorObject.GetComponent<MapColorObject>();

        if (mapColorObject == null)
        {
            mapColorObject =
                colorObject.AddComponent<MapColorObject>();

            Debug.Log(
                "MAP COLOR OBJECT AUTOMATICALLY ADDED: " +
                colorObject.name
            );
        }

        // ==========================================
        // APPLY COLOR
        // ==========================================

        mapColorObject.ApplyColor(
            bulletColor
        );

        Debug.Log(
            "MAP OBJECT REGISTERED | " +
            "Object: " +
            colorObject.name +
            " | Color: " +
            mapColorObject.GetColorName()
        );
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