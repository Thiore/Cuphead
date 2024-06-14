using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement2D : MonoBehaviour
{
    [SerializeField] private float Move_Speed = 0f;
    [SerializeField] private Vector3 moveDirection = Vector3.zero;

    private void Update()
    {
        transform.Translate(moveDirection * Move_Speed * Time.deltaTime);
    }

    public void MoveTo(Vector3 dir, float speed)
    {
        moveDirection = dir;
        Move_Speed = speed;
    }

}
