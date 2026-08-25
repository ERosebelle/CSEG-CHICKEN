using UnityEngine;

public class ChickenAnimation : MonoBehaviour
{
    public float bobHeight = 0.05f;
    public float bobSpeed = 3f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition;
    }

    void Update()
    {
        float bob = Mathf.Sin(Time.time * bobSpeed) * bobHeight;

        transform.localPosition =
            startPosition + Vector3.up * bob;
    }
}