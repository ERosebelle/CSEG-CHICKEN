using UnityEngine;
using UnityEngine.InputSystem;

public class RedPotion : MonoBehaviour
{
    [Header("Interaction UI")]
    public InteractionUI interactionUI;

    [Header("Red Count")]
    public RedCount redCount;

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
        // ADD RED POTION TO PLAYER COUNT
        // ==========================================

        if (redCount != null)
        {
            redCount.AddRedPotion();
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