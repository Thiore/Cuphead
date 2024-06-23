using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    private float FadeTime;
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        FadeTime = 1f;
    }
    void Start()
    {
        _ = StartCoroutine(Fade_co());
       

    }
    

    IEnumerator Fade_co()
    {
        Color FadeAlpha = image.color;
        while (FadeTime>=0f)
        {
            FadeTime -= Time.deltaTime;
            FadeAlpha.a = FadeTime;
            image.color = FadeAlpha;
            yield return null;

        }
        Scene_Manager.Instance.StartLoading();
        Debug.Log("loadingScene");
        while(!Scene_Manager.Instance.isNext)
        {
            Debug.Log("그럼 여기는?");
            yield return null;
        }
        while (FadeTime <= 1f)
        {
            FadeTime += Time.deltaTime;
            FadeAlpha.a = FadeTime;
            image.color = FadeAlpha;
            yield return null;

        }
        Scene_Manager.Instance.CompleteFade = true;
        yield break;
    }
}
