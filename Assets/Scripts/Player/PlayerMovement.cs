using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    private float facingDirection = 1f; // 1 = right, -1 = left
    private bool isGrounded;

    [Header("Ground Check")]
    public GameObject groundCheck; // Assign the GroundCheck child object here
    private int groundCounter = 0; // Tracks the number of ground touches

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Walk();
        Jump();
    }

    private void Walk()
    {
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
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
        }
        if (Input.GetKeyUp(KeyCode.Space) && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    // Ground Check Using Collider
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            groundCounter++;
            isGrounded = true;
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
