using UnityEngine;

public class EnemyColorWeakness : MonoBehaviour
{
    public enum WeaknessColor
    {
        Red,
        Blue,
        Green,
        Yellow
    }

    [Header("Weakness")]
    public WeaknessColor weakness;

    private EnemyHealth enemyHealth;

    private static WeaknessColor lastWeakness;
    private static bool hasPreviousWeakness = false;

    void Start()
    {
        // ==========================================
        // GET EXISTING ENEMY HEALTH
        // ==========================================

        enemyHealth =
            GetComponent<EnemyHealth>();

        if (enemyHealth == null)
        {
            enemyHealth =
                GetComponentInParent<EnemyHealth>();
        }

        if (enemyHealth == null)
            return;

        // ==========================================
        // RANDOM WEAKNESS
        // ==========================================

        WeaknessColor newWeakness;

        do
        {
            newWeakness =
                (WeaknessColor)Random.Range(
                    0,
                    4
                );

        } while (
            hasPreviousWeakness &&
            newWeakness == lastWeakness
        );

        weakness = newWeakness;

        lastWeakness = weakness;
        hasPreviousWeakness = true;

        // ==========================================
        // CHANGE EXISTING HEART COLORS
        // ==========================================

        ApplyWeaknessToHearts();
    }

    // ==========================================
    // APPLY WEAKNESS TO EXISTING HEARTS
    // ==========================================

    void ApplyWeaknessToHearts()
    {
        Color weaknessColor =
            GetWeaknessColor();

        ApplyColor(
            enemyHealth.heartModel1,
            weaknessColor
        );

        ApplyColor(
            enemyHealth.heartModel2,
            weaknessColor
        );

        ApplyColor(
            enemyHealth.heartModel3,
            weaknessColor
        );
    }

    // ==========================================
    // CHANGE HEART COLOR ONLY
    // ==========================================

    void ApplyColor(
        GameObject heart,
        Color color
    )
    {
        if (heart == null)
            return;

        Renderer[] renderers =
            heart.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            if (renderer == null)
                continue;

            renderer.material.color = color;
        }
    }

    // ==========================================
    // GET WEAKNESS COLOR
    // ==========================================

    public Color GetWeaknessColor()
    {
        switch (weakness)
        {
            case WeaknessColor.Red:
                return Color.red;

            case WeaknessColor.Blue:
                return Color.blue;

            case WeaknessColor.Green:
                return Color.green;

            case WeaknessColor.Yellow:
                return Color.yellow;
        }

        return Color.white;
    }

    // ==========================================
    // GET WEAKNESS NAME
    // ==========================================

    public string GetWeaknessName()
    {
        return weakness
            .ToString()
            .ToUpper();
    }

    // ==========================================
    // CHECK IF COLOR IS WEAKNESS
    // ==========================================

    public bool IsWeaknessColor(
        Color color
    )
    {
        Color target =
            GetWeaknessColor();

        return Mathf.Abs(
                   color.r - target.r
               ) < 0.05f &&
               Mathf.Abs(
                   color.g - target.g
               ) < 0.05f &&
               Mathf.Abs(
                   color.b - target.b
               ) < 0.05f;
    }
}