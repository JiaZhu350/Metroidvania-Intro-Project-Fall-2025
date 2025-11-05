using UnityEngine;

public class meleeEnemy : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerlayer;
    [SerializeField] private EnemyPatrol enemyPatrol;
    private float CooldownTimer = Mathf.Infinity;

    //Refrences
    private Health playerhealth;
    private Animator anim;

    //Chase Parameters
    private GameObject player; // Reference to the player's transform
    public float chaseSpeed = 5f; // Speed at which the enemy chases the player
    public LayerMask groundLayer; // Layer mask to identify ground
    public float detectRadius = 5f;
    private bool playerInChaseRange = false;
    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isAttacking = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CooldownTimer += Time.deltaTime;

        if (CooldownTimer >= attackCooldown)
        {
            if (PlayerInRange())
            {
                isAttacking = true;
                Debug.Log("Melee enemy attacks for " + damage + " damage!");
                CooldownTimer = 0f; // Reset the cooldown timer after attacking
                anim.SetTrigger("meleeAttack");
            }
        }
        // Chase logic here
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= detectRadius && !playerInChaseRange && !isAttacking)
        {
            playerInChaseRange = true;
            enemyPatrol.Interupted();
        }
        else if (distanceToPlayer > detectRadius && playerInChaseRange && !isAttacking)
        {
            playerInChaseRange = false;
            enemyPatrol.ResetPatrol();

        }

        if (playerInChaseRange && !isAttacking)
        {
            ChaseLogic();
        }

        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
        }

    }
    private bool PlayerInRange()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0f, Vector2.left, 0.1f, playerlayer);
        if (hit.collider != null)
        {
            playerhealth = hit.transform.GetComponent<Health>();
        }
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        if (PlayerInRange())
        {
            playerhealth.TakeDamage(damage);
        }
    }

    private void ChaseLogic()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);

        float direction = Mathf.Sign(player.transform.position.x - transform.position.x);
        RaycastHit2D gapAhead = Physics2D.Raycast(transform.position + new Vector3(direction * 0.5f, 0, 0), Vector2.down, 2f, groundLayer);

        if (isGrounded && gapAhead.collider)
        {
            rb.linearVelocity = new Vector2(direction * chaseSpeed, rb.linearVelocity.y);
        }

        else if (!isGrounded || !gapAhead.collider)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

}
