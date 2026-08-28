using UnityEngine;
using UnityEngine.InputSystem;

public class YellowPotion : MonoBehaviour
{
    [Header("Interaction UI")]
    public InteractionUI interactionUI;

    [Header("Yellow Count")]
    public YellowCount yellowCount;

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
        // ADD YELLOW POTION TO PLAYER COUNT
        // ==========================================

        if (yellowCount != null)
        {
            yellowCount.AddYellowPotion();
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