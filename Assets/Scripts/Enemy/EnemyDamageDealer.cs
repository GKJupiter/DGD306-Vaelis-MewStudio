using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
    // This method is called when this collider (which is NOT a trigger)
    // touches another collider (which is also NOT a trigger).
    void OnCollisionEnter2D(Collision2D collision) // Note the change to Collision2D parameter
    {
        // Access the GameObject of the colliding object via collision.gameObject
        if (collision.gameObject.CompareTag("Player")) //
        {
            // Get the PlayerHealth component from the colliding GameObject
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>(); //
            if (playerHealth != null) //
            {
                playerHealth.TakeDamage(10); //
                Debug.Log("Slime dealt damage on collision!");
            }
        }
    }

}