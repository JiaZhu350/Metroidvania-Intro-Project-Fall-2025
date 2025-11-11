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

    public PlayerMovement playerMovement;

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
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isHitting = Physics2D.OverlapCircle(
            hitboxTransform.position, hitboxRadius, hitLayers);
        float direction = playerMovement.move;
        if (direction == 1)
        {
            Vector3 localTarget = new Vector3(0.5f, 0, 0);
            hitboxTransform.localPosition = localTarget;
        }
        if (direction == -1)
        {
            Vector3 localTarget = new Vector3(-0.5f, 0, 0);
            hitboxTransform.localPosition = localTarget;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(hitboxTransform.position, hitboxRadius);
    }
}
