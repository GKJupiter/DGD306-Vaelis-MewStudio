using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    public bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth; // Set full health at start
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
        Debug.Log("Player Died!");
        // You can add respawn or game over logic here.
    }
}
