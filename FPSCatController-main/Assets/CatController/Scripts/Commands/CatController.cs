using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(CatMovement), typeof(CatInput))]
public class CatController : MonoBehaviour
{
    [SerializeField] private CatMovement movement;
    [SerializeField] private CatCamera camera;
    [SerializeField] private CatAnimator animator;

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
    }
}