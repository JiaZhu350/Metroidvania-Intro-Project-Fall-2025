using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("Attack Settings")]
    public float attackCooldown = 2f;
    public float projectileForce = 7f;
    public GameObject projectilePrefab;
    public Transform firePoint;

    [Header("Detection")]
    public float detectRadius = 8f;
    public LayerMask playerLayer;

    [SerializeField] private EnemyPatrol patrol;
    private float cooldownTimer = Mathf.Infinity;
    private Animator anim;
    private Transform player;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("Player")?.transform;
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        bool playerNear = Vector3.Distance(transform.position, player.position) <= detectRadius;

        if (playerNear)
        {
            patrol.Interupted(); // stop patrol
            anim.SetBool("moving", false);

            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0f;
                anim.SetTrigger("throw");
            }
        }
        else
        {
            patrol.ResetPatrol(); // go back to patrol
        }
    }

    // Called by animation event
    private void ThrowProjectile()
    {
        GameObject obj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        Vector2 direction = (player.position - firePoint.position);

        // Add arc by increasing Y component
        direction.y += 3f;

        rb.AddForce(direction.normalized * projectileForce, ForceMode2D.Impulse);
    }
}
