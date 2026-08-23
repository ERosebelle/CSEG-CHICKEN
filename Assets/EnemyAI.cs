using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Detection")]
    public float detectionRange = 14f;

    [Header("Movement")]
    public float moveSpeed = 3f;

    void Update()
    {
        if (player == null)
            return;

        float distance =
            Vector3.Distance(
                transform.position,
                player.position
            );

        if (distance <= detectionRange)
        {
            Debug.Log("PLAYER DETECTED!");

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

        Debug.Log("CHASING PLAYER!");
    }
}