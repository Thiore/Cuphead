using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPlatform : MonoBehaviour
{
    [SerializeField] private GameObject Cuphead;

    private BoxCollider2D boxCol;
    private float boxColY;
    private float CupY;
    private bool isCheckTrigger = false;

    public bool CheckTrigger { set { isCheckTrigger = value; } }

    private void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {

            boxColY = boxCol.transform.position.y + boxCol.size.y * 0.4f;
            CupY = Cuphead.transform.position.y + 0.15f;
            //Debug.Log("BOX" + boxColY + " Cup" + CupY);
            if (boxColY < CupY)
            {
                if (!isCheckTrigger)
                {
                    //Cuphead.GetComponent<PlayerControl>().ClearPlatform = true;
                    boxCol.isTrigger = false;
                }
                else
                {
                    //Cuphead.GetComponent<PlayerControl>().ClearPlatform = false;
                    boxCol.isTrigger = true;
                }

            }
            else
            {
                boxCol.isTrigger = true;
                isCheckTrigger = false;
            }
                
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {

            boxColY = boxCol.transform.position.y + boxCol.size.y * 0.4f;
            CupY = Cuphead.transform.position.y + 0.15f;
            
            if (boxColY < CupY)
            {
                boxCol.isTrigger = false;
                isCheckTrigger = false;

            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Cuphead.GetComponent<PlayerControl>().ClearPlatform = false;
        boxCol.isTrigger = false;
        isCheckTrigger = false;
    }
}
