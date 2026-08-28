using UnityEngine;

public class MapColorObject : MonoBehaviour
{
    [Header("Current Color")]
    public Color currentColor = Color.white;

    private Renderer[] renderers;

    void Awake()
    {
        renderers =
            GetComponentsInChildren<Renderer>();
    }

    // ==========================================
    // APPLY COLOR
    // ==========================================

    public void ApplyColor(Color newColor)
    {
        currentColor = newColor;

        if (renderers == null ||
            renderers.Length == 0)
        {
            renderers =
                GetComponentsInChildren<Renderer>();
        }

        foreach (Renderer renderer in renderers)
        {
            if (renderer == null)
                continue;

            renderer.material.color =
                newColor;
        }
    }

    // ==========================================
    // GET CURRENT COLOR
    // ==========================================

    public Color GetCurrentColor()
    {
        return currentColor;
    }

    // ==========================================
    // GET COLOR NAME
    // ==========================================

    public string GetColorName()
    {
        return GetColorName(currentColor);
    }

    public string GetColorName(Color color)
    {
        if (ApproximatelyColor(
            color,
            Color.red))
        {
            return "RED";
        }

        if (ApproximatelyColor(
            color,
            Color.blue))
        {
            return "BLUE";
        }

        if (ApproximatelyColor(
            color,
            Color.green))
        {
            return "GREEN";
        }

        if (ApproximatelyColor(
            color,
            Color.yellow))
        {
            return "YELLOW";
        }

        return "OTHER";
    }

    // ==========================================
    // COLOR COMPARISON
    // ==========================================

    bool ApproximatelyColor(
        Color a,
        Color b
    )
    {
        return Mathf.Abs(a.r - b.r) < 0.05f &&
               Mathf.Abs(a.g - b.g) < 0.05f &&
               Mathf.Abs(a.b - b.b) < 0.05f;
    }
}