using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    private Rigidbody2D itemRb;
    public float dropForce = 5.0f;
    public int healingAmt = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        itemRb = GetComponent<Rigidbody2D>();
        itemRb.AddForce(Vector2.up * dropForce, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision happened" + collision.tag);

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>().HealthItem();
            Debug.Log("Yeah you got an item");
            Destroy(gameObject); // Destroy the item after pickup
        }
    }
}
