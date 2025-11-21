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
        baseRot = ui_element.style.rotate.value;
    }

    private float func(float p, float td)
    {
        t+=td*bounceSpeed;
        while (t >= rotPeriod*sizePeriod*2*Mathf.PI)
        {
            t -= rotPeriod*sizePeriod*2*Mathf.PI;
        }

        return(
            Mathf.Sin(t*p)
        );
    }
    

    public Vector2 scaleRange = new Vector2(1,1);
    public float bounceSpeed = 1f;
    public float rotAmount = 8;
    public int rotPeriod = 1;
    public int sizePeriod = 5;
    private Rotate baseRot = new Rotate(0);

    // Update is called once per frame
    void Update()
    {
        // checks
        if (rotPeriod<1)
        {rotPeriod = (int)1;}
        if (sizePeriod<1)
        {sizePeriod = (int)1;}

        //rotate
        ui_element.style.rotate = new Rotate(baseRot.angle.value + (rotAmount*func( 1f/(float)rotPeriod,  Time.deltaTime)) );

        //scale
        float T = Mathf.Abs( func(1f/(float)sizePeriod,  Time.deltaTime) );
        T = scaleRange.x + T*Mathf.Abs(scaleRange.y - scaleRange.x);
        ui_element.style.scale = new Scale(new Vector2(T, T));
    }
}
