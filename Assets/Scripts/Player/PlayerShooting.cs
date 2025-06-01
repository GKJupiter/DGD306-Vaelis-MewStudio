using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject projectilePrefab; // Assign in the Inspector
    public Transform firePoint; // Assign a FirePoint empty GameObject
    public float projectileSpeed = 10f;
    public float fireRate = 0.2f; // Delay between shots
    private float nextFireTime = 0f;
    private bool isShooting;

    private PlayerMovement playerMovement;
    Animator animator;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>(); // Get reference to player movement
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.C) && Time.time >= nextFireTime) // Press C to shoot
        {
            Shoot();
            nextFireTime = Time.time + fireRate;

        }
        else
        {
            isShooting = false;
        }
    }

    void Shoot()
    {
        isShooting=true;
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        // Get player's facing direction from PlayerMovement
        float direction = playerMovement != null ? playerMovement.facingDirection : 1f;

        if (playerMovement.facingDirection > 0)
        {
            rb.velocity = firePoint.right * projectileSpeed;
        }
        if (playerMovement.facingDirection < 0)
        {
            rb.velocity = -firePoint.right * projectileSpeed;
        }
    }
}
