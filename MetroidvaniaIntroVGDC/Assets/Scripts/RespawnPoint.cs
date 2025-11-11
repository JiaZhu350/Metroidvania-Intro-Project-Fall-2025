using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    public bool interacted;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D _collision)
    {
        if(_collision.CompareTag("Player") && Input.GetButtonDown("Interact"))
        {
            interacted = true
        }
    }
}
