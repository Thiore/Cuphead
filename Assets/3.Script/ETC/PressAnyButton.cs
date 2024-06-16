using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PressAnyButton : MonoBehaviour
{
    [SerializeField] private GameObject FadeOut;
    [SerializeField] private float Speed;

    private Text text;
    private Outline outline;
    private Animator fadeOut;


    private float LastTime = 0;
    private int isOn = 1;
    private float FadeOutDuration;

    private void Awake()
    {
        text = GetComponent<Text>();
        outline = GetComponent<Outline>();
        fadeOut = FadeOut.transform.GetComponent<Animator>();
    }

    private void Update()
    {

        if(Time.time > LastTime+Speed)
        {
            LastTime = Time.time;

            if (isOn.Equals(0))
                isOn = 1;
            else
                isOn = 0;

            Color currentColor = text.color;
            currentColor.a = isOn;
            text.color = currentColor;

            currentColor = outline.effectColor;
            currentColor.a = isOn;
            outline.effectColor = currentColor;
        }
        Debug.Log("Time " + Time.time + "Last " + LastTime);

        if(Input.anyKeyDown)
        {
            StartCoroutine(FadeOutScreen());
        }
    }

    private IEnumerator FadeOutScreen()
    {
        FadeOut.SetActive(true);
        AnimatorStateInfo FadeOutStateInfo = fadeOut.GetCurrentAnimatorStateInfo(0);

        FadeOutDuration = FadeOutStateInfo.length;

        yield return new WaitForSeconds(FadeOutDuration);

        SceneManager.LoadScene("Manu");
        
    }


}
