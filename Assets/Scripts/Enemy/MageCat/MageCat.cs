using UnityEngine;
using System.Collections;
using System.Collections.Generic; // For lists if needed

public class MageCat : MonoBehaviour
{
    // --- Inspector Variables ---
    [Header("Player Detection")]
    public float fieldOfViewRadius = 10f;
    public LayerMask playerLayer; // Assign your Player layer in Inspector

    [Header("Movement")]
    public float verticalMovementRange = 1f; // How much it moves up/down
    public float verticalMovementSpeed = 0.5f;
    public float teleportInterval = 3f;
    public float teleportOffsetX = 2f; // How far from screen edge to teleport

    [Header("Attack Settings")]
    public float attackInterval = 1f; // Time between attacks
    public GameObject rainStarPrefab;
    public GameObject ivyTrapPrefab;
    public GameObject laserBeamPrefab;
    public Transform firePoint; // Assign an empty GameObject as fire point for laser/stars
    public int laserFrames = 25; // 25 frames for laser duration

    // --- Private Variables ---
    private Transform[] players; // Array to hold references to all player transforms
    private bool playerInFOV = false;
    private float nextAttackTime = 0f;
    private float nextTeleportTime = 0f;
    private Vector3 initialPosition;
    private float currentVerticalOffset = 0f;
    private bool movingUp = true;
    private Camera playerCamera1; // Reference to Player 1's camera
    private Camera playerCamera2; // Reference to Player 2's camera (or array if more players)


    void Start()
    {
        initialPosition = transform.position;
        nextTeleportTime = Time.time + teleportInterval;

        // Find all players (e.g., by tag)
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        players = new Transform[playerObjects.Length];
        for (int i = 0; i < playerObjects.Length; i++)
        {
            players[i] = playerObjects[i].transform;
        }

        // Get references to player cameras (you'll need a way to identify them)
        // This is a simplified example. You might have a PlayerManager that holds these references.
        playerCamera1 = GameObject.Find("Player1Camera").GetComponent<Camera>();
        playerCamera2 = GameObject.Find("Player2Camera").GetComponent<Camera>();

        // Initialize object pools if you're using them
        // ObjectPooler.Instance.CreatePool(rainStarPrefab, 10);
        // ObjectPooler.Instance.CreatePool(ivyTrapPrefab, 5);
        // ObjectPooler.Instance.CreatePool(laserBeamPrefab, 2);
    }

    void Update()
    {
        CheckForPlayerInFOV();

        if (playerInFOV)
        {
            HandleMovement();
            HandleAttacks();
        }
        else
        {
            // Optional: Catmage could have idle movement or patrol when no player in FOV
            // For now, it just stops active behavior.
            HandleIdleMovement();
        }
    }

    void CheckForPlayerInFOV()
    {
        // Check if ANY player is within the Catmage's field of view
        playerInFOV = false;
        foreach (Transform player in players)
        {
            if (player != null && Vector2.Distance(transform.position, player.position) <= fieldOfViewRadius)
            {
                playerInFOV = true;
                break; // One player in FOV is enough
            }
        }
    }

    void HandleIdleMovement()
    {
        // Simple bobbing up and down
        currentVerticalOffset = Mathf.PingPong(Time.time * verticalMovementSpeed, verticalMovementRange);
        transform.position = new Vector3(initialPosition.x, initialPosition.y + currentVerticalOffset, initialPosition.z);
    }

    void HandleMovement()
    {
        // Flying up and down a little bit
        currentVerticalOffset = Mathf.PingPong(Time.time * verticalMovementSpeed, verticalMovementRange);
        transform.position = new Vector3(transform.position.x, initialPosition.y + currentVerticalOffset, transform.position.z);

        // Teleportation
        if (Time.time >= nextTeleportTime)
        {
            TeleportToRandomScreenSide();
            nextTeleportTime = Time.time + teleportInterval;
        }
    }

    void TeleportToRandomScreenSide()
    {
        // Choose a random player's camera to base the teleportation on
        Camera targetCamera = Random.value > 0.5f ? playerCamera1 : playerCamera2;

        if (targetCamera == null) return; // Safety check

        // Determine left or right side
        bool teleportLeft = Random.value > 0.5f;

        Vector3 targetWorldPosition;
        if (teleportLeft)
        {
            // Teleport to the left side of the chosen player's camera view
            Vector3 viewportLeft = new Vector3(0f, 0.5f, targetCamera.nearClipPlane);
            targetWorldPosition = targetCamera.ViewportToWorldPoint(viewportLeft);
            targetWorldPosition.x += teleportOffsetX; // Offset from edge
        }
        else
        {
            // Teleport to the right side of the chosen player's camera view
            Vector3 viewportRight = new Vector3(1f, 0.5f, targetCamera.nearClipPlane);
            targetWorldPosition = targetCamera.ViewportToWorldPoint(viewportRight);
            targetWorldPosition.x -= teleportOffsetX; // Offset from edge
        }

        // Maintain current Y-position for the vertical bobbing effect
        transform.position = new Vector3(targetWorldPosition.x, transform.position.y, transform.position.z);
    }


