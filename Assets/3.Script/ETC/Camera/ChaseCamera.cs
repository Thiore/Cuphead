using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCamera : MonoBehaviour
{
    [SerializeField] private Camera renderCamera;
    [SerializeField] private Stage_Data stageData;
    [SerializeField] private bool isCamera;


    private void LateUpdate()
    {
        if (isCamera)
            transform.position = new Vector3(renderCamera.transform.position.x, renderCamera.transform.position.y,renderCamera.transform.position.z);
        else
            transform.position = new Vector2(renderCamera.transform.position.x, renderCamera.transform.position.y);

        transform.position =
                new Vector3(
                Mathf.Clamp(transform.position.x, stageData.LimitMin.x + 6f, stageData.LimitMax.x - 6f),
                transform.position.y, transform.position.z);


    }

}
