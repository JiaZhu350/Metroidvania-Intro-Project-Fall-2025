using UnityEngine;

public class Flying_movement : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerlayer;
    private float CooldownTimer = Mathf.Infinity;


    public float speed = 5f;
    private GameObject player;
    public bool isChasing = false;
    public Transform startingPoint;
    public float detectRadius = 5f;
    private Health playerhealth;
    private Vector3 lastPosition;
    private Rigidbody2D rb;
    private bool isAttacking = false;
    private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        lastPosition = transform.position;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
   
        CooldownTimer += Time.deltaTime;

        if (player == null)
        {
            return;
        }

        Attacking();


        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= detectRadius)
        {
            isChasing = true;
        }
        else if (distanceToPlayer > detectRadius)
        {
            isChasing = false;

        }

        if (isChasing)
        {
            Chase();
        }
        else
        {
            ReturnToStart();
        }
        Flip();
    }

    private void Chase()
    {
        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        else
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.linearVelocityX = direction.x * speed;
            rb.linearVelocityY = direction.y * speed;
        }
    }

    private void Flip()
    {
        // Get current horizontal movement direction
        float moveDirection = transform.position.x - lastPosition.x;
        if (Mathf.Abs(moveDirection) > 0.01f) // only flip if actually moving
        {
            // Flip depending on which direction we’re moving
            if (moveDirection > 0)
                transform.localScale = new Vector3(1, 1, 1);  // facing right
            else
                transform.localScale = new Vector3(-1, 1, 1); // facing left
        }

        lastPosition = transform.position;


    }

    private void ReturnToStart()
    {
        //transform.position = Vector2.MoveTowards(transform.position, startingPoint.position, speed * Time.deltaTime);
        float distanceToStart = Vector3.Distance(transform.position, startingPoint.position);
        if (distanceToStart < 0.1f)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = (startingPoint.position - transform.position).normalized;
        rb.linearVelocityX = direction.x * speed;
        rb.linearVelocityY = direction.y * speed;
        //rb.MovePosition(Vector2.MoveTowards(rb.position, startingPoint.position, speed * Time.deltaTime));
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

    private void DamagePlayer()
    {
        if (PlayerInRange())
        {
            Debug.Log("boo");
            //playerhealth.TakeDamage(damage);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void Attacking()
    {

        if (CooldownTimer >= attackCooldown)
        {
            if (PlayerInRange())
            {
                isAttacking = true;
                rb.linearVelocity = Vector2.zero;
                // Attack logic here
                Debug.Log("Melee enemy attacks for " + damage + " damage!");
                CooldownTimer = 0f; // Reset the cooldown timer after attacking
                anim.SetTrigger("meleeAttack");
            }
        }

    }
    public void EndAttack()
    {
        isAttacking = false;
    }
}
