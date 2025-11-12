using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth;  // made public -Bryce
    private Animator anim;
    public InputSystem_Actions actions;
    private bool dead = false;
    public float _heal = 1f;
    public int healthItems = 3;

    public GameObject HPUI;  // -Bryce
    private void updateUI(){HPUI.GetComponent<HP_uiHandler>().HealthChanged();}  // -Bryce

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        currentHealth = startingHealth - 2;
        Debug.Log("Player health: " + currentHealth);
        actions = new InputSystem_Actions();
        actions.Player.Enable();
    }

    private void OnEnable()
    {
        actions.Player.Heal.performed += Heal;
        actions.Player.Heal.canceled += Heal;
    }

    void OnDisable()
    {
        actions.Player.Attack.performed -= Heal;
        actions.Player.Attack.canceled -= Heal;
    }

    // Update is called once per frame
    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        if (currentHealth <= 0)
        {
            if (!dead)
            {
                dead = true;
                //anim.SetTrigger("Die");
                Debug.Log("Player died");
                // Implement player death logic here (e.g., reload scene, show game over screen)
            }
        }
        else
        {
            Debug.Log("Player hurt");
            // anim.SetTrigger("hurt");
        }
        updateUI();  // -Bryce
    }

    public void HealthItem()
    {
        healthItems++;
    }

    public void Heal(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Heal button pressed");
            if (healthItems > 0 && currentHealth < startingHealth && !dead)
            {
                healthItems--;
                currentHealth = Mathf.Clamp(currentHealth + _heal, 0, startingHealth);
                Debug.Log("Player healed: " + currentHealth);
                updateUI();  // -Bryce
            }
        }
    }
}
