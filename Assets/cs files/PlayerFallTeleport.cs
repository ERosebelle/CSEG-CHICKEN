using UnityEngine;

public class PlayerFallTeleport : MonoBehaviour
{
    [Header("Teleport Settings")]
    public float teleportHeight = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        GameObject[] enemies =
            GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            Debug.LogWarning(
                "PlayerFallTeleport: No enemies with tag 'Enemy' found!"
            );

            return;
        }

        GameObject randomEnemy =
            enemies[Random.Range(0, enemies.Length)];

        Vector3 teleportPosition =
            randomEnemy.transform.position +
            Vector3.up * teleportHeight;

        other.transform.position =
            teleportPosition;

        Debug.Log(
            "PLAYER FELL → TELEPORTED TO: " +
            randomEnemy.name
        );
    }
}