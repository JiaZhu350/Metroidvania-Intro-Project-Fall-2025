using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTongueAttack : MonoBehaviour
{
    public InputSystem_Actions actions;
    public float hitboxRadius = 0.5f;
    public LayerMask hitLayers;

    public PlayerTongueRope grapplingRope;

    public float damage;

    bool isHitting;

    public Vector2 hit;

    public Vector2 hitEnd;

    Rigidbody2D rb;
    void Awake()
    {
        actions = new InputSystem_Actions();
        actions.Player.Enable();
    }
    void OnEnable()
    {
        actions.Player.Attack.performed += OnClawAttack;
        actions.Player.Attack.canceled += OnClawAttack;
    }
    void OnDisable()
    {
        actions.Player.Attack.performed -= OnClawAttack;
        actions.Player.Attack.canceled -= OnClawAttack;
    }
    private void OnClawAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isHitting)
            {
                Debug.Log("Tongue Hit!");
                // Implement logic of when the tongue attack hits an enemy
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
    }

    // Update is called once per frame
    void Update()
    {
        hit = grapplingRope.currentPosition;
        isHitting = Physics2D.OverlapCircle(hit, hitboxRadius, hitLayers);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(hit, hitboxRadius);
    }
}

