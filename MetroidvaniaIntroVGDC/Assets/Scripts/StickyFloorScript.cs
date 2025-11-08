using System;
using UnityEngine;

public class StickyFloorScript : MonoBehaviour
{
    private PlayerMovement _playerMovement;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerMovement = other.gameObject.GetComponent<PlayerMovement>();
            _playerMovement.speed /= 2.0f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerMovement = other.gameObject.GetComponent<PlayerMovement>();
            _playerMovement.speed *= 2.0f;
        }
    }
}
