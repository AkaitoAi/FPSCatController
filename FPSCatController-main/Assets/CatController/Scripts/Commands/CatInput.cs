using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class CatInput : MonoBehaviour
{
    [SerializeField] private CatController controller;
    private PlayerInput playerInput;
    private InputState inputState;

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

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        controller.UpdateState(inputState);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputState.Move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        inputState.Look = context.ReadValue<Vector2>();
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