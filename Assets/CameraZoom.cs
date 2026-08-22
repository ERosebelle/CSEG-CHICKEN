using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    [Header("Zoom Settings")]
    public float normalFOV = 60f;
    public float zoomFOV = 20f;
    public Key zoomKey = Key.Z;

    [Header("Sniper Look")]
    public float zoomSensitivity = 150f;

    private Camera cam;
    private PlayerController playerController;
    private Renderer[] playerRenderers;

    private bool isZoomed = false;
    private float cameraPitch;

    void Start()
    {
        cam = GetComponent<Camera>();

        if (cam == null)
        {
            Debug.LogError("CameraZoom needs to be attached to a Camera.");
            return;
        }

        playerController = GetComponentInParent<PlayerController>();

        if (playerController != null)
        {
            playerRenderers = playerController.GetComponentsInChildren<Renderer>();
        }

        cam.fieldOfView = normalFOV;
    }

    void Update()
    {
        if (Keyboard.current == null || cam == null)
            return;

        if (Keyboard.current[zoomKey].wasPressedThisFrame)
        {
            ToggleZoom();
        }

        if (isZoomed)
        {
            SniperLook();
        }
    }

    void ToggleZoom()
    {
        isZoomed = !isZoomed;

        if (isZoomed)
        {
            cam.fieldOfView = zoomFOV;
            SetPlayerVisibility(false);
        }
        else
        {
            cam.fieldOfView = normalFOV;
            SetPlayerVisibility(true);
        }
    }

    void SetPlayerVisibility(bool visible)
    {
        if (playerRenderers == null)
            return;

        foreach (Renderer renderer in playerRenderers)
        {
            renderer.enabled = visible;
        }
    }

    void SniperLook()
    {
        Vector2 delta = Mouse.current.delta.ReadValue();

        float mouseX = delta.x * zoomSensitivity * Time.deltaTime;
        float mouseY = delta.y * zoomSensitivity * Time.deltaTime;

        transform.parent.Rotate(0f, mouseX, 0f);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f);

        transform.localEulerAngles = new Vector3(
            cameraPitch,
            0f,
            0f
        );
    }
}
