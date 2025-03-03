using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CatMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.81f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool wasGrounded;
    public bool IsGrounded { get; private set; }

    public event System.Action<bool> OnGroundedChanged;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        wasGrounded = controller.isGrounded;
    }

    public void Move(Vector2 input)
    {
        IsGrounded = controller.isGrounded;

        if (IsGrounded != wasGrounded)
        {
            OnGroundedChanged?.Invoke(IsGrounded);
            wasGrounded = IsGrounded;
        }

        Vector3 moveDirection = new Vector3(input.x, 0f, input.y).normalized;
        moveDirection = Quaternion.Euler(0, transform.eulerAngles.y, 0) * moveDirection;
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);

        if (IsGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void Jump()
    {
        velocity.y = jumpForce;
    }
}