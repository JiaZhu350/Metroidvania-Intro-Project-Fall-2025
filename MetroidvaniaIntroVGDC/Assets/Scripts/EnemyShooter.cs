using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float fireRate = 1f;
    private float fireTimer = 0f;

    void Update()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireRate)
        {
            Fire();
            fireTimer = 0f;
        }
    }

    void Fire()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("Projectile prefab not assigned!");
            return;
        }

        Instantiate(projectilePrefab, transform.position, transform.rotation);
    }
}
