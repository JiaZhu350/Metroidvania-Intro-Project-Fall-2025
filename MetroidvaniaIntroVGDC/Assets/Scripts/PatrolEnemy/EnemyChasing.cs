using UnityEngine;

public class EnemyChasing : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float chaseSpeed = 5f; // Speed at which the enemy chases the player
    public float jumpForce = 5f; // Force applied when the enemy jumps
    public LayerMask groundLayer; // Layer mask to identify ground

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool shouldJump;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);

        float direction = Mathf.Sign(player.position.x - transform.position.x);

        bool isPlayerAbove = Physics2D.Raycast(transform.position, Vector2.up, 3f, 1 << player.gameObject.layer);

        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(direction * chaseSpeed, rb.linearVelocity.y);
        }

        RaycastHit2D gapAhead = Physics2D.Raycast(transform.position + new Vector3(direction * 0.5f, 0, 0), Vector2.down, 2f, groundLayer);

    }
}
