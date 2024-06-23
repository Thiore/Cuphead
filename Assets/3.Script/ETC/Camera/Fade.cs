using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    private bool isFadeOut = false;
    private Image image;
    private bool isStartFade = false;
    private bool isFinishFade = false;
    public bool isFinish => isFinishFade;
    public bool isFade => isFadeOut;

    private void Awake()
    {
        image = GetComponent<Image>();
        
    }
    private void Update()
    {
        isFinishFade = false;
        if(isFadeOut) // fadeIn
        {
            if(isStartFade)
            {
                Color FadeAlpha = image.color;
                FadeAlpha.a -= Time.deltaTime;
                image.color = FadeAlpha;
                if (FadeAlpha.a <= 0f)
                {
                    isStartFade = false;
                    isFinishFade = true;
                }
                    
            }
        }
        else
        {
            if (isStartFade)
            {
                Color FadeAlpha = image.color;
                FadeAlpha.a += Time.deltaTime;
                image.color = FadeAlpha;
                if (FadeAlpha.a >= 1f)
                {
                    Debug.Log("¿©±âµé¾î¿È?fade");
                    isStartFade = false;     
                    isFinishFade = true;
                }
            }
        }

    }

    public void StartFadeIn()
    {
        isFadeOut = true;
        isStartFade = true;
    }

    public void StartFadeOut()
    {
        isFadeOut = false;
        isStartFade = true;
    }


}
