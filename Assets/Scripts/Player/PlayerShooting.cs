using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 10f;
    public float fireRate = 0.2f;
    private float nextFireTime = 0f;

    private PlayerMovement playerMovement;
    Animator animator;

    public int playerId = 1; // 1 for P1, 2 for P2

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // "Shoot_P{playerId}" corresponds to Gamepad Button West (e.g., 'X' on Xbox, 'Square' on PlayStation)
        if (Input.GetButton($"Shoot_P{playerId}") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        // Instantiate the projectile at the firePoint's position and rotation
        // Quaternion.identity means no rotation is applied initially; projectile's velocity will dictate direction
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        // Get the current facing direction from PlayerMovement to apply to the projectile
        float direction = playerMovement.facingDirection;
        // Apply velocity to the projectile based on the firePoint's right vector and player's facing direction
        // This ensures the projectile shoots in the direction the player is facing/aiming
        rb.velocity = firePoint.right * projectileSpeed * direction;
    }
}
