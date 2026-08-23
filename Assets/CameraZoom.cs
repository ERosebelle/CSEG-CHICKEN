using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    [Header("Zoom")]
    public float normalFOV = 60f;
    public float zoomFOV = 20f;
    public Key zoomKey = Key.Z;

    [Header("Look")]
    public float sensitivity = 150f;
    public float minPitch = -80f;
    public float maxPitch = 80f;

    [Header("Player")]
    public Transform player;

    [Header("Camera")]
    public CameraInitial cameraInitial;

    private Camera cam;
    private Renderer[] playerRenderers;

    private bool isZoomed;
    private float pitch;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Start()
    {
        if (cam == null)
        {
            Debug.LogError(
                "CameraZoom must be attached to Main Camera."
            );

            enabled = false;
            return;
        }

        if (player != null)
        {
            playerRenderers =
                player.GetComponentsInChildren<Renderer>();
        }

        cam.fieldOfView = normalFOV;
    }

    void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current[zoomKey].wasPressedThisFrame)
        {
            ToggleZoom();
        }

        if (isZoomed)
        {
            Look();
        }
    }

    void LateUpdate()
    {
        if (!isZoomed || player == null)
            return;

        transform.position = player.position;

        transform.rotation =
            Quaternion.Euler(
                pitch,
                player.eulerAngles.y,
                0f
            );
    }

    void ToggleZoom()
    {
        isZoomed = !isZoomed;

        if (isZoomed)
        {
            if (cameraInitial != null)
                cameraInitial.IsActive = false;

            cam.fieldOfView = zoomFOV;

            SetPlayerVisible(false);

            pitch = 0f;

            transform.position = player.position;

            transform.rotation =
                Quaternion.Euler(
                    0f,
                    player.eulerAngles.y,
                    0f
                );
        }
        else
        {
            if (cameraInitial != null)
                cameraInitial.IsActive = true;

            cam.fieldOfView = normalFOV;

            SetPlayerVisible(true);
        }
    }

    void Look()
    {
        if (Mouse.current == null || player == null)
            return;

        Vector2 delta =
            Mouse.current.delta.ReadValue();

        float mouseX =
            delta.x *
            sensitivity *
            Time.deltaTime;

        float mouseY =
            delta.y *
            sensitivity *
            Time.deltaTime;

        player.Rotate(
            0f,
            mouseX,
            0f
        );

        pitch -= mouseY;

        pitch =
            Mathf.Clamp(
                pitch,
                minPitch,
                maxPitch
            );
    }

    void SetPlayerVisible(bool visible)
    {
        if (playerRenderers == null)
            return;

        foreach (Renderer renderer in playerRenderers)
        {
            renderer.enabled = visible;
        }
    }
}