using UnityEngine;
//using System;
//using UnityEngine.SceneManagement;

public class spinCloud : MonoBehaviour
{
    public float spinSpeed = 70;
    
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime);
        transform.position = transform.parent.transform.position + new Vector3(Mathf.Sin(3*transform.rotation.z)/3, 0, 0);
    }
}
