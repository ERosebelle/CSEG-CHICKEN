using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 9f;

    [Header("Run Toggle")]
    public bool runMode = false;

    [Header("Jump")]
    public float normalJumpForce = 5f;
    public float highJumpForce = 10f;
    public float maxHoldTime = 0.6f;

    [Header("Auto Step")]
    public float stepHeight = 0.5f;
    public float stepCheckDistance = 0.5f;
    public float stepSmooth = 5f;

    [Header("Double Jump")]
    public float doubleJumpForce = 40f;

    private Rigidbody rb;
    private Vector2 moveInput;

    private bool isGrounded;
    private bool isHoldingJump;
    private float jumpHoldTime;

    private bool hasUsedDoubleJump = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;

        // Force the double jump value to 40
        // even if Unity's Inspector has an old saved value.
        doubleJumpForce = 40f;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        HandleRunToggle();
        HandleJump();
    }

    void FixedUpdate()
    {
        float currentSpeed;

        if (runMode)
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
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

    // ==========================================
    // RUN TOGGLE
    // ==========================================

    void HandleRunToggle()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.leftShiftKey.wasPressedThisFrame ||
            Keyboard.current.rightShiftKey.wasPressedThisFrame)
        {
            runMode = !runMode;

            if (runMode)
            {
                Debug.Log("RUN MODE: ON");
            }
            else
            {
                Debug.Log("RUN MODE: OFF | WALK MODE");
            }
        }
    }

    // ==========================================
    // JUMP
    // ==========================================

    void HandleJump()
    {
        if (Keyboard.current == null)
            return;

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

    // ==========================================
    // AUTO STEP
    // ==========================================

    void HandleAutoStep()
    {
        if (moveInput.sqrMagnitude <= 0.01f)
            return;

        Vector3 direction =
            (transform.forward * moveInput.y +
             transform.right * moveInput.x).normalized;

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

    // ==========================================
    // INPUT
    // ==========================================

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // ==========================================
    // DOUBLE JUMP
    // ==========================================

    public bool CanDoubleJump()
    {
        return !isGrounded && !hasUsedDoubleJump;
    }

    public void PerformDoubleJump()
    {
        if (isGrounded)
            return;

        if (hasUsedDoubleJump)
            return;

        // Use PlayerController's own double jump force
        rb.linearVelocity = new Vector3(
            rb.linearVelocity.x,
            doubleJumpForce,
            rb.linearVelocity.z
        );

        // Mark double jump as used
        hasUsedDoubleJump = true;

        // Stop normal jump-hold system
        isHoldingJump = false;

        Debug.Log(
            "DOUBLE JUMP! Force: " + doubleJumpForce
        );
    }

    // ==========================================
    // GROUND DETECTION
    // ==========================================

    void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;

                // Reset double jump when landing
                hasUsedDoubleJump = false;

                return;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}