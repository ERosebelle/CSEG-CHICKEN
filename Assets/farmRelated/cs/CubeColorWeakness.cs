using UnityEngine;

public class CubeColorWeakness : MonoBehaviour
{
    public enum CubeColor
    {
        Red,
        Blue,
        Green,
        Yellow
    }

    [Header("Cube Weakness")]
    public CubeColor weakness;

    [Header("Cube Model")]
    public Renderer cubeRenderer;

    [Header("Weakness Change")]
    public int hitsBeforeChange = 5;

    private int hitCount = 0;

    void Start()
    {
        SetRandomWeakness();
    }

    // ==========================================
    // RANDOM WEAKNESS
    // ==========================================

    void SetRandomWeakness()
    {
        weakness =
            (CubeColor)Random.Range(
                0,
                4
            );

        UpdateCubeColor();
    }

    // ==========================================
    // UPDATE CUBE COLOR
    // ==========================================

    void UpdateCubeColor()
    {
        if (cubeRenderer == null)
        {
            return;
        }

        switch (weakness)
        {
            case CubeColor.Red:
                cubeRenderer.material.color = Color.red;
                break;

            case CubeColor.Blue:
                cubeRenderer.material.color = Color.blue;
                break;

            case CubeColor.Green:
                cubeRenderer.material.color = Color.green;
                break;

            case CubeColor.Yellow:
                cubeRenderer.material.color = Color.yellow;
                break;
        }
    }

    // ==========================================
    // REGISTER HIT
    // ==========================================

    public void RegisterHit()
    {
        hitCount++;

        if (hitCount >= hitsBeforeChange)
        {
            hitCount = 0;

            SetRandomWeakness();
        }
    }

    // ==========================================
    // CHECK BULLET COLOR
    // ==========================================

    public bool IsWeaknessColor(Color bulletColor)
    {
        Color weaknessColor =
            GetWeaknessColor();

        return Mathf.Abs(
                   bulletColor.r -
                   weaknessColor.r
               ) < 0.05f
               &&
               Mathf.Abs(
                   bulletColor.g -
                   weaknessColor.g
               ) < 0.05f
               &&
               Mathf.Abs(
                   bulletColor.b -
                   weaknessColor.b
               ) < 0.05f;
    }

    // ==========================================
    // GET WEAKNESS COLOR
    // ==========================================

    public Color GetWeaknessColor()
    {
        switch (weakness)
        {
            case CubeColor.Red:
                return Color.red;

            case CubeColor.Blue:
                return Color.blue;

            case CubeColor.Green:
                return Color.green;

            case CubeColor.Yellow:
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
}