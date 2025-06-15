using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageCat : MonoBehaviour
{
    [SerializeField] Transform player;

    [SerializeField] GameObject groundAttackPrefab;

    [SerializeField] GameObject magicProjectilePrefab;

    [SerializeField] GameObject magicRainPrefab;

    [SerializeField] Transform attackSpawnPoint;

    [SerializeField] float projectileSpeed;
    [SerializeField] float groundAttackSpeed;

    float attackCooldown = 3f;
    float cooldownTimer;

    void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0f)
        {
            PerformRandomAttack();
            cooldownTimer = attackCooldown;
        }
    }

    void PerformRandomAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        int randomAttack = Random.Range(0, 3);
        if (randomAttack == 0 && distanceToPlayer < 5f)
            GroundAttack();
        else if (randomAttack == 1)
            ShootMagicProjectile();
        else
            MagicRain();
    }

    public void MagicRain()
    {
        StartCoroutine(SpawnMagicRain());
    }

    IEnumerator SpawnMagicRain()
    {
        float rainWidth = 8f; // how wide the rain area is
        int rainCount = 10;
        float spawnHeight = 6f;

        for (int i = 0; i < rainCount; i++)
        {
            float offsetX = Random.Range(-rainWidth / 2, rainWidth / 2);
            Vector3 spawnPos = new Vector3(player.position.x + offsetX, player.position.y + spawnHeight, 0f);

            Instantiate(magicRainPrefab, spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(0.2f); // small delay between drops
        }
    }

    public void ShootMagicProjectile()
    {
        Vector2 direction = (player.position - attackSpawnPoint.position).normalized;
        GameObject projectile = Instantiate(magicProjectilePrefab, attackSpawnPoint.position, Quaternion.identity);

        projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
    }

    public void GroundAttack()
    {
        Vector3 spawnPos = transform.position;
        GameObject groundAttack = Instantiate(groundAttackPrefab, spawnPos, Quaternion.identity);
        
        Vector2 direction = (player.position - transform.position).normalized;
        direction.y = 0f; // make sure it's strictly horizontal

        groundAttack.GetComponent<Rigidbody2D>().velocity = direction * groundAttackSpeed;
    }



}
