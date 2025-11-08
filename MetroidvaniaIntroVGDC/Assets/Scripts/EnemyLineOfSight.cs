using UnityEngine;

public class EnemyLineOfSight : MonoBehaviour
{
    public float viewDistance = 5f;
    public float fieldOfViewAngle = 90f;
    public LayerMask obstacleLayer;

    private Transform playerTransform;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform == null) return;

        Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
        float angleToPlayer = Vector2.Angle(transform.right, directionToPlayer); // Adjust 'transform.right' based on enemy's forward direction

        if (angleToPlayer < fieldOfViewAngle / 2f)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, viewDistance, obstacleLayer);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                Debug.Log("Player detected!");
                // Implement enemy behavior (e.g., chase, attack)
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);

        Vector2 fovDirection1 = Quaternion.Euler(0, 0, fieldOfViewAngle / 2) * transform.right;
        Vector2 fovDirection2 = Quaternion.Euler(0, 0, -fieldOfViewAngle / 2) * transform.right;

        Gizmos.DrawLine(transform.position, (Vector2)transform.position + fovDirection1 * viewDistance);
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + fovDirection2 * viewDistance);
    }

}
