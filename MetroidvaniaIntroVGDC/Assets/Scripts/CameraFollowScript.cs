using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraFollowScript : MonoBehaviour
{
    public GameObject mainCamera;
    private float _minX, _minY, _maxX, _maxY;
    private GameObject _cameraZone;
    
    private void Update()
    {
        Vector3 desiredCameraPos = new Vector3(
            Mathf.Clamp(transform.position.x, _minX, _maxX),
            Mathf.Clamp(transform.position.y, _minY, _maxY),
            mainCamera.transform.position.z);
        mainCamera.transform.position = desiredCameraPos;
        
        // Debug.Log("X position: " + transform.position.x);
        // Debug.Log("Y position: " + transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("CameraZone"))
        {
            Debug.Log(other.gameObject.name);
            _cameraZone = other.gameObject;
            _minX = _cameraZone.transform.Find("Left Bound").transform.position.x;
            _maxX = _cameraZone.transform.Find("Right Bound").transform.position.x;
            _minY = _cameraZone.transform.Find("Lower Bound").transform.position.y;
            _maxY = _cameraZone.transform.Find("Upper Bound").transform.position.y;
        }
    }
}
