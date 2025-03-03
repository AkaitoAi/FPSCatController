using UnityEngine;

public class MoveCommand : ICommand
{
    private readonly CatMovement movement;
    private Vector2 input;

    public MoveCommand(CatMovement movement)
    {
        this.movement = movement;
    }

    public void UpdateInput(Vector2 newInput)
    {
        input = newInput;
    }

    public void Execute()
    {
        movement.Move(input);
    }
}

public class LookCommand : ICommand
{
    private readonly CatCamera camera;
    private Vector2 input;

    public LookCommand(CatCamera camera)
    {
        this.camera = camera;
    }

    public void UpdateInput(Vector2 newInput)
    {
        input = newInput;
    }

    public void Execute()
    {
        camera.Rotate(input);
    }
}

public class JumpCommand : ICommand
{
    private readonly CatMovement movement;

    public JumpCommand(CatMovement movement)
    {
        this.movement = movement;
    }

    public void Execute()
    {
        if (movement.IsGrounded)
        {
            movement.Jump();
        }
    }
}

public class GrabCommand : ICommand
{
    private readonly CatAnimator animator;

    public GrabCommand(CatAnimator animator)
    {
        this.animator = animator;
    }

    public void Execute()
    {
        animator.TriggerGrab();
    }
}

public class AttackCommand : ICommand
{
    private readonly CatAnimator animator;
    private readonly CatMovement movement;

    public AttackCommand(CatAnimator animator, CatMovement movement)
    {
        this.animator = animator;
        this.movement = movement;
    }

    public void Execute()
    {
        if (movement.IsGrounded)
        {
            animator.TriggerAttack();
        }
    }
}