using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RoomChanger : MonoBehaviour
{
    public RoomConnection connection;
    public string targetSceneName;
    public Transform spawnPoint;
    public GameObject myPrefab;
    
    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            GameObject player = Instantiate(myPrefab, new Vector3(0.24f, 8.84f, 0f), Quaternion.identity);
        }
        
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
            StartCoroutine(crossFade());

            //RoomConnection.ActiveConnection = connection;
            //SceneManager.LoadScene(targetSceneName);
        }
    }


    public Animator transition;
    public float transitionTime = 1f;

    IEnumerator crossFade()
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        RoomConnection.ActiveConnection = connection;
        SceneManager.LoadScene(targetSceneName);
    }
}