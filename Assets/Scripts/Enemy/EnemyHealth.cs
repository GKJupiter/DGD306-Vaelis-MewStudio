using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Add this line

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth;
    public float knockbackForce = 5f; // Adjust in Inspector
    private Rigidbody2D rb;

    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage, Transform attacker)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage. HP: " + currentHealth);

        if (rb != null)
        {
            // Determine the knockback direction based on attacker position
            float direction = transform.position.x > attacker.position.x ? 1f : -1f;
            rb.AddForce(new Vector2(direction * knockbackForce, 2f), ForceMode2D.Impulse);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " died!");

        // Check if the dying enemy is the CatMage
        if (gameObject.name == "CatMage")
        {
            Debug.Log("CatMage died! Loading next scene...");
            // Replace "YourNextSceneName" with the actual name of the scene you want to load
            SceneManager.LoadScene("EndingCutscene"); // IMPORTANT: Make sure this scene is added to Build Settings!
        }

        Destroy(gameObject);
    }
}