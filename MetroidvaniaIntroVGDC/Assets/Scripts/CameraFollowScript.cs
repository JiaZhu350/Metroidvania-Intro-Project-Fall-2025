using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollowScript : MonoBehaviour
{
    public GameObject player;
    public float minX, minY, maxX, maxY;
    
    private void Update()
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
