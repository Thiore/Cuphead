using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCuphead : MonoBehaviour
{
    [SerializeField] private GameObject Cuphead;

    private void Update()
    {
        Vector2 pos = transform.position;
        pos.x = Mathf.Lerp(transform.position.x, Cuphead.transform.position.x, 0.3f);
        pos.y = Cuphead.transform.position.y + 3.5f;
        transform.position = pos;
    }
}
