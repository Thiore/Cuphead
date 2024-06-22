using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleTon<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;
    public static bool HasInstance => instance != null;
    public static T TryGetInstance() => HasInstance ? instance : null;
    public static T Current => instance;

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<T>();
                if(instance == null)
                {
                    var obj = new GameObject();
                    obj.name = typeof(T).Name + "_AutoCreated";
                    instance = obj.AddComponent<T>();
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

        instance = this as T;
    }
}
