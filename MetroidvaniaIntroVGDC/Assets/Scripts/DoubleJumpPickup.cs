using UnityEngine;
using UnityEngine.UIElements;

public class DoubleJumpPickup : MonoBehaviour
{
    public UIDocument abilityUI;
    //private VisualElement icon = new VisualElement();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //icon = abilityUI.rootVisualElement.Q("djump_1");
        GameObject player = GameObject.FindWithTag("Player");
        if (player.GetComponent<PlayerUpdatedMovement>().doubleJumpAble)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision happened" + collision.tag);

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerUpdatedMovement>().doubleJumpAble = true;
            //icon.style.display = DisplayStyle.Flex;
            Destroy(gameObject);
        }
    }
}
