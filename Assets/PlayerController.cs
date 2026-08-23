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

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }
}