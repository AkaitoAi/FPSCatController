using UnityEngine;

public class CatAnimator : MonoBehaviour, IAnimatorController
{
    [SerializeField] private Animator animator;

    private static readonly int GrabHash = Animator.StringToHash("grab");
    private static readonly int AttackHash = Animator.StringToHash("attack");
    private static readonly int IsWalkingHash = Animator.StringToHash("isWalking");
    private static readonly int WalkStateHash = Animator.StringToHash("walk_state");
    private static readonly int IdleStateHash = Animator.StringToHash("idle_state");
    private static readonly int JumpHash = Animator.StringToHash("jump");

    public event System.Action OnJumpTriggered;
    public event System.Action OnGrabTriggered;
    public event System.Action OnAttackTriggered;

    void Awake()
    {
        if (animator == null)
        {
            Debug.LogError("Animator is not assigned in CatAnimator!");
        }
    }

    public void UpdateAnimations(bool isGrounded, float speed, bool isGrabbing)
    {
        animator.SetBool(IsWalkingHash, speed > 0.1f && isGrounded);

        if (isGrounded)
        {
            if (speed > 0.1f)
            {
                animator.SetFloat(WalkStateHash, isGrabbing ? 1f : 0f);
                animator.SetFloat(IdleStateHash, 0f);
            }
            else
            {
                animator.SetFloat(IdleStateHash, isGrabbing ? 1f : 0f);
                animator.SetFloat(WalkStateHash, 0f);
            }
        }
        else
        {
            animator.SetFloat(WalkStateHash, 0f);
            animator.SetFloat(IdleStateHash, 0f);
        }
    }

    public void TriggerJump()
    {
        animator.SetTrigger(JumpHash);
        OnJumpTriggered?.Invoke();
    }

    public void TriggerGrab()
    {
        animator.SetTrigger(GrabHash);
        OnGrabTriggered?.Invoke();
    }

    public void TriggerAttack()
    {
        animator.SetTrigger(AttackHash);
        OnAttackTriggered?.Invoke();
    }
}