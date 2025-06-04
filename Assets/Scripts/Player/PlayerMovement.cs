using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float facingDirection = 1f;
    private bool isGrounded;
    private bool isStill;
    public Transform firePoint;
    public int playerId = 1; // 1 for P1, 2 for P2

    Animator animator;
    public GameObject groundCheck;
    private int groundCounter = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
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

        float moveInput = Input.GetAxisRaw($"Horizontal_P{playerId}");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput != 0)
        {
            facingDirection = Mathf.Sign(moveInput);
            UpdateRotation();
        }
    }

    void UpdateRotation()
    {
        transform.rotation = Quaternion.Euler(0, facingDirection > 0 ? 0 : 180, 0);
    }

    private void Jump()
    {
        if (Input.GetButtonDown($"Jump_P{playerId}") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
            animator.SetBool("isJumping", true);
        }

        if (Input.GetButtonUp($"Jump_P{playerId}") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private void FirePointDirection()
    {
        if (Input.GetKey(KeyCode.UpArrow)) // TODO: Customize per player
        {
            firePoint.localRotation = Quaternion.Euler(0, 0, facingDirection > 0 ? 45 : 225);
        }
        else
        {
            firePoint.localRotation = Quaternion.Euler(0, 0, facingDirection > 0 ? 0 : 180);
        }
    }

    private void Still()
    {
        if (Input.GetKeyDown(KeyCode.X)) isStill = true;
        if (Input.GetKeyUp(KeyCode.X)) isStill = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            groundCounter++;
            isGrounded = true;
            animator.SetBool("isJumping", false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            groundCounter--;
            if (groundCounter <= 0) isGrounded = false;
        }
    }
}
