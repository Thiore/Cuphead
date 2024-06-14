using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCamera : MonoBehaviour
{
    [SerializeField] private Camera renderCamera;

    private void Update()
    {
        transform.position = renderCamera.transform.position;
    }
}
