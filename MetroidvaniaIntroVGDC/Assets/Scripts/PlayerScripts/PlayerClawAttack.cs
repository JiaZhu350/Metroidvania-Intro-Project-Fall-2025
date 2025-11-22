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

    private GameObject enemy;

    private Animator animator;

    public PlayerMovement playerMovement;

    public Health enemyHealth;
    [SerializeField] AudioClip ClawAttackSound;

    public SoundEffectManager soundEffectManager;


    void Awake()
    {
        enemy = GameObject.FindWithTag("Enemy");
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
            Debug.Log("ATTACK");
            //soundEffectManager.PlaySoundFXClip(ClawAttackSound, hitboxTransform);
            animator.SetTrigger("TrAttack");
            Debug.Log(isHitting);
            if (isHitting)
            {
                enemyHealth.TakeDamage(damage);
                Debug.Log(damage);
                Debug.Log("HITTING");
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
        isHitting = Physics2D.OverlapCircle(
            hitboxTransform.position, hitboxRadius, hitLayers);
        float direction = playerMovement.move;
        if (direction == 1)
        {
            Vector3 localTarget = new Vector3(2f, 0, -0.1f);
            hitboxTransform.localPosition = localTarget;
        }
        if (direction == -1)
        {
            Vector3 localTarget = new Vector3(-2f, 0, -0.1f);
            hitboxTransform.localPosition = localTarget;
        }
    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawWireSphere(hitboxTransform.position, hitboxRadius);
    }
}
