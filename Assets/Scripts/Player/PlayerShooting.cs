using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;
    public float fireRate = 0.2f;
    private float nextFireTime = 0f;

    private PlayerMovement playerMovement;
    Animator animator; // Reference to the Animator component
    private bool isShooting = false; // New boolean for shooting state

    public int playerId = 1; // 1 for P1, 2 for P2

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    void Update()
    {
        // "Shoot_P{playerId}" corresponds to Gamepad Button West (e.g., 'X' on Xbox, 'Square' on PlayStation)
        if (Input.GetButton($"Shoot_P{playerId}"))
        {
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
            if (!isShooting) // Only set the bool if it's currently false
            {
                isShooting = true;
                if (animator != null)
                {
                    animator.SetBool("isShooting", true); // Set isShooting bool in Animator
                }
            }
        }
        else
        {
            if (isShooting) // Only set the bool if it's currently true
            {
                isShooting = false;
                if (animator != null)
                {
                    animator.SetBool("isShooting", false); // Reset isShooting bool in Animator
                }
            }
        }
    }

    void Shoot()
    {
        // Get the current facing direction from PlayerMovement
        float direction = playerMovement.facingDirection;

        // Calculate the rotation for the projectile based on the player's facing direction
        Quaternion projectileRotation;
        if (direction > 0) // Facing right
        {
            projectileRotation = Quaternion.Euler(0, 0, 0); // No Z rotation for right-facing
        }
        else // Facing left
        {
            projectileRotation = Quaternion.Euler(0, 180, 0); // 180 degrees Y-rotation to flip the sprite
        }

        // Instantiate the projectile at the firePoint's position with the calculated rotation
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, projectileRotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        // Apply velocity to the projectile based on the firePoint's right vector and player's facing direction
        rb.velocity = firePoint.right * projectileSpeed * direction;
    }
}