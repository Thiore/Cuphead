using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scene
{
    TitleScene = 0,
    Menu,
    Intro,
    Tutorial,
    Inkwell,
    ForestFollies
}
public class NextScene : MonoBehaviour
{
    public void SetScene(Scene nextScene)
    {
        SceneManager.LoadScene(nextScene.ToString());
    }


}
