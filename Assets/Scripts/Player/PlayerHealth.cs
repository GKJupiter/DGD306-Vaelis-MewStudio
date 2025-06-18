using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public bool isDead = false;

    // Optional: Reference to the GameManager
    public GameManager gameManager; // Assign this in the Inspector for each player

    void Start()
    {
        currentHealth = maxHealth; // Set full health at start
        // Ensure the GameManager is assigned, or find it dynamically
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log("Player took " + damage + " damage. Current HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth; // Prevent overhealing
        }

        Debug.Log("Player healed " + amount + " HP. Current HP: " + currentHealth);
    }

    private void Die()
    {
        isDead = true;
        Debug.Log(gameObject.name + " Died!");
        // Notify the GameManager that this player has died
        if (gameManager != null)
        {
            gameManager.PlayerDied();
        }
        // Optionally disable player movement/rendering here
        gameObject.SetActive(false); // Example: Hide the player
    }

    // Call this method when the player respawns
    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;
        gameObject.SetActive(true); // Example: Show the player again
    }
}