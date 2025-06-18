using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management
using System.Collections.Generic; // Required for List

public class GameManager : MonoBehaviour
{
    // A list to hold references to all PlayerHealth scripts in the scene
    public List<PlayerHealth> playersHealth;

    // Keep track of how many players are currently dead
    private int deadPlayersCount = 0;

    // Assign these in the Inspector for your desired respawn points
    public Transform player1RespawnPoint;
    public Transform player2RespawnPoint;

    void Start()
    {
        // Find all PlayerHealth scripts in the scene if not assigned in Inspector
        if (playersHealth == null || playersHealth.Count == 0)
        {
            playersHealth = new List<PlayerHealth>(FindObjectsOfType<PlayerHealth>());
        }

        // Ensure each PlayerHealth script knows about this GameManager
        foreach (PlayerHealth player in playersHealth)
        {
            player.gameManager = this;
        }

        // Initialize dead players count
        deadPlayersCount = 0;
    }

    // This method is called by a PlayerHealth script when a player dies
    public void PlayerDied()
    {
        deadPlayersCount++;
        Debug.Log("Dead players: " + deadPlayersCount);

        // Check if all players are dead
        if (deadPlayersCount >= playersHealth.Count)
        {
            Debug.Log("All players are dead. Reloading scene...");
            ReloadCurrentScene();
        }
    }

    private void ReloadCurrentScene()
    {
        // Get the name of the current active scene and reload it
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    // Call this method after the scene reloads to place players at specific locations
    void OnEnable()
    {
        // Subscribe to the sceneLoaded event to handle respawn after a scene reload
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset the dead players count when the scene reloads
        deadPlayersCount = 0;

        // Re-find players and reset their health, then position them at respawn points
        playersHealth = new List<PlayerHealth>(FindObjectsOfType<PlayerHealth>());
        if (playersHealth.Count > 0)
        {
            playersHealth[0].ResetHealth();
            if (player1RespawnPoint != null)
            {
                playersHealth[0].transform.position = player1RespawnPoint.position;
            }
        }
        if (playersHealth.Count > 1)
        {
            playersHealth[1].ResetHealth();
            if (player2RespawnPoint != null)
            {
                playersHealth[1].transform.position = player2RespawnPoint.position;
            }
        }
        // Add more logic here for additional players if needed
    }
}