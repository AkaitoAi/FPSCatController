using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController), typeof(CatMovement), typeof(CatInput))]
public class CatController : MonoBehaviour
{
    [SerializeField] private CatMovement movement;
    [SerializeField] private CatCamera camera;
    [SerializeField] private CatAnimator animator;

    // Unity Events
    [SerializeField] private UnityEvent onJump;
    [SerializeField] private UnityEvent onLand;
    [SerializeField] private UnityEvent onGrab;
    [SerializeField] private UnityEvent onAttack;

    // C# Events
    public event System.Action OnJump;
    public event System.Action OnLand;
    public event System.Action OnGrab;
    public event System.Action OnAttack;

    private MoveCommand moveCommand;
    private LookCommand lookCommand;

    public CatMovement Movement => movement;
    public CatCamera Camera => camera;
    public IAnimatorController Animator => animator;

    void Start()
    {
        if (movement == null || camera == null || animator == null)
        {
            Debug.LogError("Missing required components in CatController. Disabling component.");
            enabled = false;
            return;
        }

        moveCommand = new MoveCommand(movement);
        lookCommand = new LookCommand(camera);

        movement.OnGroundedChanged += HandleGroundedChanged;
        animator.OnJumpTriggered += HandleJumpTriggered;
        animator.OnGrabTriggered += HandleGrabTriggered;
        animator.OnAttackTriggered += HandleAttackTriggered;
    }

    void OnDestroy()
    {
        movement.OnGroundedChanged -= HandleGroundedChanged;
        animator.OnJumpTriggered -= HandleJumpTriggered;
        animator.OnGrabTriggered -= HandleGrabTriggered;
        animator.OnAttackTriggered -= HandleAttackTriggered;
    }

    public void UpdateState(InputState state)
    {
        moveCommand.UpdateInput(state.Move);
        lookCommand.UpdateInput(state.Look);
        ExecuteCommand(moveCommand);
        ExecuteCommand(lookCommand);
        animator.UpdateAnimations(movement.IsGrounded, state.Move.magnitude, state.IsGrabbing);
    }

    public void ExecuteCommand(ICommand command)
    {
        command.Execute();

        switch (command)
        {
            case JumpCommand _:
                if (movement.IsGrounded)
                {
                    animator.TriggerJump(); // Directly trigger jump animation
                    OnJump?.Invoke();
                    onJump?.Invoke();
                }
                break;
            case GrabCommand _:
                OnGrab?.Invoke();
                onGrab?.Invoke();
                break;
            case AttackCommand _:
                if (movement.IsGrounded)
                {
                    OnAttack?.Invoke();
                    onAttack?.Invoke();
                }
                break;
        }
    }

    private void HandleGroundedChanged(bool isGrounded)
    {
        if (isGrounded)
        {
            OnLand?.Invoke();
            onLand?.Invoke();
        }
    }

    private void HandleJumpTriggered()
    {
        OnJump?.Invoke();
        onJump?.Invoke();
    }

    private void HandleGrabTriggered()
    {
        OnGrab?.Invoke();
        onGrab?.Invoke();
    }

    private void HandleAttackTriggered()
    {
        OnAttack?.Invoke();
        onAttack?.Invoke();
    }
}