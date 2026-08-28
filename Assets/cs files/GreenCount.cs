using UnityEngine;

public class GreenCount : MonoBehaviour
{
    [Header("Count Controller")]
    public Count count;

    [Header("Green Potion Count")]
    [SerializeField]
    private int greenPotionCount = 0;

    [Header("Maximum")]
    public int maxCount = 99;

    private void Start()
    {
        greenPotionCount =
            Mathf.Clamp(
                greenPotionCount,
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

    public void AddGreenPotion()
    {
        if (greenPotionCount >= maxCount)
        {
            return;
        }

        greenPotionCount++;

        UpdateCountUI();
    }

    // ==========================================
    // REMOVE
    // ==========================================

    public void RemoveGreenPotion()
    {
        if (greenPotionCount <= 0)
        {
            return;
        }

        greenPotionCount--;

        UpdateCountUI();
    }

    // ==========================================
    // GET COUNT
    // ==========================================

    public int GetGreenPotionCount()
    {
        return greenPotionCount;
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
            greenPotionCount
        );
    }
}