public interface IAnimatorController
{
    void UpdateAnimations(bool isGrounded, float speed, bool isGrabbing);
    void TriggerJump();
    void TriggerGrab();
    void TriggerAttack();
}