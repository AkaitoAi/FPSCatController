using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class CatController : MonoBehaviour
{
    // Components
    [SerializeField] private Animator animator;
    private CharacterController controller;
    private CinemachineVirtualCamera cinemachineCamera;
    private PlayerInput playerInput;

    // Movement variables
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float cameraSensitivity = 2f;

    // Input values
    private Vector2 moveInput;
    private Vector2 lookInput;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isGrabbing;

    // Camera rotation tracking
    private float cameraPitch = 0f;  // Vertical rotation
    private float characterYaw = 0f; // Horizontal rotation

    // Animator parameter hashes
    private static readonly int GrabHash = Animator.StringToHash("grab");
    private static readonly int AttachHash = Animator.StringToHash("attack");
    private static readonly int IsWalkingHash = Animator.StringToHash("isWalking");
    private static readonly int WalkStateHash = Animator.StringToHash("walk_state");
    private static readonly int IdleStateHash = Animator.StringToHash("idle_state");
    private static readonly int JumpHash = Animator.StringToHash("jump");

    void Start()
    {
        // Initialize components
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cinemachineCamera = GetComponentInChildren<CinemachineVirtualCamera>();

        // Lock cursor for FPS control
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleGroundCheck();
        HandleMovement();
        HandleCameraRotation();
        HandleAnimations();
        ApplyGravity();
    }

    // Input System handlers
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            velocity.y = jumpForce;
            animator.SetTrigger(JumpHash);
        }
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        isGrabbing = context.ReadValueAsButton();
        if (context.performed)
        {
            animator.SetTrigger(GrabHash);
        }
    }

    public void OnAttach(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            animator.SetTrigger(AttachHash);
        }
    }

    private void HandleGroundCheck()
    {
        isGrounded = controller.isGrounded;
    }

    private void HandleMovement()
    {
        // Calculate 3D movement direction based on camera orientation
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y).normalized;

        // Transform direction from local to world space relative to camera yaw
        moveDirection = Quaternion.Euler(0, characterYaw, 0) * moveDirection;

        // Apply movement
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void HandleCameraRotation()
    {
        // Horizontal rotation (character and camera)
        characterYaw += lookInput.x * cameraSensitivity * rotationSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, characterYaw, 0f);

        // Vertical rotation (camera only)
        cameraPitch -= lookInput.y * cameraSensitivity * rotationSpeed * Time.deltaTime;
        cameraPitch = Mathf.Clamp(cameraPitch, -90f, 90f);

        // Apply vertical rotation to Cinemachine camera
        cinemachineCamera.transform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    private void ApplyGravity()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small downward force to keep grounded
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleAnimations()
    {
        float speed = moveInput.magnitude;

        // Set walking state
        animator.SetBool(IsWalkingHash, speed > 0.1f && isGrounded);

        if (isGrounded)
        {
            if (speed > 0.1f)
            {
                // Walk Blend Tree: 0 = Simple Walk, 1 = Grab Walk
                animator.SetFloat(WalkStateHash, isGrabbing ? 1f : 0f);
                animator.SetFloat(IdleStateHash, 0f); // Reset idle when walking
            }
            else
            {
                // Idle Blend Tree: 0 = Idle, 1 = Idle Grab
                animator.SetFloat(IdleStateHash, isGrabbing ? 1f : 0f);
                animator.SetFloat(WalkStateHash, 0f); // Reset walk when idle
            }
        }
        else
        {
            // Reset both blend trees when in air
            animator.SetFloat(WalkStateHash, 0f);
            animator.SetFloat(IdleStateHash, 0f);
        }
    }
}