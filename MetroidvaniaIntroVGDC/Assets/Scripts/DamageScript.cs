using System;
using UnityEngine;

public class DamageScript : MonoBehaviour
{
    public float damage;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // temporary
            Debug.Log("Damage: " + damage);
        }
    }
}
