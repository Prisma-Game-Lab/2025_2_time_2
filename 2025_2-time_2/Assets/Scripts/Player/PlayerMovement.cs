using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [Header("Move Variables")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Jump Variables")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float lowJumpMultiplier;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float jumpCooldown;
    private bool jumpOnCooldown;

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
        rb = playerController.rb;
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        PeformGroundCheck();

        if (shouldJump)
            Jump();
        GravityControl();
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

    public void SetMovement(bool state) 
    {
        if (state) 
        {
            if (playerController.GetCurrentPlayerState() == PlayerController.PlayerState.Blocked)
                playerController.SetCurrentPlayerState(PlayerController.PlayerState.Idle);
        } 
        else
        {
            playerController.SetCurrentPlayerState(PlayerController.PlayerState.Blocked);
        }

    }

    private void ApplyMovement()
    {
        if (playerController.GetCurrentPlayerState() == PlayerController.PlayerState.Blocked)
            return;

        if (moveInput < 0) 
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        }
        else if (moveInput > 0) 
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }

        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        float xVelocityAbs = Mathf.Abs(rb.velocity.x);

        switch (playerController.GetCurrentPlayerState())
        {
            case PlayerController.PlayerState.Idle:
                if (xVelocityAbs > 0)
                    playerController.SetCurrentPlayerState(PlayerController.PlayerState.Running);
                break;
            case PlayerController.PlayerState.Running:
                if (Mathf.Approximately(xVelocityAbs, 0))
                    playerController.SetCurrentPlayerState(PlayerController.PlayerState.Idle);
                break;
            default:
                break;
        }
    }

    private void PeformGroundCheck()
    {
        if (jumpOnCooldown)
        {
            isGrounded = false;
            return;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (isGrounded && playerController.GetCurrentPlayerState() == PlayerController.PlayerState.Jumping)
            playerController.SetCurrentPlayerState(PlayerController.PlayerState.Idle);
    }

    private void Jump()
    {
        if (!isGrounded || jumpOnCooldown || playerController.GetCurrentPlayerState() == PlayerController.PlayerState.Blocked)
            return;

        if (rb.velocity.y < 0)
            rb.velocity = new Vector2(rb.velocity.x, 0);

        rb.AddForce(Vector2.up * jumpForce);

        playerController.SetCurrentPlayerState(PlayerController.PlayerState.Jumping);

        shouldJump = false;
        StartCoroutine(JumpCooldown());
    }

    private IEnumerator JumpCooldown() 
    {
        jumpOnCooldown = true;
        yield return new WaitForSeconds(jumpCooldown);
        jumpOnCooldown = false;
    }

    private void GravityControl()
    {
        if (rb.velocity.y < 0)
            rb.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
        else if (rb.velocity.y > 0 && !holdingJump)
            rb.velocity += Vector2.up * Physics2D.gravity.y * lowJumpMultiplier * Time.deltaTime;
    }
}
