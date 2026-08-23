using UnityEngine;

public class FloatingAnimation : MonoBehaviour
{
    [Header("Floating")]
    public float speed = 2f;
    public float height = 0.5f;

    private float startY;
    private float timeOffset;

    void Start()
    {
        startY = transform.localPosition.y;
        timeOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    void Update()
    {
        float offset =
            Mathf.Sin(
                Time.time * speed + timeOffset
            ) * height;

        Vector3 position =
            transform.localPosition;

        position.y =
            startY + offset;

        transform.localPosition =
            position;
    }
}