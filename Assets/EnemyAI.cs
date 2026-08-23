using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Detection")]
    public float detectionRange = 14f;

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

    void Start()
    {
        // Remember where the enemy started
        originalPosition = transform.position;
    }

    void Update()
    {
        if (player == null)
            return;

        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        // Player is outside detection range
        if (distance > detectionRange)
        {
            ReturnToOriginalPosition();
            return;
        }

        // Player is detected
        FacePlayer();

        // Player is too close
        if (distance <= retreatRange)
        {
            RetreatFromPlayer();
        }
        else
        {
            ChasePlayer();
        }
    }

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

    void ReturnToOriginalPosition()
    {
        Vector3 direction =
            originalPosition -
            transform.position;

        direction.y = 0f;

        // Already back at original position
        if (direction.sqrMagnitude <=
            returnDistance * returnDistance)
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

        // Move back
        transform.position +=
            direction *
            returnSpeed *
            Time.deltaTime;

        // Face the direction of movement
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

    void FacePlayer()
    {
        Vector3 direction =
            player.position -
            transform.position;

        direction.y = 0f;

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
}