using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move Variables")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Jump Variables")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float lowJumpMultiplier;
    [SerializeField] private float fallMultiplier;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;
    private bool isGrounded;
    
    private float moveInput;
    private bool shouldJump;
    private bool holdingJump;

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

    private void Jump() 
    {
        if (!isGrounded)
            return;

        if (rb.velocity.y < 0) 
            rb.velocity = new Vector2(rb.velocity.x, 0);

        rb.AddForce(Vector2.up * jumpForce);

        shouldJump = false;
    }

    private void GravityControl() 
    {
        if (rb.velocity.y < 0)
            rb.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
        else if (rb.velocity.y > 0 && !holdingJump)
            rb.velocity += Vector2.up * Physics2D.gravity.y * lowJumpMultiplier * Time.deltaTime;
    }

    public void SetMoveInput(float moveInput) 
    {
        this.moveInput = moveInput;
    }

    public void SetShouldJumpInput(bool jumpInput) 
    {
        shouldJump = jumpInput;
    }

    public void SetJumpButtonPress(bool jumpInput)
    {
        holdingJump = jumpInput;
    }
}
