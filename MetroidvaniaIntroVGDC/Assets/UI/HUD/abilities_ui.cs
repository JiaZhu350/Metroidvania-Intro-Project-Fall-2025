using UnityEngine;
using UnityEngine.UIElements;

public class ability_ui : MonoBehaviour
{
    //public PlayerControl PlayerControl;
    public UIDocument UIDoc;
    public GameObject player;

    private VisualElement[] ability_icons = new VisualElement[5];

    private void Start()
    {
        //PlayerControl.OnHealthChange += HealthChanged;

        
        ability_icons[0] = UIDoc.rootVisualElement.Q<VisualElement>("wall_c");
        ability_icons[1] = UIDoc.rootVisualElement.Q<VisualElement>("djump_c");
        ability_icons[2] = UIDoc.rootVisualElement.Q<VisualElement>("tongue_c");
        chargeChanged();
    }

    //void Update()
    //{
        
    //}

    //private float timer = .1f;
    //void Update()
    // {
    //     if ((timer-=Time.deltaTime) < 0)
    //     {
    //         timer = (1/30); // reset timer
    //         HealthChanged();
    //    }
    //}

    public void chargeChanged()
    {
        //int CurrentHealth = (int)player.GetComponent<PlayerHealth>().currentHealth;
        //CurrentHealth = (int)(Mathf.Abs(player.transform.position.y))%5;
        //bool[] charged = new bool[5];
        //for (int i = 1; i <= 5; i++)
        //{
        //    VisualElement part = barParts[i-1];
        //    Visibility vis = (Mathf.Floor(CurrentHealth)>=i) ? Visibility.Visible : Visibility.Hidden;
        //    part.style.visibility = vis;
        //}
    }

}
