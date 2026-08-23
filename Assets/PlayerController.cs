using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 9f;

    [Header("Jump")]
    public float normalJumpForce = 5f;
    public float highJumpForce = 10f;
    public float maxHoldTime = 0.6f;

    [Header("Auto Step")]
    public float stepHeight = 0.5f;
    public float stepCheckDistance = 0.5f;
    public float stepSmooth = 5f;

    private Rigidbody rb;
    private Vector2 moveInput;

    private bool isGrounded;
    private bool isHoldingJump;
    private float jumpHoldTime;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        HandleJump();
    }

    void FixedUpdate()
    {
        float currentSpeed = walkSpeed;

        if (Keyboard.current.leftShiftKey.isPressed ||
            Keyboard.current.rightShiftKey.isPressed)
        {
            currentSpeed = runSpeed;
        }

        Vector3 move =
            (transform.forward * moveInput.y +
             transform.right * moveInput.x) *
            currentSpeed;

        rb.linearVelocity = new Vector3(
            move.x,
            rb.linearVelocity.y,
            move.z
        );

        HandleAutoStep();
    }

    void HandleJump()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame &&
            isGrounded)
        {
            isHoldingJump = true;
            jumpHoldTime = 0f;

            rb.AddForce(
                Vector3.up * normalJumpForce,
                ForceMode.Impulse
            );

            isGrounded = false;
        }

        if (isHoldingJump &&
            Keyboard.current.spaceKey.isPressed)
        {
            jumpHoldTime += Time.deltaTime;

            jumpHoldTime = Mathf.Clamp(
                jumpHoldTime,
                0f,
                maxHoldTime
            );
        }

        if (isHoldingJump &&
            Keyboard.current.spaceKey.wasReleasedThisFrame)
        {
            isHoldingJump = false;

            float holdPercent =
                jumpHoldTime / maxHoldTime;

            float extraForce =
                (highJumpForce - normalJumpForce) *
                holdPercent;

            rb.AddForce(
                Vector3.up * extraForce,
                ForceMode.Impulse
            );
        }
    }

    void HandleAutoStep()
    {
        if (moveInput.sqrMagnitude <= 0.01f)
            return;

        Vector3 direction =
            (transform.forward * moveInput.y +
             transform.right * moveInput.x).normalized;

        // Check for an obstacle near the player's feet
        Vector3 lowerOrigin =
            transform.position +
            Vector3.up * 0.1f;

        if (!Physics.Raycast(
            lowerOrigin,
            direction,
            out RaycastHit lowerHit,
            stepCheckDistance))
        {
            return;
        }

        // Check if there is something blocking the top
        Vector3 upperOrigin =
            transform.position +
            Vector3.up * stepHeight;

        if (Physics.Raycast(
            upperOrigin,
            direction,
            stepCheckDistance))
        {
            return;
        }

        // Look for walkable ground above the obstacle
        Vector3 groundCheckOrigin =
            lowerHit.point +
            Vector3.up * stepHeight +
            direction * 0.1f;

        if (Physics.Raycast(
            groundCheckOrigin,
            Vector3.down,
            out RaycastHit groundHit,
            stepHeight + 0.3f))
        {
            Vector3 targetPosition =
                rb.position;

            targetPosition.y =
                groundHit.point.y;

            rb.MovePosition(
                Vector3.Lerp(
                    rb.position,
                    targetPosition,
                    stepSmooth *
                    Time.fixedDeltaTime
                )
            );
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            // Only consider surfaces underneath
            // the player as ground
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                return;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}