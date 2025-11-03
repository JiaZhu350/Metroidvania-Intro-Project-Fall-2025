using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }
}
