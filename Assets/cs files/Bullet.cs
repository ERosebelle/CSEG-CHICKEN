using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum SpecialPower
    {
        None,
        Knockback,
        DamageZone
    }

    [Header("Movement")]
    public float speed = 10f;

    [Header("Lifetime")]
    public float lifetime = 3f;

    [Header("Player Damage")]
    public bool damagePlayer = false;

    [Tooltip("Damage dealt to the player.")]
    public float playerDamage = 20f;

    [Header("Player Detection")]
    public string playerTag = "Player";

    [Header("Special Power")]
    public SpecialPower specialPower = SpecialPower.None;

    [Header("Knockback")]
    public float knockbackForce = 15f;

    [Header("Damage Zone")]
    public float damageInterval = 0.5f;
    public float damageZoneRadius = 3f;

    private float timer;
    private float damageTimer;

    private bool playerInsideDamageZone;

    // ==========================================
    // ENABLE
    // ==========================================

    void OnEnable()
    {
        timer = 0f;
        damageTimer = 0f;
        playerInsideDamageZone = false;
    }

    // ==========================================
    // UPDATE
    // ==========================================

    void Update()
    {
        // ==========================================
        // DAMAGE ZONE
        // ==========================================

        if (specialPower == SpecialPower.DamageZone)
        {
            HandleDamageZone();
        }
        else
        {
            // ==========================================
            // NORMAL BULLET MOVEMENT
            // ==========================================

            transform.position +=
                transform.forward *
                speed *
                Time.deltaTime;
        }

        // ==========================================
        // LIFETIME
        // ==========================================

        timer += Time.deltaTime;

        if (timer >= lifetime)
        {
            gameObject.SetActive(false);
        }
    }

    // ==========================================
    // COLLISION
    // ==========================================

    void OnCollisionEnter(Collision collision)
    {
        HandleHit(collision.gameObject);
    }

    // ==========================================
    // TRIGGER
    // ==========================================

    void OnTriggerEnter(Collider other)
    {
        HandleHit(other.gameObject);
    }

    // ==========================================
    // HANDLE HIT
    // ==========================================

    void HandleHit(GameObject hitObject)
    {
        if (hitObject == null)
            return;

        Debug.Log(
            "ENEMY BULLET HIT: " +
            hitObject.name
        );

        // ==========================================
        // DAMAGE ZONE
        // ==========================================

        if (specialPower == SpecialPower.DamageZone)
        {
            return;
        }

        // ==========================================
        // CHECK PLAYER
        // ==========================================

        if (hitObject.CompareTag(playerTag))
        {
            HandlePlayerHit(hitObject);

            // ==========================================
            // NORMAL BULLET DISAPPEARS
            // ==========================================

            if (specialPower != SpecialPower.DamageZone)
            {
                gameObject.SetActive(false);
            }

            return;
        }

        // ==========================================
        // HIT ANY OTHER OBJECT
        // ==========================================

        gameObject.SetActive(false);
    }

    // ==========================================
    // PLAYER HIT
    // ==========================================

    void HandlePlayerHit(GameObject playerObject)
    {
        // ==========================================
        // NORMAL DAMAGE
        // ==========================================

        if (damagePlayer)
        {
            Debug.Log(
                "PLAYER DAMAGE ENABLED | " +
                "Damage: " +
                playerDamage
            );

            // PLAYER HEALTH WILL BE CONNECTED HERE LATER
        }

        // ==========================================
        // KNOCKBACK
        // ==========================================

        if (specialPower == SpecialPower.Knockback)
        {
            Debug.Log(
                "PLAYER KNOCKBACK | " +
                "Force: " +
                knockbackForce
            );

            Rigidbody playerRigidbody =
                playerObject.GetComponent<Rigidbody>();

            if (playerRigidbody == null)
            {
                playerRigidbody =
                    playerObject.GetComponentInParent<Rigidbody>();
            }

            if (playerRigidbody != null)
            {
                Vector3 direction =
                    playerObject.transform.position -
                    transform.position;

                direction.y = 0f;

                if (direction.sqrMagnitude <= 0.01f)
                {
                    direction =
                        transform.forward;
                }

                direction.Normalize();

                playerRigidbody.AddForce(
                    direction *
                    knockbackForce,
                    ForceMode.Impulse
                );
            }
        }
    }

    // ==========================================
    // DAMAGE ZONE
    // ==========================================

    void HandleDamageZone()
    {
        Collider[] nearbyObjects =
            Physics.OverlapSphere(
                transform.position,
                damageZoneRadius
            );

        bool playerFound = false;

        foreach (Collider nearbyCollider in nearbyObjects)
        {
            if (nearbyCollider == null)
                continue;

            GameObject nearbyObject =
                nearbyCollider.gameObject;

            if (nearbyObject.CompareTag(playerTag))
            {
                playerFound = true;
                break;
            }

            Transform parent =
                nearbyObject.transform.parent;

            while (parent != null)
            {
                if (parent.CompareTag(playerTag))
                {
                    playerFound = true;
                    break;
                }

                parent =
                    parent.parent;
            }

            if (playerFound)
                break;
        }

        playerInsideDamageZone =
            playerFound;

        if (!playerInsideDamageZone)
        {
            damageTimer = 0f;
            return;
        }

        damageTimer += Time.deltaTime;

        if (damageTimer >= damageInterval)
        {
            damageTimer = 0f;

            Debug.Log(
                "DARKNESS DAMAGE ZONE | " +
                "Player is inside"
            );

            if (damagePlayer)
            {
                Debug.Log(
                    "PLAYER DAMAGE ENABLED | " +
                    "Damage: " +
                    playerDamage
                );

                // PLAYER HEALTH WILL BE CONNECTED HERE LATER
            }
        }
    }

    // ==========================================
    // GIZMO
    // ==========================================

    void OnDrawGizmosSelected()
    {
        if (specialPower != SpecialPower.DamageZone)
            return;

        Gizmos.DrawWireSphere(
            transform.position,
            damageZoneRadius
        );
    }
}