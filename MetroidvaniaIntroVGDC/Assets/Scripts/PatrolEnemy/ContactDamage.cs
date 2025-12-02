using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField] protected float damage;
    public float knockbackForce = 4f;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Something has entered me");
        if (collision.tag == "Player")
        {
            if (collision.GetComponent<PlayerHealth>() != null)
            {
                Debug.Log("Player has touched me");
                collision.GetComponent<PlayerHealth>().TakeDamage(damage);
                KnockBack(collision);
            }
                
        }
            
    }

    void KnockBack(Collider2D player)
    {
        Rigidbody2D playerRB = player.GetComponent<Rigidbody2D>();
            if (playerRB != null)
            {
                
                Vector2 knockbackDir = (player.transform.position - transform.position).normalized;
                Debug.Log(knockbackDir);
                playerRB.linearVelocity = Vector2.zero;
                playerRB.AddForce(knockbackDir * knockbackForce, ForceMode2D.Impulse);
            }
    }
}