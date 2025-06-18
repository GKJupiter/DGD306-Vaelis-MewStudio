using UnityEngine;

public class MagicStar : MonoBehaviour
{
    public float fallSpeed = 5f; // How fast the star falls
    public float destroyDelay = 0.1f; // Small delay before destroying after collision

    void Update()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Destroy if it hits ground, players, or other specified layers/tags
        if (other.CompareTag("Player") || other.CompareTag("Ground") || other.CompareTag("Obstacle"))
        {
            Debug.Log("Magic Star hit: " + other.name);
            // Optionally play a small effect here
            Destroy(gameObject, destroyDelay);
        }
    }
}