using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [Header("Move Variables")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float accelerationRate;
    [SerializeField] private float desaccelerationRate;
    [SerializeField] private float minTurnSpeed = 0.01f;
    [SerializeField] private float minSpeed = 0.01f;

    [Header("Jump Variables")]
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float lowJumpMultiplier;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float jumpCooldown;
    private bool jumpOnCooldown;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckWidth = 0.2f;
    [SerializeField] private float groundCheckHeight = 0.2f;
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
        SetOrientation();
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

    private void ApplyMovement()
    {
        float desiredMoveDir = moveInput;

        if (playerController.GetCurrentPlayerState() == PlayerController.PlayerState.Blocked)
            desiredMoveDir = 0;

        float desiredSpeed = desiredMoveDir * moveSpeed;
        float speedDif = desiredSpeed - rb.velocity.x;

        float accelRate;
        if (desiredSpeed * rb.velocity.x >= 0)
        {
            if (Mathf.Abs(desiredSpeed) >= Mathf.Abs(rb.velocity.x))
            {
                accelRate = accelerationRate;
            }
            else
            {
                accelRate = desaccelerationRate;
            }
        }
        else
        {
            accelRate = desaccelerationRate;
        }

        rb.AddForce(Vector2.right * speedDif * accelRate);

        VelocityStateHandler();
    }

    private void SetOrientation() 
    {
        if (playerController.GetCurrentPlayerState() == PlayerController.PlayerState.Blocked)
            return;

        if (rb.velocity.x < -minTurnSpeed)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        }
        else if (rb.velocity.x > minTurnSpeed)
        {
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }
    }

    private void PeformGroundCheck()
    {
        if (jumpOnCooldown)
        {
            isGrounded = false;
            return;
        }

        isGrounded = Physics2D.OverlapBox(groundCheck.position, new Vector2(groundCheckWidth, groundCheckHeight), 0, groundLayer);
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

    private void VelocityStateHandler() 
    {
        float xVelocityAbs = Mathf.Abs(rb.velocity.x);

        switch (playerController.GetCurrentPlayerState())
        {
            case PlayerController.PlayerState.Idle:
                if (xVelocityAbs > minSpeed)
                {
                    playerController.SetCurrentPlayerState(PlayerController.PlayerState.Running);
                }
                break;

            case PlayerController.PlayerState.Running:
                if (xVelocityAbs < minSpeed)
                {
                    playerController.SetCurrentPlayerState(PlayerController.PlayerState.Idle);
                    rb.velocity *= Vector2.up;
                }
                break;

            case PlayerController.PlayerState.Blocked:
                if (xVelocityAbs < minSpeed)
                {
                    rb.velocity *= Vector2.up;
                }
                break;

            default:
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        //Gizmos.DrawWireSphere(groundCheck.transform.position, groundCheckRadius);
        Gizmos.DrawWireCube(groundCheck.transform.position, new Vector3(groundCheckWidth, groundCheckHeight, 0));
    }
}
