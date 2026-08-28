using UnityEngine;

public class AmmoCount : MonoBehaviour
{
    [Header("Count Controller")]
    public Count count;

    [Header("Ammo Count")]
    [SerializeField]
    private int ammoCount = 0;

    [Header("Maximum")]
    public int maxCount = 9;

    private void Start()
    {
        ammoCount = Mathf.Clamp(
            ammoCount,
            0,
            maxCount
        );

        if (count == null)
            return;

        UpdateCountUI();
    }

    // ==========================================
    // ADD AMMO
    // ==========================================

    public void AddAmmo()
    {
        if (ammoCount >= maxCount)
            return;

        ammoCount++;

        UpdateCountUI();
    }

    // ==========================================
    // REMOVE AMMO
    // ==========================================

    public void RemoveAmmo()
    {
        if (ammoCount <= 0)
            return;

        ammoCount--;

        UpdateCountUI();
    }

    // ==========================================
    // GET AMMO
    // ==========================================

    public int GetAmmoCount()
    {
        return ammoCount;
    }

    // ==========================================
    // UPDATE UI
    // ==========================================

    private void UpdateCountUI()
    {
        if (count == null)
            return;

        count.SetCount(ammoCount);
    }
}