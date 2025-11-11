using UnityEngine;

public class BulletScript : MonoBehaviour
{
    GameObject target;
    public float speed;
    public int damage;
    public float knockbackForce = 5f; // New: amount of knockback
    Rigidbody2D bulletRB;

    void Start()
    {
        bulletRB = GetComponent<Rigidbody2D>();

        if (bulletRB == null)
        {
            Debug.LogWarning("BulletScript: missing Rigidbody2D on bullet prefab. Movement and collisions may not work.");
        }

        target = GameObject.FindGameObjectWithTag("Player");
        if (target != null && bulletRB != null)
        {
            Vector2 moveDir = (target.transform.position - transform.position).normalized;
            bulletRB.linearVelocity = moveDir * speed;
        }

        Destroy(this.gameObject, 2f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleHit(other.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleHit(collision.gameObject);
    }
    private void HandleHit(GameObject other)
    {
        if (other == null) return;

        if (other.CompareTag("Player"))
        {
            // Deal damage (your existing logic)
            Debug.Log("Shooter enemy attacks for " + damage + " damage!");

            // Apply knockback
            Rigidbody2D playerRB = other.GetComponent<Rigidbody2D>();
            if (playerRB != null)
            {
                // Direction from bullet to player
                Vector2 knockbackDir = (other.transform.position - transform.position).normalized;

                // Apply impulse force in that direction
                playerRB.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
            }

            Destroy(gameObject);
            return;
        }

        // Destroy on impact with walls, environment
        Destroy(this.gameObject);
    }

}
