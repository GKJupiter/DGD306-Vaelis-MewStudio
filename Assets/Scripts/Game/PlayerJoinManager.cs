using UnityEngine;
using System.Collections.Generic;

public class PlayerJoinManager : MonoBehaviour
{
    public GameObject player2Prefab; // Assign the Player 2 prefab here in the Inspector
    public Transform player2SpawnPoint; // Assign an empty GameObject as spawn point for P2
    public CameraManager cameraManager; // Reference to the CameraManager script

    private bool player2Joined = false;

    void Update()
    {
        // Check if Player 2 has not joined yet and the join button is pressed
        // "Join_P2" corresponds to Gamepad Button North (e.g., 'Y' on Xbox, 'Triangle' on PlayStation)
        if (!player2Joined && Input.GetButtonDown("Join_P2"))
        {
            JoinPlayer2();
        }
    }

    void JoinPlayer2()
    {
        if (player2Prefab == null)
        {
            Debug.LogError("Player 2 Prefab is not assigned in PlayerJoinManager!");
            return;
        }
        if (player2SpawnPoint == null)
        {
            Debug.LogError("Player 2 Spawn Point is not assigned in PlayerJoinManager!");
            return;
        }

        // Instantiate Player 2 at the specified spawn point
        GameObject player2Instance = Instantiate(player2Prefab, player2SpawnPoint.position, Quaternion.identity);

        // Get the PlayerMovement component and set playerId to 2
        PlayerMovement player2Movement = player2Instance.GetComponent<PlayerMovement>();
        if (player2Movement != null)
        {
            player2Movement.playerId = 2;
            Debug.Log("Player 2 joined! Assigned Player ID: " + player2Movement.playerId);
        }
        else
        {
            Debug.LogError("PlayerMovement component not found on Player 2 prefab.");
        }

        // Get the PlayerShooting component and set playerId to 2
        PlayerShooting player2Shooting = player2Instance.GetComponent<PlayerShooting>();
        if (player2Shooting != null)
        {
            player2Shooting.playerId = 2;
        }

        player2Joined = true;

        // Inform the CameraManager that Player 2 has joined and provide their transform
        if (cameraManager != null)
        {
            cameraManager.SetPlayer2Target(player2Instance.transform);
        }
        else
        {
            Debug.LogError("CameraManager is not assigned in PlayerJoinManager!");
        }
    }
}
