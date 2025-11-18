using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) return; // prevent self-hit
        Destroy(gameObject);
    }
}
