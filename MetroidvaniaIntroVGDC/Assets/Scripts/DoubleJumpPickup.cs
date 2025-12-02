using UnityEngine;

public class DoubleJumpPickup : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision happened" + collision.tag);

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerMovement>().HealthItem();
            Debug.Log("Yeah you got an item");
            Destroy(gameObject);
        }
    }
}
