using UnityEngine;
using UnityEngine.InputSystem;

public class HealthCount : MonoBehaviour
{
    [Header("Count Controller")]
    public Count count;

    [Header("Player Health")]
    public PlayerHealth playerHealth;

    [Header("Health Potion Count")]
    [SerializeField]
    private int healthPotionCount = 0;

    [Header("Maximum")]
    public int maxCount = 9;

    private void Start()
    {
        healthPotionCount = Mathf.Clamp(
            healthPotionCount,
            0,
            maxCount
        );

        if (playerHealth == null)
        {
            playerHealth = FindFirstObjectByType<PlayerHealth>();
        }

        UpdateCountUI();
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.kKey.wasPressedThisFrame)
        {
            UseHealthPotion();
        }
    }

    public void AddHealthPotion()
    {
        if (healthPotionCount >= maxCount)
        {
            return;
        }

        healthPotionCount++;

        UpdateCountUI();
    }

    public void RemoveHealthPotion()
    {
        if (healthPotionCount <= 0)
        {
            return;
        }

        healthPotionCount--;

        UpdateCountUI();
    }

    public void UseHealthPotion()
    {
        if (healthPotionCount <= 0)
        {
            return;
        }

        if (playerHealth == null)
        {
            playerHealth = FindFirstObjectByType<PlayerHealth>();
        }

        if (playerHealth == null)
        {
            return;
        }

        if (playerHealth.GetHealthPercent() >= 1f)
        {
            return;
        }

        playerHealth.RestoreFullHealth();

        healthPotionCount--;

        UpdateCountUI();
    }

    public int GetHealthPotionCount()
    {
        return healthPotionCount;
    }

    private void UpdateCountUI()
    {
        if (count == null)
        {
            return;
        }

        count.SetCount(healthPotionCount);
    }
}