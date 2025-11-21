using System;
using System.Collections;
using UnityEngine;

public class DamageOverTimeScript : MonoBehaviour
{
    public float damage;
    public float damageCooldown;
    public PlayerHealth player;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null && other.CompareTag("Player"))
        {
            StartCoroutine(DealDamage());
        }
    }

    private IEnumerator DealDamage()
    {
        Collider2D thisCollider = GetComponent<Collider2D>();
        thisCollider.enabled = false;

        var playerHealthScript = player.GetComponent<PlayerHealth>();
        playerHealthScript.TakeDamage(damage);

        yield return new WaitForSeconds(damageCooldown);
        thisCollider.enabled = true;
    }

}
