using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public bool fadeIn = false;
    public bool fadeOut = false;

    public float TimeToFade;

    // Update is called once per frame
    void Update()
    {
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

        if (fadeOut)
        {
            if (canvasGroup.alpha >= 0)
            {
                canvasGroup.alpha -= Time.deltaTime / TimeToFade;
                if (canvasGroup.alpha == 0)
                {
                    canvasGroup.alpha = 0;
                    fadeIn = false;
                }
            }
        }
    }

    public void FadeIn()
    {
        fadeOut = false;
        fadeIn = true;
        canvasGroup.alpha = 0;
        Debug.Log("Fading IN");
    }

    public void FadeOut()
    {
        fadeIn = false;
        fadeOut = true;
        canvasGroup.alpha = 1;
        Debug.Log("Fading Out");
    }

}
