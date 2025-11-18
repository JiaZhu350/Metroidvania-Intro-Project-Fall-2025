using System;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    public float damage;
    public PlayerHealth playerHealthScript;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            playerHealthScript.TakeDamage(damage);
        }
    }
}
