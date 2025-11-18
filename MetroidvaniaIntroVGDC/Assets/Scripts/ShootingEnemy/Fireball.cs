using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 7f;
    public int damage = 10;
    public float arcHeight = 3f;   // Increase this for a higher arc
    
    private Rigidbody2D bulletRB;
    private Transform target;

    void Start()
    {
        bulletRB = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (bulletRB == null || target == null) return;

        // Calculate firing direction
        Vector2 direction = (target.position - transform.position);

        // Add upward arc
        direction.y += arcHeight;

        // Launch projectile
        bulletRB.AddForce(direction.normalized * speed, ForceMode2D.Impulse);

        // Destroy after a few seconds
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Health>()?.TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (!other.isTrigger) // hit wall/ground
        {
            Destroy(gameObject);
        }
    }
}
