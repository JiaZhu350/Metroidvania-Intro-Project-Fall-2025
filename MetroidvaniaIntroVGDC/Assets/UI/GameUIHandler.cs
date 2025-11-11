using UnityEngine;
using UnityEngine.UIElements;

public class GameUIHandler : MonoBehaviour
{
    //public PlayerControl PlayerControl;
    public UIDocument UIDoc;
    public GameObject player;

    private VisualElement[] barParts = new VisualElement[5];

    private void Start()
    {
        //PlayerControl.OnHealthChange += HealthChanged;

        
        barParts[0] = UIDoc.rootVisualElement.Q<VisualElement>("barFill_1");
        barParts[1] = UIDoc.rootVisualElement.Q<VisualElement>("barFill_2");
        barParts[2] = UIDoc.rootVisualElement.Q<VisualElement>("barFill_3");
        barParts[3] = UIDoc.rootVisualElement.Q<VisualElement>("barFill_4");
        barParts[4] = UIDoc.rootVisualElement.Q<VisualElement>("barFill_5");
        HealthChanged();
    }

    //void Update()
    //{
        
    //}

    private float timer = .1f;
    void Update()
    {
        if ((timer-=Time.deltaTime) < 0)
        {
            timer = .1f; // reset timer
            HealthChanged();
        }
    }


    void HealthChanged()
    {
        //float healthRatio = (float)PlayerControl.CurrentHealth / PlayerControl.MaxHealth;
        float healthRatio = player.transform.position.y;
        //float healthPercent = Mathf.Lerp(0, 66, healthRatio);
        bool[] showList = new bool[5];
        for (int i = 1; i <= 5; i++)
        {
            VisualElement part = barParts[i-1];
            Visibility vis = (Mathf.Floor(healthRatio)>=i) ? Visibility.Visible : Visibility.Hidden;
            part.style.visibility = vis;
        }
        //m_barMask.style.width = Length.Percent(healthPercent);
    }

}
