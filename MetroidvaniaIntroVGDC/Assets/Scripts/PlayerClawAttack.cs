using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerClawAttack : MonoBehaviour
{
    public InputSystem_Actions actions;
    public Transform hitboxTransform;
    public float hitboxRadius = 0.5f;
    public LayerMask hitLayers;

    public float damage;

    bool isHitting;

    private Animator animator;

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
                Debug.Log("Claw Attack Hit!");
                animator.SetTrigger("TrAttack");
                animator.SetTrigger("TrNeutral");
                
                // Implement logic for when the claw attack hits an enemy
            }
            else
            {
                Debug.Log("Claw Attack Missed!");
                // Implement logic for when the claw attack misses
            }
        }
        else if (context.canceled)
        {
            Debug.Log("Claw Attack Canceled");
            // Implement logic for when the claw attack is canceled, if needed
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isHitting = Physics2D.OverlapCircle(
            hitboxTransform.position, hitboxRadius, hitLayers);
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(hitboxTransform.position, hitboxRadius);
    }
}
