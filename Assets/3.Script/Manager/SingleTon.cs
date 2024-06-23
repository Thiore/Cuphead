using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTon<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance = null;
    

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                var obj = FindObjectOfType<T>();
                if(obj != null)
                {
                    instance = obj;
                }
                else
                {
                    var newObj = new GameObject().AddComponent<T>();
                    newObj.name = typeof(T).ToString() + "_Singleton";
                    instance = newObj;

                    DontDestroyOnLoad(newObj);  
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        InitializeSingleton();
    }
    protected virtual void InitializeSingleton()
    {
        if (!Application.isPlaying)
            return;

        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
