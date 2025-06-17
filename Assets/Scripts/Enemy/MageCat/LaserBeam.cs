using UnityEngine;
using System.Collections;

public class LaserBeam : MonoBehaviour
{
    public float damage = 15f;
    public LineRenderer lineRenderer; // Optional, for dynamic laser visual
    public float maxLaserLength = 20f; // Max length the laser can extend
    public LayerMask playerLayer; // Assign your Player layer

    private int currentFrames = 0;
    private int totalFrames = 0;

    void Awake()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
    }

    public void ActivateLaser(int frames)
    {
        totalFrames = frames;
        currentFrames = 0;
        StartCoroutine(LaserLifecycle());

        // Initial setup for visual laser (if using SpriteRenderer)
        // Adjust scale or other properties here
        // If using LineRenderer:
        if (lineRenderer != null)
        {
            lineRenderer.enabled = true;
            // Set line positions, e.g., from firepoint to a point far away in its direction
            Vector3 startPoint = transform.position;
            Vector3 endPoint = transform.position + transform.right * maxLaserLength; // Assuming laser's forward is +X
            lineRenderer.SetPositions(new Vector3[] { startPoint, endPoint });
        }
    }

    IEnumerator LaserLifecycle()
    {
        // Calculate total duration in seconds
        float duration = totalFrames / 60f; // Assuming 60 frames per second
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            // Raycast to detect players along the laser path
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.right, maxLaserLength, playerLayer);
            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    // Deal damage once per hit
                    // hit.collider.GetComponent<PlayerHealth>()?.TakeDamage(damage * Time.deltaTime); // Or a fixed damage per hit
                }
            }
            yield return null; // Wait for next frame
        }

        // Deactivate/destroy laser
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
        Destroy(gameObject);
        // If pooling: gameObject.SetActive(false); // Return to pool
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // This is a simpler collision detection, might be less accurate for a beam
        // than raycasting, but useful if the laser is a thin rectangle collider.
        if (other.CompareTag("Player"))
        {
            // other.GetComponent<PlayerHealth>()?.TakeDamage(damage); // Instant damage
        }
    }
}