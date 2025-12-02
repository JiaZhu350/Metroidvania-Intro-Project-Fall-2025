using UnityEngine;

public class grapplePickUp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player.GetComponentInChildren<PlayerTongueGun>().grappleAble)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision happened" + collision.tag);

        if (collision.CompareTag("Player"))
        {
            collision.GetComponentInChildren<PlayerTongueGun>().grappleAble = true;
            collision.GetComponentInChildren<PlayerTongueAttack>().grappleAble = true;
            Destroy(gameObject);
        }
    }
}
