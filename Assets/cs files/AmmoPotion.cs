using UnityEngine;
using UnityEngine.InputSystem;

public class AmmoPotion : MonoBehaviour
{
    [Header("Interaction UI")]
    public InteractionUI interactionUI;

    [Header("Ammo Count")]
    public AmmoCount ammoCount;

    [Header("Ammo Given")]
    public int ammoAmount = 10;

    [Header("Maximum Ammo")]
    public int maxAmmo = 99;

    [Header("Ammo Spawner")]
    public AmmoSpawner ammoSpawner;

    private bool playerNearby = false;

    // ==========================================
    // UPDATE
    // ==========================================

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

    // ==========================================
    // PLAYER ENTER
    // ==========================================

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

    // ==========================================
    // PLAYER EXIT
    // ==========================================

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

    // ==========================================
    // COLLECT POTION
    // ==========================================

    private void CollectPotion()
    {
        if (ammoCount == null)
            return;

        // ==========================================
        // GET CURRENT AMMO
        // ==========================================

        int currentAmmo =
            ammoCount.GetAmmoCount();

        // ==========================================
        // CHECK IF FULL
        // ==========================================

        if (currentAmmo >= maxAmmo)
            return;

        // ==========================================
        // CALCULATE AMMO TO GIVE
        // ==========================================

        int ammoToGive =
            Mathf.Min(
                ammoAmount,
                maxAmmo - currentAmmo
            );

        // ==========================================
        // ADD AMMO
        // ==========================================

        for (int i = 0; i < ammoToGive; i++)
        {
            ammoCount.AddAmmo();
        }

        // ==========================================
        // HIDE E UI
        // ==========================================

        if (interactionUI != null)
        {
            interactionUI.HideCollectUI();
        }

        // ==========================================
        // REMOVE POTION
        // ==========================================

        playerNearby = false;

        // Tell the spawner that the ammo was picked up
        if (ammoSpawner != null)
        {
            ammoSpawner.AmmoPickedUp();
        }

        // Disable this potion
        gameObject.SetActive(false);
    }
}