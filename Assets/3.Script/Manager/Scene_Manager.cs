using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum eScene
{
    TitleScreen = 0,
    Menu,
    Intro,
    Tutorial,
    Inkwell,
    ForestFollies
}
public class Scene_Manager : SingleTon<Scene_Manager>
{
    private eScene nextScene;
    private bool isNextScene = false;
    private bool isFadeOut = false;

    public bool isNext => isNextScene;
    public bool CompleteFade { set { isFadeOut = value; } }

    protected override void Awake()
    {
        base.Awake();
    }
    public void SetScene(eScene nextScene)
    {
        this.nextScene = nextScene;
        SceneManager.LoadScene("Loading");
    }


    public IEnumerator LoadSceneProcess()
    {
        Debug.Log(nextScene.ToString());
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene.ToString());
        op.allowSceneActivation = false;
        float range = 0f;
        float timer = 0f;
        while(!op.isDone)
        {
            yield return null;
            if (op.progress < 0.9f)
            {
                range = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                range = Mathf.Lerp(0.9f, 1f, timer);
                isNextScene = true;
                //Debug.Log(range);
                while (range >= 1f)
                {
                    
                    if (isFadeOut)
                    {
                        isNextScene = false;
                        op.allowSceneActivation = true;
                        isFadeOut = false;
                        yield break;
                    }
                    yield return null;
                }
            }
        }
    }

    public void StartLoading()
    {
        _ = StartCoroutine(LoadSceneProcess());
    }
}
