using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform pointA, pointB; 
    public float speed = 2f;
    public float detectionRange = 6f;
    public GameObject projectilePrefab;
    public float shootCooldown = 1.2f;
    public float projectileSpeed = 7f;

    private Transform player;
    private Vector3 targetPoint;
    private float shootTimer = 0f;
    private SpriteRenderer sr;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sr = GetComponent<SpriteRenderer>();
        targetPoint = pointB.position;
        Debug.Log("Enemy started patrolling towards PointB.");
    }

    void Update()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f)
        {
            Shoot();
            shootTimer = shootCooldown; // reset cooldown
            Debug.Log("Auto-shoot fired for testing.");
        }
        if (distToPlayer <= detectionRange)
        {
            Debug.Log($"Player detected at distance {distToPlayer:F2}!");
            AttackPlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPoint, speed * Time.deltaTime);

        // Flip sprite based on direction
        sr.flipX = (targetPoint.x < transform.position.x);

        if (Vector2.Distance(transform.position, targetPoint) < 0.1f)
        {
            targetPoint = (targetPoint == pointA.position) ? pointB.position : pointA.position;
            Debug.Log($"Reached patrol point. Next target: {(targetPoint == pointA.position ? "PointA" : "PointB")}");
        }
    }

    void AttackPlayer()
    {
        // Face the player
        sr.flipX = (player.position.x < transform.position.x);

        shootTimer -= Time.deltaTime;

        if (shootTimer <= 0f)
        {
            Shoot();
            shootTimer = shootCooldown;
        }
    }

    void Shoot()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * projectileSpeed;

        Debug.Log($"Shooting projectile towards player at position {player.position} with velocity {rb.linearVelocity}.");
    }
}
