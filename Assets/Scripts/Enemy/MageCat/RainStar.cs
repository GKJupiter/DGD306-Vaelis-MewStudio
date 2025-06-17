using UnityEngine;

public class RainStar : MonoBehaviour
{
    public float damage = 10f; // Damage dealt by the star

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if it hit a player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Deal damage to player (PlayerHealth script or similar)
            // collision.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }

        // Destroy after hitting anything (ground, player, etc.)
        Destroy(gameObject);
        // If pooling: gameObject.SetActive(false); // Return to pool
    }

    // Optional: If stars can pass through other things and only trigger on player/ground
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Ground")) // Assuming a "Ground" tag
        {
            if (other.gameObject.CompareTag("Player"))
            {
                // Deal damage
                // other.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            }
            Destroy(gameObject);
            // If pooling: gameObject.SetActive(false); // Return to pool
        }
    }
}