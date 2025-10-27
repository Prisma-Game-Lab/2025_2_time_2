using UnityEngine;
using UnityEngine.InputSystem;

public class MovimentoPlayer : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 5f;
    private Vector2 moveInput;

    [Header("Pulo")]
    public float jumpForce = 5f;
    private bool jumpPressed;
    private bool isGrounded;

    [Header("Detecção de chão")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private GameplayControls controls;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new GameplayControls();

        controls.Move.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Move.Move.canceled += ctx => moveInput = Vector2.zero;

        controls.Jump.Newaction.performed += ctx => jumpPressed = true;
    }

    void OnEnable()
    {
        controls.Move.Enable();
        controls.Jump.Enable();
    }

    void OnDisable()
    {
        controls.Move.Disable();
        controls.Jump.Disable();
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

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
}
