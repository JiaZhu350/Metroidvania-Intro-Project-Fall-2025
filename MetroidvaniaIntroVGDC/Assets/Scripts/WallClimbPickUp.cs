using UnityEngine;
using UnityEngine.UIElements;

public class WallClimbPickUp : MonoBehaviour
{
    //public UIDocument abilityUI;
    //private VisualElement icon = new VisualElement();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //icon = abilityUI.rootVisualElement.Q("wall_1");
        GameObject player = GameObject.FindWithTag("Player");
        if (player.GetComponent<PlayerUpdatedMovement>().wallJumpAble)
        {
            Destroy(gameObject);
            Debug.Log("Destroyed Wall Climb Pick Up on Start");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision happened" + collision.tag);

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerUpdatedMovement>().wallJumpAble = true;
            //icon.style.display = DisplayStyle.Flex;
            Destroy(gameObject);
        }
    }
}
