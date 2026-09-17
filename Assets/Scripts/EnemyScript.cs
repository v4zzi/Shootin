using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyScript : MonoBehaviour
{
    [Header("Recompensas (Loot)")]
    public GameObject ammoPickupPrefab;
    [Range(0f, 1f)] public float dropChance = 0.8f;

    [Header("Salud del Enemigo")]
    public float health = 50f;

    [Header("Referencias")]
    public Transform playerTransform;

    [Header("Parámetros de Detección y Ataque")]
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackDamage = 10f;
    public float attackCooldown = 1.5f;

    [Header("Puntos de Patrulla")]
    public Transform[] waypoints;
    public float waypointWaitTime = 2f;
    private int currentWaypointIndex = 0;
    private float waitTimer = 0f;

    private NavMeshAgent agent;
    private float nextAttackTime = 0f;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
        }

        if (waypoints != null && waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    private void Update()
    {
        float distanceToPlayer = playerTransform != null
            ? Vector3.Distance(transform.position, playerTransform.position)
            : float.MaxValue;

        if (distanceToPlayer <= attackRange)
        {
            agent.isStopped = true;

            Vector3 lookDirection = (playerTransform.position - transform.position).normalized;
            lookDirection.y = 0;
            if (lookDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(lookDirection);
            }

            if (Time.time >= nextAttackTime)
            {
                nextAttackTime = Time.time + attackCooldown;
                AttackPlayer();
            }
        }
        else if (distanceToPlayer <= detectionRange)
        {
            agent.isStopped = false;
            agent.SetDestination(playerTransform.position);
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        agent.isStopped = false;

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waypointWaitTime)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
                agent.SetDestination(waypoints[currentWaypointIndex].position);
                waitTimer = 0f;
            }
        }
    }

    private void AttackPlayer()
    {
        Debug.Log("¡El enemigo te ha atacado!");

        if (playerTransform != null)
        {
            PlayerHealth playerHealth = playerTransform.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        if (ammoPickupPrefab != null && Random.value <= dropChance)
        {
            Vector3 spawnPosition = transform.position + Vector3.up * 0.5f;
            Instantiate(ammoPickupPrefab, spawnPosition, Quaternion.identity);
        }

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.EnemyDefeated();
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange); 
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);    
    }
}