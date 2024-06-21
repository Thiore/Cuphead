using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCuphead : MonoBehaviour
{
    [SerializeField] private GameObject Cuphead;
    [SerializeField] private Stage_Data stageData;
    

    private void Update()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(transform.position.x, Cuphead.transform.position.x, 0.1f);
        transform.position = pos;
    }
    private void LateUpdate()
    {
        transform.position =
                new Vector3(
                Mathf.Clamp(transform.position.x, stageData.LimitMin.x +6f, stageData.LimitMax.x - 6f),
                transform.position.y,transform.position.z);
    }
}
