using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;

    [Header("Lifetime")]
    public float lifetime = 3f;

    [Header("Player Damage")]
    public bool damagePlayer = false;

    [Tooltip("Normal damage dealt when the bullet hits the player.")]
    public float playerDamage = 20f;

    [Header("Special Damage")]
    public bool specialDamage = false;

    [Tooltip("Continuous damage dealt while the bullet remains active.")]
    public float specialDamageAmount = 5f;

    [Tooltip("Time in seconds between each special damage tick.")]
    public float specialDamageInterval = 1f;

    [Header("Special Damage Effect")]
    [Tooltip("Drag the special damage effect model/prefab here.")]
    public GameObject specialDamageEffect;

    [Tooltip("Drag the Player's Transform where the special effect should appear.")]
    public Transform specialDamageEffectPoint;

    [Header("Player Detection")]
    public string playerTag = "Player";

    private float timer;

    private Vector3 previousPosition;

    private PlayerHealth affectedPlayer;

    private GameObject activeSpecialEffect;

    private float specialDamageTimer;

    private bool specialDamageActive;


    // =========================================================
    // ENABLE
    // =========================================================

    private void OnEnable()
    {
        timer = 0f;

        previousPosition =
            transform.position;

        affectedPlayer = null;

        activeSpecialEffect = null;

        specialDamageTimer = 0f;

        specialDamageActive = false;
    }


    // =========================================================
    // UPDATE
    // =========================================================

    private void Update()
    {
        // =====================================================
        // BULLET LIFETIME
        // =====================================================

        timer += Time.deltaTime;

        if (timer >= lifetime)
        {
            StopSpecialDamage();

            gameObject.SetActive(false);

            return;
        }


        // =====================================================
        // SPECIAL DAMAGE
        // =====================================================

        if (specialDamageActive)
        {
            UpdateSpecialDamage();

            return;
        }


        // =====================================================
        // MOVE BULLET
        // =====================================================

        previousPosition =
            transform.position;

        transform.position +=
            transform.forward *
            speed *
            Time.deltaTime;


        // =====================================================
        // CHECK BULLET MOVEMENT
        // =====================================================

        Vector3 movement =
            transform.position -
            previousPosition;

        float distance =
            movement.magnitude;

        if (distance > 0f)
        {
            RaycastHit hit;

            if (Physics.Raycast(
                previousPosition,
                movement.normalized,
                out hit,
                distance
            ))
            {
                HandleHit(
                    hit.collider
                );

                return;
            }
        }
    }


    // =========================================================
    // HANDLE HIT
    // =========================================================

    private void HandleHit(
        Collider hitCollider
    )
    {
        if (hitCollider == null)
            return;


        // =====================================================
        // FIND PLAYER HEALTH
        // =====================================================

        PlayerHealth playerHealth =
            hitCollider.GetComponent<PlayerHealth>();

        if (playerHealth == null)
        {
            playerHealth =
                hitCollider.GetComponentInParent<PlayerHealth>();
        }

        if (playerHealth == null)
        {
            playerHealth =
                hitCollider.GetComponentInChildren<PlayerHealth>();
        }


        // =====================================================
        // PLAYER FOUND
        // =====================================================

        if (playerHealth != null)
        {
            // =================================================
            // SPECIAL BULLET
            // =================================================

            if (specialDamage)
            {
                Debug.Log(
                    "================================"
                );

                Debug.Log(
                    "SPECIAL BULLET HIT PLAYER"
                );

                Debug.Log(
                    "Sending special attack to PlayerHealth."
                );

                Debug.Log(
                    "================================"
                );


                // ---------------------------------------------
                // PLAYER HEALTH HANDLES SPECIAL ATTACK
                // ---------------------------------------------

                playerHealth.ReceiveSpecialBulletHit(
                    specialDamageAmount
                );


                // ---------------------------------------------
                // START CONTINUOUS SPECIAL DAMAGE
                // ---------------------------------------------

                StartSpecialDamage(
                    playerHealth
                );

                return;
            }


            // =================================================
            // NORMAL BULLET
            // =================================================

            if (damagePlayer)
            {
                Debug.Log(
                    "================================"
                );

                Debug.Log(
                    "NORMAL BULLET HIT PLAYER"
                );

                Debug.Log(
                    "Sending normal attack to PlayerHealth."
                );

                Debug.Log(
                    "================================"
                );


                // ---------------------------------------------
                // PLAYER HEALTH HANDLES NORMAL ATTACK
                // ---------------------------------------------

                playerHealth.ReceiveNormalBulletDamage(
                    playerDamage
                );
            }


            // ---------------------------------------------
            // DISABLE NORMAL BULLET
            // ---------------------------------------------

            gameObject.SetActive(false);

            return;
        }


        // =====================================================
        // PLAYER TAG
        // =====================================================

        if (hitCollider.CompareTag(playerTag))
        {
            gameObject.SetActive(false);

            return;
        }


        // =====================================================
        // OTHER OBJECT
        // =====================================================

        gameObject.SetActive(false);
    }


    // =========================================================
    // START SPECIAL DAMAGE
    // =========================================================

    private void StartSpecialDamage(
        PlayerHealth playerHealth
    )
    {
        if (playerHealth == null)
            return;


        // =====================================================
        // CONNECT PLAYER
        // =====================================================

        affectedPlayer =
            playerHealth;


        // =====================================================
        // ACTIVATE SPECIAL DAMAGE
        // =====================================================

        specialDamageActive =
            true;


        specialDamageTimer =
            0f;


        // =====================================================
        // SPAWN SPECIAL DAMAGE EFFECT
        // =====================================================

        SpawnSpecialDamageEffect();
    }


    // =========================================================
    // UPDATE SPECIAL DAMAGE
    // =========================================================

    private void UpdateSpecialDamage()
    {
        // =====================================================
        // PLAYER NO LONGER EXISTS
        // =====================================================

        if (affectedPlayer == null)
        {
            StopSpecialDamage();

            return;
        }


        // =====================================================
        // PLAYER DEAD
        // =====================================================

        if (affectedPlayer.IsDead())
        {
            StopSpecialDamage();

            gameObject.SetActive(false);

            return;
        }


        // =====================================================
        // CONTINUOUS DAMAGE TIMER
        // =====================================================

        specialDamageTimer -=
            Time.deltaTime;


        if (specialDamageTimer <= 0f)
        {
            ApplySpecialDamage();

            specialDamageTimer =
                Mathf.Max(
                    0.01f,
                    specialDamageInterval
                );
        }


        // =====================================================
        // FOLLOW PLAYER
        // =====================================================

        if (activeSpecialEffect != null)
        {
            if (specialDamageEffectPoint != null)
            {
                activeSpecialEffect.transform.position =
                    specialDamageEffectPoint.position;

                activeSpecialEffect.transform.rotation =
                    specialDamageEffectPoint.rotation;
            }
            else
            {
                activeSpecialEffect.transform.position =
                    affectedPlayer.transform.position;
            }
        }
    }


    // =========================================================
    // APPLY SPECIAL DAMAGE
    // =========================================================

    private void ApplySpecialDamage()
    {
        if (affectedPlayer == null)
            return;

        if (specialDamageAmount <= 0f)
            return;


        // =====================================================
        // APPLY CONTINUOUS DAMAGE
        // =====================================================

        affectedPlayer.TakeDamageAmount(
            specialDamageAmount
        );
    }


    // =========================================================
    // SPAWN SPECIAL DAMAGE EFFECT
    // =========================================================

    private void SpawnSpecialDamageEffect()
    {
        if (specialDamageEffect == null)
            return;

        if (affectedPlayer == null)
            return;


        // =====================================================
        // DEFAULT POSITION
        // =====================================================

        Vector3 spawnPosition =
            affectedPlayer.transform.position;

        Quaternion spawnRotation =
            Quaternion.identity;


        // =====================================================
        // USE MANUAL EFFECT POINT
        // =====================================================

        if (specialDamageEffectPoint != null)
        {
            spawnPosition =
                specialDamageEffectPoint.position;

            spawnRotation =
                specialDamageEffectPoint.rotation;
        }


        // =====================================================
        // CREATE EFFECT
        // =====================================================

        activeSpecialEffect =
            Instantiate(
                specialDamageEffect,
                spawnPosition,
                spawnRotation
            );
    }


    // =========================================================
    // STOP SPECIAL DAMAGE
    // =========================================================

    private void StopSpecialDamage()
    {
        specialDamageActive =
            false;

        affectedPlayer =
            null;


        // =====================================================
        // REMOVE SPECIAL EFFECT
        // =====================================================

        if (activeSpecialEffect != null)
        {
            Destroy(
                activeSpecialEffect
            );

            activeSpecialEffect =
                null;
        }
    }
}