using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CatMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float movementSmoothing = 0.1f; // New: Controls acceleration/deceleration
    [SerializeField] private float coyoteTime = 0.2f; // New: Time window for late jumps

    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 currentMoveVelocity; // New: For smoothing movement
    private Vector3 targetMoveVelocity; // New: For smoothing movement
    private bool wasGrounded;
    public bool IsGrounded { get; private set; }
    private float lastGroundedTime; // New: Tracks last time grounded for coyote time
    private bool canJump; // New: Tracks jump eligibility with coyote time

    public event System.Action<bool> OnGroundedChanged;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        wasGrounded = controller.isGrounded;
    }

    void Update()
    {
        // Update coyote time
        if (IsGrounded)
        {
            lastGroundedTime = Time.time;
            canJump = true;
        }
        else if (Time.time > lastGroundedTime + coyoteTime)
        {
            canJump = false;
        }
    }

    public void Move(Vector2 input)
    {
        IsGrounded = controller.isGrounded;

        if (IsGrounded != wasGrounded)
        {
            OnGroundedChanged?.Invoke(IsGrounded);
            wasGrounded = IsGrounded;
        }

        // Calculate target movement velocity
        Vector3 moveDirection = new Vector3(input.x, 0f, input.y).normalized;
        moveDirection = Quaternion.Euler(0, transform.eulerAngles.y, 0) * moveDirection;
        targetMoveVelocity = moveDirection * moveSpeed;

        // Smooth the current velocity towards target velocity
        currentMoveVelocity = Vector3.Lerp(currentMoveVelocity, targetMoveVelocity,
            movementSmoothing > 0 ? Time.deltaTime / movementSmoothing : 1f);

        // Apply smoothed movement
        controller.Move(currentMoveVelocity * Time.deltaTime);

        // Handle vertical velocity
        if (IsGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (canJump) // Modified: Use coyote time check instead of just IsGrounded
        {
            velocity.y = jumpForce;
            canJump = false; // Prevent multiple jumps during coyote time
        }
    }
}