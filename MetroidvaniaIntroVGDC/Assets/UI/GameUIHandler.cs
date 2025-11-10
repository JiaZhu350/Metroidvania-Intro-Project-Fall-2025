using UnityEngine;
using UnityEngine.UIElements;

public class GameUIHandler : MonoBehaviour
{
    //public PlayerControl PlayerControl;
    public UIDocument UIDoc;
    public GameObject player;

    private VisualElement m_HealthBarMask;

    private void Start()
    {
        //PlayerControl.OnHealthChange += HealthChanged;

        m_HealthBarMask = UIDoc.rootVisualElement.Q<VisualElement>("barMask");
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
        float healthPercent = Mathf.Lerp(0, 66, healthRatio);
        m_HealthBarMask.style.width = Length.Percent(healthPercent);
    }

}
