using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Image HealthBar;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth;
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
        HealthBar.fillAmount = currentHealth / 100;
        Debug.Log("Player took " + damage + " damage. Current HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
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