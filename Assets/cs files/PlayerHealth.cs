using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;

    [SerializeField]
    private float currentHealth = 100f;


    [Header("Hearts")]
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    public GameObject heart4;
    public GameObject heart5;
    public GameObject heart6;
    public GameObject heart7;


    [Header("Player Music")]
    public PlayerMusic playerMusic;


    private bool isDead = false;


    // =========================================================
    // AWAKE
    // =========================================================

    private void Awake()
    {
        FindPlayerMusic();
    }


    // =========================================================
    // START
    // =========================================================

    private void Start()
    {
        currentHealth = maxHealth;
        isDead = false;

        FindPlayerMusic();

        UpdateHealthUI();
        LogHealthPercentage();
    }


    // =========================================================
    // FIND PLAYER MUSIC
    // =========================================================

    private void FindPlayerMusic()
    {
        if (playerMusic != null)
            return;


        // Same GameObject
        playerMusic =
            GetComponent<PlayerMusic>();

        if (playerMusic != null)
        {
            Debug.Log(
                "PlayerHealth: PlayerMusic found on same GameObject."
            );

            return;
        }


        // Parent
        playerMusic =
            GetComponentInParent<PlayerMusic>();

        if (playerMusic != null)
        {
            Debug.Log(
                "PlayerHealth: PlayerMusic found on parent."
            );

            return;
        }


        // Children
        playerMusic =
            GetComponentInChildren<PlayerMusic>();

        if (playerMusic != null)
        {
            Debug.Log(
                "PlayerHealth: PlayerMusic found in child."
            );

            return;
        }


        // Anywhere in the scene
        playerMusic =
            FindFirstObjectByType<PlayerMusic>();


        if (playerMusic != null)
        {
            Debug.Log(
                "PlayerHealth: PlayerMusic found in scene: " +
                playerMusic.name
            );
        }
        else
        {
            Debug.LogError(
                "PlayerHealth: NO PlayerMusic FOUND!"
            );
        }
    }


    // =========================================================
    // BULLET DETECTION
    // =========================================================

    private void OnTriggerEnter(Collider other)
    {
        DetectBullet(other);
    }


    private void OnCollisionEnter(Collision collision)
    {
        DetectBullet(collision.collider);
    }


    // =========================================================
    // DETECT BULLET
    // =========================================================

    private void DetectBullet(Collider hitCollider)
    {
        if (isDead)
            return;

        if (hitCollider == null)
            return;


        Bullet enemyBullet =
            hitCollider.GetComponent<Bullet>();


        if (enemyBullet == null)
        {
            enemyBullet =
                hitCollider.GetComponentInParent<Bullet>();
        }


        if (enemyBullet == null)
        {
            enemyBullet =
                hitCollider.GetComponentInChildren<Bullet>();
        }


        if (enemyBullet == null)
            return;


        // =====================================================
        // SPECIAL ATTACK
        // =====================================================

        if (enemyBullet.specialDamage)
        {
            ReceiveSpecialBulletHit(
                enemyBullet.specialDamageAmount
            );

            return;
        }


        // =====================================================
        // NORMAL ATTACK
        // =====================================================

        if (enemyBullet.damagePlayer)
        {
            ReceiveNormalBulletDamage(
                enemyBullet.playerDamage
            );
        }
    }


    // =========================================================
    // NORMAL BULLET DAMAGE
    // =========================================================

    public void ReceiveNormalBulletDamage(float damage)
    {
        if (isDead)
            return;

        Debug.Log(
            "PLAYER HIT BY NORMAL ATTACK"
        );


        // Make absolutely sure PlayerMusic is connected.
        if (playerMusic == null)
        {
            FindPlayerMusic();
        }


        // Play normal hit sound.
        if (playerMusic != null)
        {
            playerMusic.PlayNormalHitSound();
        }
        else
        {
            Debug.LogError(
                "PlayerHealth: Cannot play normal hit sound because PlayerMusic is missing."
            );
        }


        TakeDamage(damage);
    }


    // =========================================================
    // SPECIAL BULLET DAMAGE
    // =========================================================

    public void ReceiveSpecialBulletHit(float damage)
    {
        if (isDead)
            return;

        Debug.Log(
            "PLAYER HIT BY SPECIAL ATTACK"
        );


        // Make absolutely sure PlayerMusic is connected.
        if (playerMusic == null)
        {
            FindPlayerMusic();
        }


        // Play special hit sound.
        if (playerMusic != null)
        {
            playerMusic.PlaySpecialHitSound();
        }
        else
        {
            Debug.LogError(
                "PlayerHealth: Cannot play special hit sound because PlayerMusic is missing."
            );
        }


        TakeDamage(damage);
    }


    // =========================================================
    // TAKE DAMAGE FROM BULLET
    // =========================================================

    public void TakeDamageFromBullet(Bullet enemyBullet)
    {
        if (isDead)
            return;

        if (enemyBullet == null)
            return;

        if (!enemyBullet.damagePlayer)
            return;


        ReceiveNormalBulletDamage(
            enemyBullet.playerDamage
        );
    }


    // =========================================================
    // TAKE DAMAGE
    // =========================================================

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        if (damage <= 0f)
            return;


        currentHealth -= damage;


        currentHealth =
            Mathf.Clamp(
                currentHealth,
                0f,
                maxHealth
            );


        UpdateHealthUI();
        LogHealthPercentage();


        if (currentHealth <= 0f)
        {
            Die();
        }
    }


    // =========================================================
    // CONTINUOUS DAMAGE
    // =========================================================

    public void TakeDamageAmount(float damage)
    {
        if (isDead)
            return;

        if (damage <= 0f)
            return;


        TakeDamage(damage);
    }


    // =========================================================
    // RESTORE FULL HEALTH
    // =========================================================

    public void RestoreFullHealth()
    {
        if (isDead)
            return;


        currentHealth =
            maxHealth;


        UpdateHealthUI();
        LogHealthPercentage();
    }


    // =========================================================
    // UPDATE HEALTH UI
    // =========================================================

    private void UpdateHealthUI()
    {
        if (maxHealth <= 0f)
            return;


        float healthPerHeart =
            maxHealth / 7f;


        int activeHearts =
            Mathf.CeilToInt(
                currentHealth /
                healthPerHeart
            );


        activeHearts =
            Mathf.Clamp(
                activeHearts,
                0,
                7
            );


        if (heart1 != null)
            heart1.SetActive(activeHearts >= 1);

        if (heart2 != null)
            heart2.SetActive(activeHearts >= 2);

        if (heart3 != null)
            heart3.SetActive(activeHearts >= 3);

        if (heart4 != null)
            heart4.SetActive(activeHearts >= 4);

        if (heart5 != null)
            heart5.SetActive(activeHearts >= 5);

        if (heart6 != null)
            heart6.SetActive(activeHearts >= 6);

        if (heart7 != null)
            heart7.SetActive(activeHearts >= 7);
    }


    // =========================================================
    // LOG HEALTH
    // =========================================================

    private void LogHealthPercentage()
    {
        if (maxHealth <= 0f)
            return;


        float healthPercentage =
            (currentHealth / maxHealth) * 100f;


        Debug.Log(
            "PLAYER HEALTH: " +
            healthPercentage.ToString("F1") +
            "%"
        );
    }


    // =========================================================
    // DEATH
    // =========================================================

    private void Die()
    {
        if (isDead)
            return;


        isDead = true;

        currentHealth = 0f;


        UpdateHealthUI();
        LogHealthPercentage();


        Debug.Log(
            "================================"
        );

        Debug.Log(
            "PLAYER DEFEATED"
        );

        Debug.Log(
            "================================"
        );
    }


    // =========================================================
    // GET CURRENT HEALTH
    // =========================================================

    public float GetCurrentHealth()
    {
        return currentHealth;
    }


    // =========================================================
    // GET HEALTH PERCENT
    // =========================================================

    public float GetHealthPercent()
    {
        if (maxHealth <= 0f)
            return 0f;


        return currentHealth /
               maxHealth;
    }


    // =========================================================
    // CHECK DEAD
    // =========================================================

    public bool IsDead()
    {
        return isDead;
    }
}