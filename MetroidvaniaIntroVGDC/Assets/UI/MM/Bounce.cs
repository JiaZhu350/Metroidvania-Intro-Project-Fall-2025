using UnityEngine;
using UnityEngine.UIElements;

public class Bounce : MonoBehaviour
{

    
    public UIDocument ui_doc;
    private VisualElement ui_element;
    private float t;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ui_element = ui_doc.rootVisualElement.Q<VisualElement>("Title");
    }

    private float func(float p)
    {
        t+=.001f;
        while (t >= 10*Mathf.PI)
        {
            t -= 10*Mathf.PI;
        }

        return(
            8*(Mathf.Sin(t*p))
        );
    }

    // Update is called once per frame
    void Update()
    {
        ui_element.style.rotate = new Rotate(func(1f));
        ui_element.style.scale = new Scale( new Vector2(Mathf.Abs(func(1f/5)/40)+1, Mathf.Abs(func(1f/5)/40)+1 ));
    }
}
