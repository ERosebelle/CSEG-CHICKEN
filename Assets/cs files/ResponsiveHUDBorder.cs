using UnityEngine;

public class ResponsiveHUDBorder : MonoBehaviour
{
    private RectTransform borderImage;

    void Awake()
    {
        borderImage = GetComponent<RectTransform>();
    }

    void Start()
    {
        UpdateBorder();
    }

    void UpdateBorder()
    {
        if (borderImage == null)
            return;

        borderImage.anchorMin = new Vector2(0f, 0f);
        borderImage.anchorMax = new Vector2(1f, 1f);

        borderImage.offsetMin = Vector2.zero;
        borderImage.offsetMax = Vector2.zero;

        borderImage.pivot = new Vector2(0.5f, 0.5f);
    }
}