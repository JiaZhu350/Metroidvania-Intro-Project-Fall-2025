using UnityEngine;

public class ContactDamage : MonoBehaviour
{
    [SerializeField] protected float damage;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Something has entered me");
        if (collision.tag == "Player")
            Debug.Log("Player has touched me");
            collision.GetComponent<PlayerHealth>().TakeDamage(damage);
    }
}