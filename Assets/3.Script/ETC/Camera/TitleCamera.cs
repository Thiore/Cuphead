using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleCamera : MonoBehaviour
{

    [SerializeField] GameObject CameraPrefab;

    private static TitleCamera instance;

    private GameObject CurrentCamera;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "TitleScreen" || scene.name == "Menu")
        {
            if(CurrentCamera == null)
            {
                CurrentCamera = Instantiate(CameraPrefab);
                DontDestroyOnLoad(CurrentCamera);
            }
        }
        else
        {
            DestroyCurrentCamera();
        }
           
        
    }

    private void DestroyCurrentCamera()
    {
        if(CurrentCamera != null)
        {
            Destroy(CurrentCamera);
            CurrentCamera = null;
        }
        
    }

}
