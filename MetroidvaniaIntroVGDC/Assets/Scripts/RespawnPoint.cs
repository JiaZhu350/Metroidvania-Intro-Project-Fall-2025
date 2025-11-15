using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RespawnPoint : MonoBehaviour
{
    public bool interacted; // Has this checkpoint been activated?
    [HideInInspector] public bool playerInside = false; // Is the player currently in the trigger?
    private Transform child;
    public CanvasGroup canvasGroup;

    private void Start()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = true;
            Debug.Log("Player entered respawn area!");
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = false;
            Debug.Log("Player left respawn area!");
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }
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