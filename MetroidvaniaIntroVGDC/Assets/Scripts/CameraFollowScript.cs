using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollowScript : MonoBehaviour
{
    private GameObject player;
    public float minX, minY, maxX, maxY;

    private void Awake()
    {
        
    }

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            Vector3 desiredCameraPos = new Vector3(
                Mathf.Clamp(player.transform.position.x, minX, maxX),
                Mathf.Clamp(player.transform.position.y, minY, maxY),
                transform.position.z);
            transform.position = desiredCameraPos;

            // Debug.Log("X position: " + transform.position.x);
            // Debug.Log("Y position: " + transform.position.y);
        }
    }
}
