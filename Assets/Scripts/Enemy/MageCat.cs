using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageCat : MonoBehaviour
{
    [SerializeField] Vector2 lineOfSight;
    [SerializeField] LayerMask playerLayer;
    private bool canSeePlayer;

    private Transform player;
    [SerializeField] float attackCooldown = 2f;
    private float lastAttackTime;
    [SerializeField] GameObject groundTrapPrefab;
    [SerializeField] float trapDelay = 0.5f;

    [SerializeField] GameObject laserPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] float laserSpeed = 10f;

    [SerializeField] GameObject rainProjectilePrefab;
    [SerializeField] int numberOfDrops = 5;
    [SerializeField] float spreadRange = 5f;
    [SerializeField] float dropHeight = 6f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("MageCat: Player not found! Make sure your player is tagged as 'Player'");
        }
    }

    void Update()
    {
        canSeePlayer = Physics2D.OverlapBox(transform.position, lineOfSight, 0, playerLayer);

        if (canSeePlayer)
        {
            // Optional: Face the player
            AttackLogic();
        }
    }

    void AttackLogic()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        int randomAttack = Random.Range(0, 3); // 0 - Ground, 1 - Laser, 2 - Rain

        switch (randomAttack)
        {
            case 0: StartCoroutine(GroundTrapAttack()); break;
            case 1: StartCoroutine(LaserAttack()); break;
            case 2: StartCoroutine(MagicRainAttack()); break;
        }

        lastAttackTime = Time.time;
    }



    IEnumerator GroundTrapAttack()
    {
        if (player == null) yield break;

        Vector3 trapPos = new Vector3(player.position.x, player.position.y - 0.5f, 0);
        yield return new WaitForSeconds(trapDelay);

        Instantiate(groundTrapPrefab, trapPos, Quaternion.identity);
    }


    IEnumerator LaserAttack()
    {
        yield return new WaitForSeconds(0.3f); // Optional delay before firing

        Vector2 dir = (player.position - firePoint.position).normalized;
        GameObject laser = Instantiate(laserPrefab, firePoint.position, Quaternion.identity);
        laser.GetComponent<Rigidbody2D>().velocity = dir * laserSpeed;
    }



    IEnumerator MagicRainAttack()
    {
        if (player == null)
        {
            Debug.LogError("MageCat: player reference is null.");
            yield break;
        }

        if (rainProjectilePrefab == null)
        {
            Debug.LogError("MageCat: rainProjectilePrefab is not assigned!");
            yield break;
        }

        for (int i = 0; i < numberOfDrops; i++)
        {
            float randomOffset = Random.Range(-spreadRange, spreadRange);
            Vector3 spawnPos = new Vector3(player.position.x + randomOffset, player.position.y + dropHeight, 0);
            Instantiate(rainProjectilePrefab, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(0.2f);
        }
    }


        void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, lineOfSight);
    }
}
