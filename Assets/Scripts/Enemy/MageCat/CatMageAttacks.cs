using UnityEngine;
using System.Collections;

public class CatMageAttacks : MonoBehaviour
{
    [Header("Field of View")]
    public float detectionRange = 7f; // Radius for player detection
    public LayerMask playerLayer; // Assign the 'Player' layer in the Inspector

    [Header("Attack Settings")]
    public float attackFireRate = 2f; // Time between attacks
    public GameObject ivyTrapPrefab; // Assign your IvyTrap prefab
    public GameObject magicStarPrefab; // Assign your MagicStar prefab
    public GameObject laserBeamPrefab; // Assign your Laser Beam prefab
    public Transform laserFirePoint; // Point where the laser shoots from

    private float attackCooldownTimer;
    private Transform targetPlayer; // The player currently targeted

    // Event/Action to notify the main CatMageEnemy script about player detection
    public delegate void PlayerDetectionEvent(bool playerDetected);
    public event PlayerDetectionEvent OnPlayerDetectionChange;

    public Transform GetTargetPlayer()
    {
        return targetPlayer;
    }

    void Start()
    {
        attackCooldownTimer = attackFireRate; // Initialize cooldown
    }

    void Update()
    {
        DetectPlayer();

        if (targetPlayer != null)
        {
            HandleAttacks();
        }
    }

    void DetectPlayer()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, detectionRange, playerLayer);

        Transform newTarget = null;
        if (hitPlayers.Length > 0)
        {
            float closestDistance = Mathf.Infinity;
            foreach (Collider2D playerCollider in hitPlayers)
            {
                float distance = Vector2.Distance(transform.position, playerCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    newTarget = playerCollider.transform;
                }
            }
        }

        // Check if detection status has changed
        if (newTarget != targetPlayer)
        {
            targetPlayer = newTarget;
            OnPlayerDetectionChange?.Invoke(targetPlayer != null); // Notify listeners
        }
    }

    void HandleAttacks()
    {
        attackCooldownTimer -= Time.deltaTime;

        if (attackCooldownTimer <= 0)
        {
            ChooseAndPerformAttack();
            attackCooldownTimer = attackFireRate; // Reset cooldown
        }
    }

    void ChooseAndPerformAttack()
    {
        if (targetPlayer == null) return; // Should not happen if HandleAttacks is called correctly

        int attackChoice = Random.Range(0, 3); // 0, 1, or 2

        switch (attackChoice)
        {
            case 0:
                PerformIvyTrapAttack();
                break;
            case 1:
                PerformMagicRainAttack();
                break;
            case 2:
                PerformLaserAttack();
                break;
        }
    }

    void PerformIvyTrapAttack()
    {
        if (ivyTrapPrefab == null)
        {
            Debug.LogError("Ivy Trap Prefab not assigned in CatMageAttacks!");
            return;
        }

        RaycastHit2D hit = Physics2D.Raycast(targetPlayer.position, Vector2.down, 10f, LayerMask.GetMask("Ground"));

        if (hit.collider != null)
        {
            Vector2 spawnPosition = new Vector2(targetPlayer.position.x, hit.point.y);
            Instantiate(ivyTrapPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("CatMage used Ivy Trap!");
        }
        else
        {
            Debug.LogWarning("Could not find ground under player for Ivy Trap!");
        }
    }

    void PerformMagicRainAttack()
    {
        if (magicStarPrefab == null)
        {
            Debug.LogError("Magic Star Prefab not assigned in CatMageAttacks!");
            return;
        }

        int numberOfStars = Random.Range(3, 7);
        for (int i = 0; i < numberOfStars; i++)
        {
            Vector2 spawnOffset = new Vector2(Random.Range(-2f, 2f), Random.Range(5f, 8f));
            Vector2 spawnPosition = (Vector2)targetPlayer.position + spawnOffset;
            Instantiate(magicStarPrefab, spawnPosition, Quaternion.identity);
        }
        Debug.Log("CatMage used Magic Rain!");
    }

    void PerformLaserAttack()
    {
        if (laserBeamPrefab == null)
        {
            Debug.LogError("Laser Beam Prefab not assigned in CatMageAttacks!");
            return;
        }
        if (laserFirePoint == null)
        {
            Debug.LogError("Laser Fire Point not assigned in CatMageAttacks!");
            return;
        }

        GameObject laser = Instantiate(laserBeamPrefab, laserFirePoint.position, Quaternion.identity);

        Vector2 directionToPlayer = (targetPlayer.position - laserFirePoint.position).normalized;
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        laser.transform.rotation = Quaternion.Euler(0, 0, angle);

        Debug.Log("CatMage used Laser Attack!");
    }

    // Optional: Draw detection range in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}