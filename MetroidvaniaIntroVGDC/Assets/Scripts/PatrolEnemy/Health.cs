using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    private float currentHealth;
    private Animator anim;
    private bool dead = false;
    public GameObject healthItem;
    [Range(0f, 1f)]
    public float itemDropChance = 0.3f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        currentHealth = startingHealth;
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth <= 0)
        {
            if (!dead)
            {
                dead = true;
                anim.SetTrigger("Die");
                DropHealthItem();
                if (GetComponent<meleeEnemy>() != null)
                {
                    GetComponent<meleeEnemy>().enabled = false;
                }

                if (GetComponent<Flying_movement>() != null)
                {
                    GetComponent<Flying_movement>().enabled = false;
                }

                if (GetComponentInParent<EnemyPatrol>() != null)
                {
                    GetComponentInParent<EnemyPatrol>().enabled = false;

                }

                if (GetComponent<Collider2D>() != null)
                {
                    GetComponent<Collider2D>().enabled = false;
                }

                if (GetComponent<Rigidbody2D>() != null)
                {
                    GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                }
            }
        }

        else
        {
            anim.SetTrigger("hurt");
        }
    }

    private void DropHealthItem()
    {
        if (Random.value < itemDropChance)
        {
            Instantiate(healthItem, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        }
            
    }

}
