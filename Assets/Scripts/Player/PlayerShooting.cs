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

    public int playerId = 1;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButton($"Shoot_P{playerId}") && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        float direction = playerMovement.facingDirection;
        rb.velocity = firePoint.right * projectileSpeed * direction;
    }
}
