using Unity.VisualScripting;
using UnityEngine;

public class Win : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public bool fadeIn = false;
    private bool done = false;
    public float TimeToFade;
    private GameObject enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = GameObject.FindWithTag("Enemy");
    }

    // Update is called once per frame
    void Update()
    {

        if (enemy.GetComponent<Health>().dead && !done)
        {
            fadeIn = true;
            done = true;
        }
        if (fadeIn)
        {
            if (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime / TimeToFade;
                if (canvasGroup.alpha >= 1)
                {
                    canvasGroup.alpha = 1;
                    fadeIn = false;
                }
            }
        }
    }
}
