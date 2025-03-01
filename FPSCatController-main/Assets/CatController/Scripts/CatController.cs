using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class CatController : MonoBehaviour
{
    // Components
    private CharacterController controller;
    private Animator animator;
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

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
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
            animator.SetTrigger("Jump");
        }
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        isGrabbing = context.ReadValueAsButton();
        if (context.performed)
        {
            animator.SetTrigger("Grab");
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

        // Transform direction from local to world space relative to camera
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

        // Apply vertical rotation to camera
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

        animator.SetFloat("Speed", speed);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsGrabbing", isGrabbing);

        // Animation state machine
        if (isGrounded)
        {
            if (speed > 0.1f)
            {
                animator.Play(isGrabbing ? "WalkGrab" : "Walk");
            }
            else
            {
                animator.Play(isGrabbing ? "IdleGrab" : "Idle");
            }
        }
    }
}