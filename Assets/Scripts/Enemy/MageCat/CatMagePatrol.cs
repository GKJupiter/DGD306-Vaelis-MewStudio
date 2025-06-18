using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CatMagePatrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform[] patrolPoints; // Array of waypoints for patrolling
    public float patrolSpeed = 2f;

    private int currentPatrolIndex = 0;
    private bool patrollingForward = true;
    private bool isPatrollingEnabled = true; // Use this to enable/disable patrol from outside

    public bool IsPatrollingEnabled
    {
        get { return isPatrollingEnabled; }
        set { isPatrollingEnabled = value; }
    }

    void Start()
    {
        if (patrolPoints.Length == 0)
        {
            Debug.LogWarning("CatMagePatrol: No patrol points assigned! Catmage will not patrol.");
            isPatrollingEnabled = false;
        }
    }

    void Update()
    {
        if (isPatrollingEnabled)
        {
            Patrol();
        }
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        // Move towards the current patrol point
        transform.position = Vector2.MoveTowards(transform.position, patrolPoints[currentPatrolIndex].position, patrolSpeed * Time.deltaTime);

        // Check if reached the current patrol point
        if (Vector2.Distance(transform.position, patrolPoints[currentPatrolIndex].position) < 0.1f)
        {
            // Move to the next point
            if (patrollingForward)
            {
                currentPatrolIndex++;
                if (currentPatrolIndex >= patrolPoints.Length)
                {
                    currentPatrolIndex = patrolPoints.Length - 2; // Go back one to start reversing
                    patrollingForward = false;
                }
            }
            else // Patrolling backwards
            {
                currentPatrolIndex--;
                if (currentPatrolIndex < 0)
                {
                    currentPatrolIndex = 1; // Go forward one to start advancing
                    patrollingForward = true;
                }
            }
            FlipSprite(); // Flip sprite when changing direction
        }
    }

    void FlipSprite()
    {
        // Simple sprite flipping based on movement direction
        // Ensure this logic correctly reflects your sprite orientation
        if (transform.position.x < patrolPoints[currentPatrolIndex].position.x)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z); // Facing right (assuming 1 is right)
        }
        else
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z); // Facing left (assuming -1 is left)
        }
    }

    // Optional: Draw patrol path in editor
    void OnDrawGizmosSelected()
    {
        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                Gizmos.DrawSphere(patrolPoints[i].position, 0.2f); // Draw small spheres at waypoints
                if (i < patrolPoints.Length - 1)
                {
                    Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position); // Draw lines between them
                }
            }
        }
    }
}