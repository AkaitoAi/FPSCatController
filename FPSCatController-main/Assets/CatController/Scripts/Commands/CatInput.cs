using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class CatInput : MonoBehaviour
{
    [SerializeField] private CatController controller;
    [SerializeField] private LookAreaInput lookArea; // Optional: Reference to look area
    [SerializeField] private float mouseSensitivity = 1f; // Sensitivity for mouse input
    [SerializeField] private float touchSensitivity = 1f; // Sensitivity for UI drag input
    [SerializeField] private float lookSmoothing = 0.1f; // New: Smoothing factor for look input

    private PlayerInput playerInput;
    private InputState inputState;
    private bool useLookArea; // Flag to determine which input method to use
    private Vector2 currentLookVelocity; // New: Current smoothed look value
    private Vector2 targetLookVelocity; // New: Target look value to smooth towards

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        inputState = new InputState();

        if (controller == null)
        {
            Debug.LogError("CatController is not assigned in CatInput. Disabling component.");
            enabled = false;
            return;
        }

        // Determine input method based on whether lookArea is assigned
        useLookArea = lookArea != null;
        if (!useLookArea)
        {
            Debug.Log("LookAreaInput not assigned. Falling back to mouse input.");
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (lookArea == null)
        {
            Debug.LogError("LookAreaInput reference is null but was expected. Disabling component.");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        if (useLookArea)
        {
            // Use UI drag input with sensitivity
            Vector2 rawLookInput = lookArea.LookInput * touchSensitivity;
            targetLookVelocity = rawLookInput;
        }

        // Smooth the look input
        currentLookVelocity = Vector2.Lerp(currentLookVelocity, targetLookVelocity,
            lookSmoothing > 0 ? Time.deltaTime / lookSmoothing : 1f);
        inputState.Look = currentLookVelocity;

        controller.UpdateState(inputState);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputState.Move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        // Only process mouse input with sensitivity if we're not using LookAreaInput
        if (!useLookArea)
        {
            Vector2 rawMouseInput = context.ReadValue<Vector2>() * mouseSensitivity;
            targetLookVelocity = rawMouseInput;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            controller.ExecuteCommand(new JumpCommand(controller.Movement));
        }
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        inputState.IsGrabbing = context.ReadValueAsButton();
        if (context.performed)
        {
            controller.ExecuteCommand(new GrabCommand((CatAnimator)controller.Animator));
        }
    }

    public void OnAttach(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            controller.ExecuteCommand(new AttackCommand((CatAnimator)controller.Animator, controller.Movement));
        }
    }
}