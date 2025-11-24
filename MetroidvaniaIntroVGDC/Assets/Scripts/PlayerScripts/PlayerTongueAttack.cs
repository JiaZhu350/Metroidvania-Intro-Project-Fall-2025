using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTongueAttack : MonoBehaviour
{
    public InputSystem_Actions actions;
    public float hitboxRadius = 0.5f;
    public LayerMask hitLayers;

    public PlayerTongueRope grapplingRope;

    public float damage;

    Collider2D hitEM;

    public Vector2 hit;

    public Vector2 hitEnd;

    public Health enemyHealth;

    private GameObject enemy;

    Rigidbody2D rb;
    void Awake()
    {
        actions = new InputSystem_Actions();
        actions.Player.Enable();
    }
    void OnEnable()
    {
        actions.Player.Tongue.performed += OnClawAttack;
        actions.Player.Tongue.canceled += OnClawAttack;
    }
    void OnDisable()
    {
        actions.Player.Tongue.performed -= OnClawAttack;
        actions.Player.Tongue.canceled -= OnClawAttack;
    }
    private void OnClawAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (hitEM != null)
            {
                Debug.Log("ENEMY HIT TONGUE");
                enemyHealth = hitEM.GetComponent<Health>();
                enemyHealth.TakeDamage(damage);
                // Implement logic of when the tongue attack hits an enemy
            }
            if (hitEM == null)
            {
                //Nothing
            }
            else
            {
                // Implement logic for when the tongue attack misses
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        hit = grapplingRope.currentPosition;
        hitEM = Physics2D.OverlapCircle(hit, hitboxRadius, hitLayers);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(hit, hitboxRadius);
    }
}

