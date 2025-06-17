using UnityEngine;
using System.Collections;
using System.Collections.Generic; // <--- Add this line!

public class IvyTrap : MonoBehaviour
{
    public float duration = 2f; // How long the trap stays active
    public float damagePerSecond = 5f; // Damage if player stays in it

    private HashSet<GameObject> playersInTrap = new HashSet<GameObject>(); // This line will now work

    void Start()
    {
        StartCoroutine(VanishAfterDuration());
    }

    IEnumerator VanishAfterDuration()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
        // If pooling: gameObject.SetActive(false); // Return to pool
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersInTrap.Add(other.gameObject);
            // Optional: Start a damage over time coroutine for this specific player
            // StartCoroutine(DamagePlayerOverTime(other.gameObject));
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playersInTrap.Remove(other.gameObject);
        }
    }

    void Update()
    {
        // Apply continuous damage to players currently in the trap
        foreach (GameObject player in playersInTrap)
        {
            // player.GetComponent<PlayerHealth>()?.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }

    // You could also use a single coroutine per player to damage them
    // IEnumerator DamagePlayerOverTime(GameObject player) { ... }
}