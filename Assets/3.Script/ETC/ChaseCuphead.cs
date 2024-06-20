using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCuphead : MonoBehaviour
{
    [SerializeField] private GameObject Cuphead;
    

    private void Update()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(transform.position.x, Cuphead.transform.position.x, 0.1f);
        transform.position = pos;
        


    }
}
