using UnityEngine;

public class BatShooter : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1.5f;
    public float bulletSpeed = 4f;
    public float shootAngleRange = 20f;
    private float nextFireTime = 0f;

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        Vector3 forwardDirection = transform.localScale.x < 0 ? transform.right : -transform.right;

        float randomAngle = Random.Range(-shootAngleRange, shootAngleRange);

        Quaternion rotation = Quaternion.Euler(0, 0, randomAngle);
        Vector3 direction = rotation * forwardDirection;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }
    }
}
