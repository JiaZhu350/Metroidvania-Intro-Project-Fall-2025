using UnityEngine;

public class FollowShoot : MonoBehaviour
{
    public float speed;
    public float lineOfSight;
    private Transform player;
    public float shootingRange;
    public GameObject bullet;
    public GameObject bulletParent;

    [Header("Shooting")]
    public float fireRate = 0.5f;
    private float fireTimer = 0f;
    public float projectileSpeed = 5f;
    public float spawnAngleOffset = 0f;

    [Header("Positioning")]
    public bool keepAbovePlayer = false;
    public float aboveOffsetY = 3f;

    private Vector2 startPosition; // store the original position

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPosition = transform.position; // remember starting position
    }

    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceFromPlayer < lineOfSight && distanceFromPlayer > shootingRange)
        {
            // Follow the player
            Vector2 targetPos = keepAbovePlayer ? 
                new Vector2(player.position.x, player.position.y + aboveOffsetY) : 
                player.position;

            transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
        else if (distanceFromPlayer <= shootingRange)
        {
            // Shoot at the player
            ShootAtPlayer();
        }
        else
        {
            // Player out of sight, return to start position
            transform.position = Vector2.MoveTowards(transform.position, startPosition, speed * Time.deltaTime);
        }

        // Countdown the fire timer
        if (fireTimer > 0f) fireTimer -= Time.deltaTime;
    }

    void ShootAtPlayer()
    {
        if (bullet == null)
        {
            Debug.LogWarning("FollowShoot: 'bullet' prefab is not assigned.");
            return;
        }

        if (bulletParent == null)
        {
            Debug.LogWarning("FollowShoot: 'bulletParent' is not assigned.");
            return;
        }

        if (fireTimer <= 0f)
        {
            Vector2 dir = (player.position - bulletParent.transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            angle += spawnAngleOffset;

            GameObject spawned = Instantiate(bullet, bulletParent.transform.position, Quaternion.Euler(0f, 0f, angle));

            var projScript = spawned.GetComponent<BulletScript>();
            if (projScript != null)
            {
                projScript.speed = projectileSpeed;
            }
            else
            {
                var rb = spawned.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = dir * projectileSpeed;
                }
            }

            fireTimer = fireRate;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }
}
