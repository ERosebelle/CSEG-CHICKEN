using UnityEngine;
using UnityEngine.InputSystem;

public class MysteryPotion : MonoBehaviour
{
    public InteractionUI interactionUI;
    public MysteryCount mysteryCount;

    private bool playerNearby = false;

    private void Update()
    {
        if (!playerNearby)
            return;

        if (Keyboard.current == null)
            return;

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
            interactionUI.ShowCollectUI(transform);
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
        if (mysteryCount == null)
            return;

        mysteryCount.AddMysteryPotion();

        if (interactionUI != null)
        {
            interactionUI.HideCollectUI();
        }

        gameObject.SetActive(false);
    }
}