using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerClawAttack : MonoBehaviour
{
    public InputSystem_Actions actions;
    public Transform hitboxTransform;
    public float hitboxRadius = 0.5f;
    public LayerMask hitLayers;

    public float damage;

    Collider2D hit;

    private Animator animator;

    public PlayerMovement playerMovement;

    public Health enemyHealth;
    [SerializeField] AudioClip ClawAttackSound;


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
            SoundEffectManager.Instance.PlaySoundFXClip(ClawAttackSound, transform);
            animator.SetTrigger("TrAttack");
            if (hit != null)
            {
                Debug.Log("ENEMY HIT CLAW");
                enemyHealth = hit.GetComponent<Health>();
                enemyHealth.TakeDamage(damage);
                // Implement logic for when the claw attack hits an enemy
            }
        }
        else if (context.canceled)
        {
            animator.SetTrigger("TrNeutral");
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
        hit = Physics2D.OverlapCircle(
    hitboxTransform.position, hitboxRadius, hitLayers);
        float direction = playerMovement.move;
        if (direction == 1)
        {
            Vector3 localTarget = new Vector3(3.3f, 0, -0.1f);
            hitboxTransform.localPosition = localTarget;
        }
        if (direction == -1)
        {
            Vector3 localTarget = new Vector3(-3.3f, 0, -0.1f);
            hitboxTransform.localPosition = localTarget;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(hitboxTransform.position, hitboxRadius);
    }
}
