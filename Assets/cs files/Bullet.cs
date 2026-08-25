using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;

    [Header("Lifetime")]
    public float lifetime = 3f;

    [Header("Player Damage")]
    public bool damagePlayer = false;

    [Tooltip("Damage dealt to the player when damagePlayer is enabled.")]
    public float playerDamage = 20f;

    [Header("Player Detection")]
    public string playerTag = "Player";

    private float timer;

    // ==========================================
    // ENABLE
    // ==========================================

    void OnEnable()
    {
        timer = 0f;
    }

    // ==========================================
    // UPDATE
    // ==========================================

    void Update()
    {
        transform.position +=
            transform.forward *
            speed *
            Time.deltaTime;

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
        // CHECK IF PLAYER
        // ==========================================

        if (hitObject.CompareTag(playerTag))
        {
            if (damagePlayer)
            {
                Debug.Log(
                    "PLAYER DAMAGE ENABLED | " +
                    "Damage: " +
                    playerDamage
                );

                // PLAYER HEALTH WILL BE CONNECTED HERE LATER
            }
            else
            {
                Debug.Log(
                    "PLAYER HIT | " +
                    "PLAYER DAMAGE DISABLED"
                );
            }
        }

        // ==========================================
        // BULLET ALWAYS DISAPPEARS
        // ==========================================

        gameObject.SetActive(false);
    }
}