using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCamera : MonoBehaviour
{
    [SerializeField] private Camera renderCamera;

    

    private void LateUpdate()
    {
        transform.position = new Vector2(renderCamera.transform.position.x, renderCamera.transform.position.y);
    }

}
