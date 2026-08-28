using UnityEngine;

public class BlueCount : MonoBehaviour
{
    [Header("Count Controller")]
    public Count count;

    [Header("Blue Potion Count")]
    [SerializeField]
    private int bluePotionCount = 0;

    [Header("Maximum")]
    public int maxCount = 99;

    private void Start()
    {
        bluePotionCount =
            Mathf.Clamp(
                bluePotionCount,
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

    public void AddBluePotion()
    {
        if (bluePotionCount >= maxCount)
        {
            return;
        }

        bluePotionCount++;

        UpdateCountUI();
    }

    // ==========================================
    // REMOVE
    // ==========================================

    public void RemoveBluePotion()
    {
        if (bluePotionCount <= 0)
        {
            return;
        }

        bluePotionCount--;

        UpdateCountUI();
    }

    // ==========================================
    // GET COUNT
    // ==========================================

    public int GetBluePotionCount()
    {
        return bluePotionCount;
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
            bluePotionCount
        );
    }
}