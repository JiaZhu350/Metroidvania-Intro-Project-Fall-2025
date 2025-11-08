using UnityEngine;

public class BulletScript : MonoBehaviour
{
    GameObject target;
    public float speed;
    public int damage;
    Rigidbody2D bulletRB;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

        // If we hit the player, try to apply damage via Health component
        if (other.CompareTag("Player"))
        {
            //Attack logic here
            Debug.Log("Shooter enemy attacks for " + damage + " damage!");
            Destroy(gameObject);
            return;
        }

        // Otherwise destroy on impact (walls, environment)
        Destroy(this.gameObject);
    }
}
