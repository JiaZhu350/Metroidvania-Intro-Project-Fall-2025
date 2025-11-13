using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public bool interacted; // Has this checkpoint been activated?
    [HideInInspector] public bool playerInside = false; // Is the player currently in the trigger?

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = true;
            Debug.Log("Player entered respawn area!");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = false;
            Debug.Log("Player left respawn area!");
        }
    }

    // Called by PlayerHealth when player presses Interact
    public void TryActivate()
    {
        if (playerInside && !interacted)
        {
            interacted = true;
            GameManager.Instance.SetRespawn(this);
            Debug.Log("Respawn point activated!");
        }
    }
}