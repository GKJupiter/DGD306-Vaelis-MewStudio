using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float facingDirection = 1f; // 1 = right, -1 = left
    private bool isGrounded;
    private bool isStill;
    public Transform firePoint;

    Animator animator;

    [Header("Ground Check")]
    public GameObject groundCheck; // Assign the GroundCheck child object here
    private int groundCounter = 0; // Tracks the number of ground touches

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        animator.SetFloat("xVelocity", Math.Abs(rb.velocity.x));
        animator.SetFloat("yVelocity", rb.velocity.y);

    }

    void Update()
    {
        Still();
        Walk();
        Jump();
        FirePointDirection();
    }

    private void Walk()
    {
        if (isStill)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput != 0)
        {
            facingDirection = Mathf.Sign(moveInput);
            UpdateRotation();
        }
    }

    void UpdateRotation()
    {
            if (facingDirection > 0)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else if (facingDirection < 0)
                transform.rotation = Quaternion.Euler(0, -180, 0);
    }


    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
            animator.SetBool("isJumping", !isGrounded);
        }
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private void FirePointDirection()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.1f)
            {
                // Diagonal up while walking
                firePoint.localRotation = Quaternion.Euler(0, 0, facingDirection > 0 ? 45 : 225);
            }
            else
            {
                // Looking straight up
                firePoint.localRotation = Quaternion.Euler(0, 0, facingDirection > 0 ? 90 : -90);
            }
        }
        else
        {
            // Horizontal aiming
            firePoint.localRotation = Quaternion.Euler(0, 0, facingDirection > 0 ? 0 : 180);
        }
    }



    private void Still()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            isStill = true;
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            isStill = false;
        }
    }

    // Ground Check Using Collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            groundCounter++;
            isGrounded = true;
            animator.SetBool("isJumping", !isGrounded);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            groundCounter--;
            if (groundCounter <= 0)
                isGrounded = false;
        }
    }
}
