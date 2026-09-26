using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;

    [Header("Lifetime")]
    public float lifetime = 3f;

    [Header("Player Damage")]
    public bool damagePlayer = false;

    public float playerDamage = 20f;

    [Header("Special Damage")]
    public bool specialDamage = false;

    public float specialDamageAmount = 5f;

    public float specialDamageInterval = 1f;

    [Header("Special Damage Effect")]
    public GameObject specialDamageEffect;

    public Transform specialDamageEffectPoint;

    [Header("Player Detection")]
    public string playerTag = "Player";

    private float timer;
    private Vector3 previousPosition;
    private PlayerHealth affectedPlayer;
    private GameObject activeSpecialEffect;
    private float specialDamageTimer;
    private bool specialDamageActive;

    private void OnEnable()
    {
        timer = 0f;
        previousPosition = transform.position;
        affectedPlayer = null;
        activeSpecialEffect = null;
        specialDamageTimer = 0f;
        specialDamageActive = false;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= lifetime)
        {
            StopSpecialDamage();
            gameObject.SetActive(false);
            return;
        }

        if (specialDamageActive)
        {
            UpdateSpecialDamage();
            return;
        }

        previousPosition = transform.position;

        transform.position +=
            transform.forward *
            speed *
            Time.deltaTime;

        Vector3 movement =
            transform.position - previousPosition;

        float distance =
            movement.magnitude;

        if (distance > 0f)
        {
            RaycastHit hit;

            if (Physics.Raycast(
                previousPosition,
                movement.normalized,
                out hit,
                distance))
            {
                HandleHit(hit.collider);
                return;
            }
        }
    }

    private void HandleHit(Collider hitCollider)
    {
        if (hitCollider == null)
            return;

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

        if (playerHealth != null)
        {
            if (specialDamage)
            {
                playerHealth.ReceiveSpecialBulletHit(
                    specialDamageAmount
                );

                StartSpecialDamage(playerHealth);

                return;
            }

            if (damagePlayer)
            {
                playerHealth.ReceiveNormalBulletDamage(
                    playerDamage
                );
            }

            gameObject.SetActive(false);
            return;
        }

        if (hitCollider.CompareTag(playerTag))
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(false);
    }

    private void StartSpecialDamage(PlayerHealth playerHealth)
    {
        if (playerHealth == null)
            return;

        affectedPlayer = playerHealth;
        specialDamageActive = true;
        specialDamageTimer = 0f;

        SpawnSpecialDamageEffect();
    }

    private void UpdateSpecialDamage()
    {
        if (affectedPlayer == null)
        {
            StopSpecialDamage();
            return;
        }

        if (affectedPlayer.IsDead())
        {
            StopSpecialDamage();
            gameObject.SetActive(false);
            return;
        }

        specialDamageTimer -= Time.deltaTime;

        if (specialDamageTimer <= 0f)
        {
            ApplySpecialDamage();

            specialDamageTimer =
                Mathf.Max(
                    0.01f,
                    specialDamageInterval
                );
        }

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

    private void ApplySpecialDamage()
    {
        if (affectedPlayer == null)
            return;

        if (specialDamageAmount <= 0f)
            return;

        affectedPlayer.TakeDamageAmount(
            specialDamageAmount
        );
    }

    private void SpawnSpecialDamageEffect()
    {
        if (specialDamageEffect == null)
            return;

        if (affectedPlayer == null)
            return;

        Vector3 spawnPosition =
            affectedPlayer.transform.position;

        Quaternion spawnRotation =
            Quaternion.identity;

        if (specialDamageEffectPoint != null)
        {
            spawnPosition =
                specialDamageEffectPoint.position;

            spawnRotation =
                specialDamageEffectPoint.rotation;
        }

        activeSpecialEffect =
            Instantiate(
                specialDamageEffect,
                spawnPosition,
                spawnRotation
            );
    }

    private void StopSpecialDamage()
    {
        specialDamageActive = false;
        affectedPlayer = null;

        if (activeSpecialEffect != null)
        {
            Destroy(activeSpecialEffect);
            activeSpecialEffect = null;
        }
    }
}