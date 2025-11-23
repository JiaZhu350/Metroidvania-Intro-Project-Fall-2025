using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    private Rigidbody2D itemRb;
    private Collider2D itemCollider;

    public float dropForce = 5.0f;
    public float pickupDelay = 0.5f;
    public int healingAmt = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemRb = GetComponent<Rigidbody2D>();
        itemCollider = GetComponent<Collider2D>();
        itemCollider.enabled = false; // Disable collider initially
        Invoke(nameof(EnablePickup), pickupDelay); // Enable collider after delay
        itemRb.AddForce(Vector2.up * dropForce, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!itemCollider.enabled) return;

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>().HealthItem();
            //Debug.Log("Yeah you got an item");
            Destroy(gameObject); // Destroy the item after pickup
        }
    }

    private void EnablePickup()
    {
        itemCollider.enabled = true;
    }
}
