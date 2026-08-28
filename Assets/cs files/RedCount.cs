using UnityEngine;

public class RedCount : MonoBehaviour
{
    [Header("Count Controller")]
    public Count count;

    [Header("Red Potion Count")]
    [SerializeField]
    private int redPotionCount = 0;

    [Header("Maximum")]
    public int maxCount = 99;

    private void Start()
    {
        redPotionCount =
            Mathf.Clamp(
                redPotionCount,
                0,
                maxCount
            );

        if (count == null)
        {
            return;
        }

        UpdateCountUI();
    }

    // ==========================================
    // ADD
    // ==========================================

    public void AddRedPotion()
    {
        if (redPotionCount >= maxCount)
        {
            return;
        }

        redPotionCount++;

        UpdateCountUI();
    }

    // ==========================================
    // REMOVE
    // ==========================================

    public void RemoveRedPotion()
    {
        if (redPotionCount <= 0)
        {
            return;
        }

        redPotionCount--;

        UpdateCountUI();
    }

    // ==========================================
    // GET COUNT
    // ==========================================

    public int GetRedPotionCount()
    {
        return redPotionCount;
    }

    // ==========================================
    // UPDATE UI
    // ==========================================

    private void UpdateCountUI()
    {
        if (count == null)
        {
            return;
        }

        count.SetCount(
            redPotionCount
        );
    }
}