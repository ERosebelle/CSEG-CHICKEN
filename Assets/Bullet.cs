using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 10f;

    [Header("Lifetime")]
    public float lifetime = 3f;

    private float timer;

    void OnEnable()
    {
        timer = 0f;
    }

    void Update()
    {
        transform.position +=
            transform.forward *
            speed *
            Time.deltaTime;

        timer += Time.deltaTime;

        if (timer >= lifetime)
        {
            gameObject.SetActive(false);
        }
    }
}