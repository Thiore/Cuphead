using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchWall : MonoBehaviour
{
    private BoxCollider2D boxCol;
    
    private PlayerControl playerControl;
    private Rigidbody2D PlayerRigid;

    private void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
        PlayerRigid = GetComponentInParent<Rigidbody2D>();
        playerControl = GetComponentInParent<PlayerControl>();
    }

    private void LateUpdate()
    {
        //Vector3 MyPosition = transform.position;
        //if (playerControl.ISDir)
        //{
        //    //Vector2 leftRayOrigin = new Vector2(MyPosition.x - boxCol.bounds.size.x * 0.5f, MyPosition.y);
        //    RaycastHit2D hit = Physics2D.Raycast(MyPosition, Vector2.left, boxCol.bounds.size.x * 0.5f);

        //    if (hit.collider != null)
        //    {
        //        Debug.Log(boxCol.bounds.size.x * 0.5f);
        //        Debug.Log(hit.collider.name);
        //        MyPosition.x = hit.collider.bounds.center.x - (hit.collider.bounds.size.x * 0.5f) + (boxCol.bounds.size.x * 0.5f);

        //        transform.position = MyPosition;

        //    }
        //}
        //else
        //{

        //    //Vector2 RightRayOrigin = new Vector2(MyPosition.x + boxCol.bounds.size.x * 0.5f, MyPosition.y);
        //    RaycastHit2D hit = Physics2D.Raycast(MyPosition, Vector2.right, boxCol.bounds.size.x * 0.5f);
        //    if (hit.collider != null)
        //    {
        //        Debug.Log(playerControl.ISDir);
        //        Debug.Log(hit.collider.name);
        //        MyPosition.x = hit.collider.bounds.center.x + (hit.collider.bounds.size.x * 0.5f) - (boxCol.bounds.size.x * 0.5f);



        //        transform.position = MyPosition;

        //    }

        //}
    }
}
