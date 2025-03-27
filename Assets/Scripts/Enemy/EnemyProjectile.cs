using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float lifeTime = 2f; // Destroy after 2 seconds

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) // Prevent destroying on player
        {
            Destroy(gameObject);
        }
    }
}
