using UnityEngine;

public class LurkingNPC : MonoBehaviour
{
    [Header("Player")]
    public Transform player;

    [Header("Lurking Distance")]
    public float minimumDistance = 20f;
    public float maximumDistance = 35f;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float rotationSpeed = 5f;

    void Update()
    {
        if (player == null)
            return;

        Vector3 direction =
            player.position - transform.position;

        direction.y = 0f;

        float distance =
            direction.magnitude;

        // ==========================================
        // TOO FAR - MOVE TOWARD PLAYER
        // ==========================================

        if (distance > maximumDistance)
        {
            Vector3 movement =
                direction.normalized *
                moveSpeed *
                Time.deltaTime;

            transform.position += movement;
        }

        // ==========================================
        // FACE PLAYER
        // ==========================================

        if (direction.sqrMagnitude > 0.01f)
        {
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
}