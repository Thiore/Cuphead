using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectCreator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject newObject = new GameObject();
        newObject.AddComponent<TestComponent>();
        newObject.name = "Test Component Game Object";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
