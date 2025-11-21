using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth;  // made public -Bryce
    private Animator anim;
    public InputSystem_Actions actions;
    public bool dead = false;
    public float _heal = 1f;
    public int healthItems = 10;
    public int MaxHealthItems = 3;
    public float RespawnTime = 1f;

    public GameObject HPUI;  // -Bryce
    private void updateUI(){HPUI.GetComponent<HP_uiHandler>().HealthChanged();}  // -Bryce

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        currentHealth = startingHealth;
        Debug.Log("Player health: " + currentHealth);
        actions = new InputSystem_Actions();
        actions.Player.Enable();
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        actions.Player.Heal.performed += Heal;
        actions.Player.Heal.canceled += Heal;
        actions.Player.Interact.performed += OnInteractRespawn;
        actions.Player.Interact.canceled += OnInteractRespawn;
    }

    void OnDisable()
    {
        actions.Player.Heal.performed -= Heal;
        actions.Player.Heal.canceled -= Heal;
        actions.Player.Interact.performed -= OnInteractRespawn;
        actions.Player.Interact.canceled -= OnInteractRespawn;
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
                updateUI();
                FreezeMovement();
                //anim.SetTrigger("Die");
                Debug.Log("Player died");
                // Implement player death logic here (e.g., reload scene, show game over screen)
                GameManager.Instance.StartFadeAndRespawn(RespawnTime); // Notify GameManager of player death
            }
        }
        else
        {
            Debug.Log("Player hurt");
            Debug.Log("current Health: " + currentHealth);
        }
        updateUI();  // -Bryce
    }

    public void HealthItem()
    {
       // if (healthItems < MaxHealthItems)
        //{
           // healthItems++;
        //}
        currentHealth = Mathf.Clamp(currentHealth + _heal, 0, startingHealth);
        updateUI();  // -Bryce

            
    }

    public void Heal(InputAction.CallbackContext context)
    {
        //if (context.performed)
       // {
           // if (healthItems > 0 && currentHealth < startingHealth && !dead)
           // {
               // healthItems--;
               // currentHealth = Mathf.Clamp(currentHealth + _heal, 0, startingHealth);
               // updateUI();  // -Bryce
           // }
       // }
    }

   public void Respawn()
    {
        if (dead)
        {
            dead = false;
            currentHealth = startingHealth;
            //anim.Play("Idle");
            Debug.Log("Player respawned with health: " + currentHealth);
            updateUI();  // -Bryce
            UnfreezeMovement();
        }
    }
    private void OnInteractRespawn(InputAction.CallbackContext context)
    {
       // Debug.Log("Interact button pressed for respawn point");
        // Find the respawn point in the scene
        RespawnPoint rp = Object.FindAnyObjectByType<RespawnPoint>();

        if (rp != null && rp.playerInside && !rp.interacted)
        {
            rp.TryActivate();
            resting();
        }
    }

    private void resting()
    {
        FreezeMovement();
        currentHealth = startingHealth;
        updateUI();  // -Bryce
        Debug.Log("Player healed: " + currentHealth);
    }

    public void FreezeMovement()
    {
        GetComponent<PlayerMovement>().enabled = false;
        Object.FindAnyObjectByType<PlayerClawAttack>().enabled = false;
    }

    public void UnfreezeMovement()
    {
        GetComponent<PlayerMovement>().enabled = true;
        Object.FindAnyObjectByType<PlayerClawAttack>().enabled = true;
    }
}
