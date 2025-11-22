using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomChanger : MonoBehaviour
{
    public RoomConnection connection;
    public string targetSceneName;
    public Transform spawnPoint;

    private void Start()
    {
        if (connection == RoomConnection.ActiveConnection)
        {
            FindObjectOfType<PlayerMovement>().gameObject.transform.position = spawnPoint.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.collider.gameObject;
        if (player != null && player.CompareTag("Player"))
        {
            RoomConnection.ActiveConnection = connection;
            SceneManager.LoadScene(targetSceneName);
        }
    }
}