    void HandleAttacks()
    {
        if (Time.time >= nextAttackTime)
        {
            // Select a random player to target
            Transform targetPlayer = GetRandomPlayerInFOV();
            if (targetPlayer == null) return; // Should not happen if playerInFOV is true

            int attackType = Random.Range(0, 3); // 0: Rain, 1: Trap, 2: Laser

            switch (attackType)
            {
                case 0:
                    StartCoroutine(RainMagicAttack(targetPlayer));
                    break;
                case 1:
                    StartCoroutine(TrapGroundAttack(targetPlayer));
                    break;
                case 2:
                    StartCoroutine(LaserMagicAttack(targetPlayer));
                    break;
            }

            nextAttackTime = Time.time + attackInterval;
        }
    }

    Transform GetRandomPlayerInFOV()
    {
        List<Transform> playersInFOV = new List<Transform>();
        foreach (Transform player in players)
        {
            if (player != null && Vector2.Distance(transform.position, player.position) <= fieldOfViewRadius)
            {
                playersInFOV.Add(player);
            }
        }

        if (playersInFOV.Count > 0)
        {
            return playersInFOV[Random.Range(0, playersInFOV.Count)];
        }
        return null;
    }


    IEnumerator RainMagicAttack(Transform targetPlayer)
    {
        // Wait for a brief moment before spawning the stars
        yield return new WaitForSeconds(0.2f); // Small delay for visual effect

        int numberOfStars = Random.Range(3, 7); // Example: 3 to 6 stars
        float spawnRadius = 2f; // Stars spawn within this radius above player

        for (int i = 0; i < numberOfStars; i++)
        {
            Vector3 spawnPosition = targetPlayer.position + Vector3.up * 5f; // Start high above player
            spawnPosition += (Vector3)Random.insideUnitCircle * spawnRadius; // Random offset

            // Instantiate or get from pool
            GameObject star = Instantiate(rainStarPrefab, spawnPosition, Quaternion.identity);
            // If using pooling: GameObject star = ObjectPooler.Instance.GetPooledObject(rainStarPrefab); star.transform.position = spawnPosition; star.SetActive(true);
        }
    }

    IEnumerator TrapGroundAttack(Transform targetPlayer)
    {
        // Spawn ivy on the ground directly under the player
        Vector3 spawnPosition = targetPlayer.position;
        // Adjust Y to be at ground level, you might need to raycast down from player to find ground
        // For simplicity, assuming player's pivot is at their feet or slightly above.
        // If your ground is consistent, you can adjust Y directly.
        spawnPosition.y -= 0.5f; // Example: adjust if player pivot is center

        GameObject ivy = Instantiate(ivyTrapPrefab, spawnPosition, Quaternion.identity);
        // If using pooling: GameObject ivy = ObjectPooler.Instance.GetPooledObject(ivyTrapPrefab); ivy.transform.position = spawnPosition; ivy.SetActive(true);

        // The IvyTrap.cs script will handle its own destruction after 2 seconds.
        yield return null; // No need to wait here in this coroutine, IvyTrap handles its own lifecycle.
    }

    IEnumerator LaserMagicAttack(Transform targetPlayer)
    {
        // Aim at the target player
        Vector2 direction = (targetPlayer.position - firePoint.position).normalized;

        GameObject laser = Instantiate(laserBeamPrefab, firePoint.position, Quaternion.identity);
        // If using pooling: GameObject laser = ObjectPooler.Instance.GetPooledObject(laserBeamPrefab); laser.transform.position = firePoint.position; laser.SetActive(true);

        // Make the laser point towards the target
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        laser.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // You'll need to stretch/scale the laser visually to reach the target,
        // or have the LaserBeam.cs script handle its visual length.
        // For now, let's assume the prefab has a fixed length.

        // Pass the duration to the laser script or handle it here
        LaserBeam laserScript = laser.GetComponent<LaserBeam>();
        if (laserScript != null)
        {
            laserScript.ActivateLaser(laserFrames); // Pass frames
        }
        else
        {
            // Fallback for simple destruction if no dedicated script
            float duration = laserFrames / 60f; // Assuming 60 FPS
            Destroy(laser, duration);
        }

        yield return null; // Or yield return new WaitForSeconds(laserDuration); if handling full lifecycle here
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, fieldOfViewRadius);
    }
}