using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextScene : MonoBehaviour
{
    [SerializeField] private eScene nextScene;
    [SerializeField] private Key_Data Key;
    [SerializeField] private Key keyCode;

    private void Update()
    {
       
        if (Input.GetKeyDown(Key.GetKey(keyCode)))
        {
            Scene_Manager.Instance.SetScene(nextScene);
        }
        
       
    }
}
