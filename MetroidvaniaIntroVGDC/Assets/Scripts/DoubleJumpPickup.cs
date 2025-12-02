using UnityEngine;

public class DoubleJumpPickup : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player.GetComponent<PlayerMovement>().doubleJumpAble)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision happened" + collision.tag);

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerMovement>().doubleJumpAble = true;
            Destroy(gameObject);
        }
    }
}
