using UnityEngine;
using UnityEngine.InputSystem;

public class MysteryCount : MonoBehaviour
{
    [Header("Count Controller")]
    public Count count;

    [Header("Mystery Potion Count")]
    [SerializeField]
    private int mysteryPotionCount = 0;

    [Header("Maximum")]
    public int maxCount = 9;

    [Header("Random Potion Prefabs")]
    public GameObject redPotion;
    public GameObject bluePotion;
    public GameObject greenPotion;
    public GameObject yellowPotion;

    [Header("Spawn Point")]
    public Transform spawnPotion;

    private void Start()
    {
        mysteryPotionCount = Mathf.Clamp(
            mysteryPotionCount,
            0,
            maxCount
        );

        UpdateCountUI();
    }

    private void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            UseMysteryPotion();
        }
    }

    // ==========================================
    // ADD
    // ==========================================

    public void AddMysteryPotion()
    {
        if (mysteryPotionCount >= maxCount)
            return;

        mysteryPotionCount++;

        UpdateCountUI();
    }

    // ==========================================
    // REMOVE
    // ==========================================

    public void RemoveMysteryPotion()
    {
        if (mysteryPotionCount <= 0)
            return;

        mysteryPotionCount--;

        UpdateCountUI();
    }

    // ==========================================
    // USE MYSTERY POTION
    // ==========================================

    private void UseMysteryPotion()
    {
        // Must have Mystery Potions
        if (mysteryPotionCount <= 0)
            return;

        // Must have a spawn point
        if (spawnPotion == null)
            return;

        // ==========================================
        // CREATE POTION ARRAY
        // ==========================================

        GameObject[] potionPrefabs =
        {
            redPotion,
            bluePotion,
            greenPotion,
            yellowPotion
        };

        // ==========================================
        // GET AVAILABLE POTIONS
        // ==========================================

        System.Collections.Generic.List<GameObject>
            availablePotions =
            new System.Collections.Generic.List<GameObject>();

        foreach (GameObject potion in potionPrefabs)
        {
            if (potion != null)
            {
                availablePotions.Add(potion);
            }
        }

        // No potion prefabs assigned
        if (availablePotions.Count == 0)
            return;

        // ==========================================
        // RANDOM POTION
        // ==========================================

        int randomIndex =
            Random.Range(
                0,
                availablePotions.Count
            );

        GameObject selectedPotion =
            availablePotions[randomIndex];

        // ==========================================
        // SPAWN POTION
        // ==========================================

        GameObject spawnedPotion =
            Instantiate(
                selectedPotion,
                spawnPotion.position,
                spawnPotion.rotation
            );

        // Make sure the spawned potion is active
        spawnedPotion.SetActive(true);

        // ==========================================
        // REMOVE 1 MYSTERY POTION
        // ==========================================

        mysteryPotionCount--;

        UpdateCountUI();
    }

    // ==========================================
    // GET COUNT
    // ==========================================

    public int GetMysteryPotionCount()
    {
        return mysteryPotionCount;
    }

    // ==========================================
    // UPDATE UI
    // ==========================================

    private void UpdateCountUI()
    {
        if (count == null)
            return;

        count.SetCount(
            mysteryPotionCount
        );
    }
}