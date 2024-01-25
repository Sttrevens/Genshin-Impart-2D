using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float detectionRange = 10f;
    public GameObject projectilePrefab;
    public Transform player1Transform;
    public Transform player2Transform;
    public Vector3 originalPosition;
    public float attackInterval = 3f;
    public float moveSpeed = 2f;

    public float attackRange = 5f;
    private float attackTimer = 0f;

    public bool isIce = false;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        float distanceToPlayer1 = Vector3.Distance(transform.position, player1Transform.position);
        float distanceToPlayer2 = Vector3.Distance(transform.position, player2Transform.position);

        if (distanceToPlayer1 <= detectionRange || distanceToPlayer2 <= detectionRange)
        {
            Transform targetPlayer = distanceToPlayer1 <= distanceToPlayer2 ? player1Transform : player2Transform;

            if (Vector3.Distance(transform.position, targetPlayer.position) <= attackRange)
            {
                // Enemy is within attack range
                attackTimer += Time.deltaTime;
                if (attackTimer >= attackInterval)
                {
                    LaunchProjectile(targetPlayer);
                    attackTimer = 0f;
                }
            }
            else
            {
                // Chase the closest player
                ChasePlayer(Vector3.Distance(transform.position, targetPlayer.position), targetPlayer);
            }
        }
        else
        {
            ReturnToOriginalPosition();
        }
    }

    void ChasePlayer(float distanceToPlayer, Transform playerTransform)
    {
        if (distanceToPlayer > 1f) // Check to avoid too close distance
        {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    void LaunchProjectile(Transform playerTransform)
    {
        Instantiate(projectilePrefab, transform.position, Quaternion.LookRotation(playerTransform.position - transform.position));
        projectilePrefab.GetComponent<Projectile>().IdentifyPlayer(playerTransform.position);
    }

    void ReturnToOriginalPosition()
    {
        if (Vector3.Distance(transform.position, originalPosition) > 1f)
        {
            Vector3 direction = (originalPosition - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }
}
