using UnityEngine;

public class YellowCount : MonoBehaviour
{
    [Header("Count Controller")]
    public Count count;

    [Header("Yellow Potion Count")]
    [SerializeField]
    private int yellowPotionCount = 0;

    [Header("Maximum")]
    public int maxCount = 99;

    private void Start()
    {
        yellowPotionCount =
            Mathf.Clamp(
                yellowPotionCount,
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

    public void AddYellowPotion()
    {
        if (yellowPotionCount >= maxCount)
        {
            return;
        }

        yellowPotionCount++;

        UpdateCountUI();
    }

    // ==========================================
    // REMOVE
    // ==========================================

    public void RemoveYellowPotion()
    {
        if (yellowPotionCount <= 0)
        {
            return;
        }

        yellowPotionCount--;

        UpdateCountUI();
    }

    // ==========================================
    // GET COUNT
    // ==========================================

    public int GetYellowPotionCount()
    {
        return yellowPotionCount;
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
            yellowPotionCount
        );
    }
}