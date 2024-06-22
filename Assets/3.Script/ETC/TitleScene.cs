using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    [SerializeField] private NextScene next;

    private void Update()
    {
        if(Input.anyKeyDown)
        {
            next.SetScene(Scene.Menu);
        }
    }
}
