using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float facingDirection = 1f; // 1 for right, -1 for left
    private bool isGrounded;
    private bool isStill;
    public Transform firePoint;
    public int playerId = 1; // 1 for P1, 2 for P2

    Animator animator;
    public GameObject groundCheck;
    private int groundCounter = 0; // Counter to handle multiple ground contacts

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        // Ensure initial rotation is correct based on facingDirection
        UpdateRotation();
    }

    void FixedUpdate()
    {
        // Update animator parameters based on current velocity
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    void Update()
    {
        HandleStillInput(); // Handle the "stand still" input
        Walk();             // Handle horizontal movement
        Jump();             // Handle jumping
        FirePointDirection(); // Handle fire point rotation for aiming
    }

    private void HandleStillInput()
    {
        // "Still_P{playerId}" corresponds to Gamepad Button East (e.g., 'B' on Xbox, 'Circle' on PlayStation)
        // This button toggles the isStill state
        if (Input.GetButtonDown($"Still_P{playerId}"))
        {
            isStill = true;
        }
        if (Input.GetButtonUp($"Still_P{playerId}"))
        {
            isStill = false;
        }
    }

    private void Walk()
    {
        // If the player is in "still" mode, stop horizontal movement
        if (isStill)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            return;
        }

        // Get horizontal input from the assigned gamepad D-pad (Horizontal_P1 or Horizontal_P2)
        float moveInput = Input.GetAxisRaw($"Horizontal_P{playerId}");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // Update facing direction and rotation if there's horizontal movement
        if (moveInput != 0)
        {
            facingDirection = Mathf.Sign(moveInput);
            UpdateRotation();
        }
    }

    void UpdateRotation()
    {
        // Flip the player sprite based on facing direction
        transform.rotation = Quaternion.Euler(0, facingDirection > 0 ? 0 : 180, 0);
    }

    private void Jump()
    {
        // "Jump_P{playerId}" corresponds to Gamepad Button South (e.g., 'A' on Xbox, 'X' on PlayStation)
        if (Input.GetButtonDown($"Jump_P{playerId}") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isGrounded = false;
            animator.SetBool("isJumping", true); // Trigger jump animation
        }

        // Short hop mechanic: if jump button is released while moving upwards, reduce jump height
        if (Input.GetButtonUp($"Jump_P{playerId}") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private void FirePointDirection()
    {
        // "VerticalAim_P{playerId}" corresponds to Gamepad D-pad Y-axis (Up/Down)
        float verticalAimInput = Input.GetAxisRaw($"VerticalAim_P{playerId}");

        if (isStill)
        {
            if (verticalAimInput > 0.1f) // D-pad Up (aim upwards or diagonally)
            {
                // Aim diagonally if moving, otherwise straight up
                if (facingDirection > 0) // Facing right
                {
                    firePoint.localRotation = Quaternion.Euler(0, 0, 45); // Up-right
                }
                else // Facing left
                {
                    firePoint.localRotation = Quaternion.Euler(0, 0, 135); // Up-left
                }
            }
            else
            {
                // If not pressing up, aim straight horizontally when still
                firePoint.localRotation = Quaternion.Euler(0, 0, facingDirection > 0 ? 0 : 180);
            }
        }
        else // Not still, revert to horizontal aiming
        {
            firePoint.localRotation = Quaternion.Euler(0, 0, facingDirection > 0 ? 0 : 180);
        }
    }

    // Handles collision with ground to determine if the player is grounded
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // IMPORTANT: Added null check for collision.gameObject
        // This prevents NullReferenceException if the GameObject somehow doesn't exist.
        if (collision != null && collision.gameObject != null && collision.gameObject.CompareTag("Ground"))
        {
            groundCounter++;
            isGrounded = true;
            animator.SetBool("isJumping", false); // Reset jump animation state
        }
    }

    // Handles exiting collision with ground
    private void OnTriggerExit2D(Collider2D collision)
    {
        // IMPORTANT: Added null check for collision.gameObject
        if (collision != null && collision.gameObject != null && collision.gameObject.CompareTag("Ground"))
        {
            groundCounter--;
            if (groundCounter <= 0) // Only set to not grounded if no more ground contacts
            {
                isGrounded = false;
            }
        }
    }
}
