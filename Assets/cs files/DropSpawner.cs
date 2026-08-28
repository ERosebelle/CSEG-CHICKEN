using UnityEngine;

public class DropSpawner : MonoBehaviour
{
    [Header("Drop Models")]
    public GameObject mysteryPotion;
    public GameObject ammoPotion;

    [Header("Spawn Settings")]
    public float spawnDelay = 3f;

    [Header("Drop Limit")]
    public int maxDrops = 5;

    private void Start()
    {
        InvokeRepeating(
            nameof(CheckAndSpawn),
            spawnDelay,
            spawnDelay
        );
    }

    private void CheckAndSpawn()
    {
        // ==========================================
        // CHECK DROP MODELS
        // ==========================================

        if (mysteryPotion == null)
            return;

        if (ammoPotion == null)
            return;

        // ==========================================
        // COUNT CURRENT DROPS
        // ==========================================

        MysteryPotion[] mysteryDrops =
            FindObjectsByType<MysteryPotion>(
                FindObjectsInactive.Exclude,
                FindObjectsSortMode.None
            );

        AmmoPotion[] ammoDrops =
            FindObjectsByType<AmmoPotion>(
                FindObjectsInactive.Exclude,
                FindObjectsSortMode.None
            );

        int currentDrops =
            mysteryDrops.Length +
            ammoDrops.Length;

        // ==========================================
        // MAX DROP LIMIT
        // ==========================================

        if (currentDrops >= maxDrops)
            return;

        // ==========================================
        // RANDOM DROP
        // ==========================================

        GameObject selectedDrop;

        int randomDrop =
            Random.Range(0, 2);

        if (randomDrop == 0)
        {
            selectedDrop = mysteryPotion;
        }
        else
        {
            selectedDrop = ammoPotion;
        }

        // ==========================================
        // SPAWN
        // ==========================================

        GameObject newDrop =
            Instantiate(
                selectedDrop,
                transform.position,
                transform.rotation
            );

        newDrop.SetActive(true);
    }
}