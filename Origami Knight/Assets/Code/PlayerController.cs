using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float gravityScale;
    [SerializeField] private KeyCode moveLeft;
    [SerializeField] private KeyCode moveRight;
    [SerializeField] private KeyCode jumpKey;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;

    private Rigidbody2D rb;
    private Vector2 velocity;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        CheckGrounded();
        ApplyGravity();
        ApplyVelocity();
    }

    void HandleInput()
    {
        // Horizontal movement
        float moveDirection = 0f;
        if (Input.GetKey(moveLeft))
        {
            moveDirection = -1f;
        }
        else if (Input.GetKey(moveRight))
        {
            moveDirection = 1f;
        }
        velocity.x = moveDirection * moveSpeed;

        // Jump
        if (Input.GetKeyDown(jumpKey) && isGrounded)
        {
            velocity.y = jumpForce;
        }

        // Cut jump short when releasing jump key
        if (Input.GetKeyUp(jumpKey) && velocity.y > 0)
        {
            velocity.y *= 0.5f;
        }
    }

    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void ApplyGravity()
    {
        velocity.y += Physics2D.gravity.y * gravityScale * Time.fixedDeltaTime;
    }

    void ApplyVelocity()
    {
        rb.linearVelocity = velocity;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
