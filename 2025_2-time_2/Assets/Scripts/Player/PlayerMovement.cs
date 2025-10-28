using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 5f;
    private float moveInput;

    [Header("Pulo")]
    public float jumpForce = 5f;
    private bool shouldJump;
    private bool holdingJump;
    private bool jumpPressed;
    private bool isGrounded;

    [Header("Detecção de chão")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (shouldJump)
            Jump();
        GravityControl();
    }

    private void Update()
    {
        if (jumpPressed && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * 1f * Time.deltaTime;
        } 

        jumpPressed = false; 
    }

    private void Jump() 
    {
        if (!isGrounded)
            return;

        rb.AddForce(Vector2.up * jumpForce);

        shouldJump = false;
    }

    private void GravityControl() 
    {
        if (rb.velocity.y < 0)
            rb.velocity += Vector2.up * Physics2D.gravity.y * Time.deltaTime;
    }

    public void SetMoveInput(float moveInput) 
    {
        this.moveInput = moveInput;
    }

    public void SetJumpInput(bool jumpInput) 
    {
        shouldJump = jumpInput;
        holdingJump = jumpInput;
    }
}
