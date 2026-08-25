using UnityEngine;
using UnityEngine.InputSystem;

public class CameraInitial : MonoBehaviour
{
    [Header("Camera")]
    public Transform player;
    public Transform cameraInitialState;
    public Camera playerCamera;

    [Header("Orbit")]
    public float mouseSensitivity = 0.2f;
    public float minVerticalAngle = -20f;
    public float maxVerticalAngle = 70f;
    public float rotateSpeed = 10f;

    private float yaw;
    private float pitch;
    private float distance;

    public bool IsActive { get; set; } = true;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (player == null || cameraInitialState == null)
            return;

        Vector3 direction =
            cameraInitialState.position -
            player.position;

        distance = direction.magnitude;

        yaw =
            Mathf.Atan2(
                direction.x,
                direction.z
            ) * Mathf.Rad2Deg;

        pitch =
            -Mathf.Asin(
                direction.y / distance
            ) * Mathf.Rad2Deg;
    }

    void Update()
    {
        if (!IsActive)
            return;

        if (Mouse.current != null &&
            Mouse.current.leftButton.isPressed)
        {
            Vector2 delta =
                Mouse.current.delta.ReadValue();

            yaw +=
                delta.x *
                mouseSensitivity;

            pitch -=
                delta.y *
                mouseSensitivity;

            pitch = Mathf.Clamp(
                pitch,
                minVerticalAngle,
                maxVerticalAngle
            );
        }

        RotatePlayerToCamera();
    }

    void LateUpdate()
    {
        if (!IsActive)
            return;

        if (player == null || playerCamera == null)
            return;

        Quaternion rotation =
            Quaternion.Euler(
                pitch,
                yaw,
                0f
            );

        Vector3 offset =
            rotation *
            Vector3.back *
            distance;

        Vector3 target =
            player.position;

        playerCamera.transform.position =
            target + offset;

        playerCamera.transform.LookAt(target);
    }

    void RotatePlayerToCamera()
    {
        if (player == null)
            return;

        if (Keyboard.current == null)
            return;

        bool moving =
            Keyboard.current.wKey.isPressed ||
            Keyboard.current.aKey.isPressed ||
            Keyboard.current.sKey.isPressed ||
            Keyboard.current.dKey.isPressed ||
            Keyboard.current.upArrowKey.isPressed ||
            Keyboard.current.downArrowKey.isPressed ||
            Keyboard.current.leftArrowKey.isPressed ||
            Keyboard.current.rightArrowKey.isPressed;

        if (!moving)
            return;

        Quaternion targetRotation =
            Quaternion.Euler(
                0f,
                yaw,
                0f
            );

        player.rotation =
            Quaternion.Slerp(
                player.rotation,
                targetRotation,
                rotateSpeed * Time.deltaTime
            );
    }
}