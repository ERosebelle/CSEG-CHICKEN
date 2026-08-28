using UnityEngine;
using UnityEngine.InputSystem;

public class HealthPotion : MonoBehaviour
{
    [Header("Interaction UI")]
    public InteractionUI interactionUI;

    [Header("Health Count")]
    public HealthCount healthCount;

    private bool playerNearby = false;

    private void Update()
    {
        if (!playerNearby)
            return;

        if (Keyboard.current == null)
            return;

        // E key
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            CollectPotion();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerNearby = true;

        if (interactionUI != null)
        {
            interactionUI.ShowCollectUI(
                transform
            );
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerNearby = false;

        if (interactionUI != null)
        {
            interactionUI.HideCollectUI();
        }
    }

    private void CollectPotion()
    {
        // ==========================================
        // ADD HEALTH POTION TO PLAYER COUNT
        // ==========================================

        if (healthCount != null)
        {
            healthCount.AddHealthPotion();
        }
        else
        {
            return;
        }

        // ==========================================
        // HIDE E COLLECT UI
        // ==========================================

        if (interactionUI != null)
        {
            interactionUI.HideCollectUI();
        }

        // ==========================================
        // REMOVE POTION
        // ==========================================

        gameObject.SetActive(false);
    }
}