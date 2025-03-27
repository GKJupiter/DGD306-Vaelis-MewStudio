using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public float lifeTime = 2f; // Destroy after 2 seconds

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) // Prevent destroying on player
        {
            Destroy(gameObject);
        }
    }
}
