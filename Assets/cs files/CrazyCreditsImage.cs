using UnityEngine;

public class CrazyCreditsImage : MonoBehaviour
{
    private RectTransform rect;

    private Vector2 startPosition;
    private Vector3 startScale;

    private float timer = 0f;

    [Header("Intro Zoom")]
    public float zoomDuration = 3f;
    public float zoomAmount = 2.5f;

    [Header("Movement")]
    public float moveAmount = 100f;
    public float moveSpeed = 7f;

    [Header("Rotation")]
    public float rotationSpeed = 360f;

    [Header("Scaling")]
    public float scaleAmount = 0.5f;
    public float scaleSpeed = 6f;

    [Header("Shake")]
    public float shakeAmount = 20f;
    public float shakeSpeed = 35f;

    [Header("Random Teleport")]
    public bool randomTeleport = true;
    public float teleportInterval = 1.5f;
    public float teleportRange = 300f;

    private float teleportTimer;

    void Start()
    {
        rect = GetComponent<RectTransform>();

        startPosition = rect.anchoredPosition;
        startScale = rect.localScale;

        timer = 0f;
        teleportTimer = teleportInterval;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // ==========================================
        // FIRST 3 SECONDS - DRAMATIC ZOOM
        // ==========================================

        if (timer < zoomDuration)
        {
            float progress =
                timer / zoomDuration;

            // Smooth zoom
            float zoom =
                Mathf.SmoothStep(
                    1f,
                    zoomAmount,
                    progress
                );

            rect.localScale =
                startScale * zoom;

            // Keep picture still during intro
            rect.anchoredPosition =
                startPosition;

            return;
        }

        // ==========================================
        // CHAOS STARTS
        // ==========================================

        float x =
            Mathf.Sin(Time.time * moveSpeed) *
            moveAmount;

        float y =
            Mathf.Cos(Time.time * moveSpeed * 1.3f) *
            moveAmount;

        Vector2 movement =
            new Vector2(x, y);

        // ==========================================
        // VIOLENT SHAKE
        // ==========================================

        float shakeX =
            Mathf.Sin(Time.time * shakeSpeed) *
            shakeAmount;

        float shakeY =
            Mathf.Cos(Time.time * shakeSpeed * 1.4f) *
            shakeAmount;

        movement +=
            new Vector2(
                shakeX,
                shakeY
            );

        rect.anchoredPosition =
            startPosition + movement;

        // ==========================================
        // SPIN
        // ==========================================

        rect.Rotate(
            0f,
            0f,
            rotationSpeed *
            Time.deltaTime
        );

        // ==========================================
        // CRAZY PULSE
        // ==========================================

        float scale =
            zoomAmount +
            Mathf.Sin(
                Time.time * scaleSpeed
            ) * scaleAmount;

        rect.localScale =
            startScale * scale;

        // ==========================================
        // RANDOM TELEPORT
        // ==========================================

        if (randomTeleport)
        {
            teleportTimer -=
                Time.deltaTime;

            if (teleportTimer <= 0f)
            {
                Vector2 randomPosition =
                    startPosition +
                    new Vector2(
                        Random.Range(
                            -teleportRange,
                            teleportRange
                        ),
                        Random.Range(
                            -teleportRange,
                            teleportRange
                        )
                    );

                rect.anchoredPosition =
                    randomPosition;

                teleportTimer =
                    teleportInterval;
            }
        }
    }
}