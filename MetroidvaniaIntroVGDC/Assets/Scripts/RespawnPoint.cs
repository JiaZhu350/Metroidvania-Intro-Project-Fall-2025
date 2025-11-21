using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class RespawnPoint : MonoBehaviour
{
    public bool interacted; // Has this checkpoint been activated?
    [HideInInspector] public bool playerInside = false; // Is the player currently in the trigger?
    private Transform child;
    public CanvasGroup canvasGroup;
    FadeInOut fade;
    [SerializeField] private AudioClip respawnSound;
    private void Start()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
        }
        fade = GetComponent<FadeInOut>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = true;
           // Debug.Log("Player entered respawn area!");
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 1f;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInside = false;
           // Debug.Log("Player left respawn area!");
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
            }
        }
    }

    // Called by PlayerHealth when player presses Interact
    public void TryActivate()
    {
        if (playerInside && !interacted)
        {
            interacted = true;
            StartCoroutine(SetSpawn(1));
           // Debug.Log("Respawn point activated!");
        }
    }

    public IEnumerator SetSpawn(float fadeTime)
    {
        if (fade != null)
        {
            fade.TimeToFade = fadeTime;
            fade.FadeIn();
            SoundEffectManager.Instance.PlaySoundFXClip(respawnSound, transform);
            yield return new WaitForSeconds(fadeTime + 2);
            GameManager.Instance.SetRespawn(this);
            fade.FadeOut();
            interacted = false;
        }
    }
}