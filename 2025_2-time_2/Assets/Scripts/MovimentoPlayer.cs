using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoPlayer : MonoBehaviour
{
    public float MoveSpeed = 5.0f;

    private Rigidbody2D rb;

    private float MoveInput;

    public float JumpForce = 5.0f;

   
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Move()
    {
        MoveInput = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(MoveInput * MoveSpeed, rb.velocity.y);
    }
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            rb.velocity = new Vector2(0, JumpForce);

    }
    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        Jump();
    }



}
