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
        {
            Debug.LogError(
                "EnemyColorWeakness: EnemyHealth NOT FOUND on " +
                gameObject.name
            );

            return;
        }

        // ==========================================
        // RANDOM WEAKNESS
        // ==========================================

        weakness =
            (WeaknessColor)Random.Range(
                0,
                4
            );

        Debug.Log(
            "ENEMY WEAKNESS | " +
            gameObject.name +
            " | WEAKNESS: " +
            GetWeaknessName()
        );

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

        foreach (
            Renderer renderer
            in renderers
        )
        {
            if (renderer == null)
                continue;

            renderer.material.color =
                color;
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