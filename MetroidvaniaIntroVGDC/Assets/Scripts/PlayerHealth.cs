using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    private float currentHealth;
    private Animator anim;
    private bool dead = false;
    public GameObject healthItem;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        currentHealth = startingHealth - 2;
        Debug.Log("Player health: " + currentHealth);
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
    }

    public void HealthItem()
    {
        currentHealth = Mathf.Clamp(currentHealth + 20, 0, startingHealth);
        Debug.Log("Player health increased: " + currentHealth);
    }
}
