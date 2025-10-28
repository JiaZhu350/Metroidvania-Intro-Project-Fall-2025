using UnityEngine;

public class melee_enemy : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerlayer;
    private float CooldownTimer = Mathf.Infinity;

    //Refrences
    private Health playerhealth;
    private Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
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
                // Attack logic here
                Debug.Log("Melee enemy attacks for " + damage + " damage!");
                CooldownTimer = 0f; // Reset the cooldown timer after attacking
                anim.SetTrigger("meleeAttack");
            }
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
}
