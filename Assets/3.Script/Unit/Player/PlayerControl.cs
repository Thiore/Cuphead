using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : Unit
{
    [SerializeField] private Key_Data keyData;
    [SerializeField] private float DashSpeed;

    private Vector2 LastVelocity;

    private float x;

    private bool isJumpDash = false;
    private float Speed = 5f;

    //private float DashDistance = 12f;
    
    

    BoxCollider2D playerCol;
    private void Start()
    {
        move = GetComponent<Movement2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Anim = GetComponent<Animator>();
        Rigid = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<BoxCollider2D>();

        x = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        Run();
        
        
        float y = 0f;

        Move_Horizontal();
        
        if(!Anim.GetBool("Jump")&& Rigid.simulated)
        {

            if (Input.GetKeyDown(keyData.JumpKey))
            {
                Jump();
            }
            
            
        }
        else
        {
            if (Input.GetMouseButtonUp(0) && Rigid.velocity.y > 0)
            {
                Rigid.velocity = Rigid.velocity * 0.5f;
            }

            if (Input.GetKeyDown(keyData.JumpKey))
            {
                //parry
            }

            

        }

        
        if (Input.GetKeyDown(keyData.DashKey)&& !isJumpDash)
        {
            isJumpDash = true;
            Rigid.simulated =false;
            if (Anim.GetBool("Jump"))
            {
                
                Anim.SetTrigger("Dash");
                //Rigid.gravityScale = 0f;
                StartCoroutine("Dash_Jump");
            }
            else
            {
                Anim.SetTrigger("Dash");
                StartCoroutine("Dash");
            }    
        }



        Flip(x);

            move.MoveTo(new Vector3(x, y, 0), Speed);
            

    }

    private void LateUpdate()
    {
        DontMoveScreen(0f,2f);
    }

    private void Move_Horizontal()
    {
        if (Input.GetKey(keyData.LeftKey) && Input.GetKey(keyData.RightKey))
        {
            if (Rigid.simulated)
                x = 0;
            isRun = false;
        }
        else if (Input.GetKey(keyData.LeftKey))
        {
            if(Rigid.simulated)
                x = -1f;
            isRun = true;
        }
        else if (Input.GetKey(keyData.RightKey))
        {
            if (Rigid.simulated)
                x = 1f;
            isRun = true;
        }
        else
        {
            if (Rigid.simulated)
                x = 0;
            
            
            isRun = false;
        }
    }

   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.transform.name);
            Anim.SetBool("Jump", false);
        if (isJumpDash)
            isJumpDash = false;
        
    }

    private IEnumerator Dash_Jump()
    {



        if (spriteRenderer.flipX)
        {
            x = -1;
        }
        else
        {
            x = 1;
        }
        Speed = 10f;

        //AnimatorStateInfo dashStateInfo = Anim.GetCurrentAnimatorStateInfo(0);

        //float dashDuration = dashStateInfo.length;
        // Debug.Log(dashDuration);

        yield return new WaitForSeconds(0.333333f);
        Rigid.simulated = true;
        Rigid.velocity = Vector2.zero;
        Speed = 5f;
        yield break;
    }

    private IEnumerator Dash()
    {
        if (spriteRenderer.flipX)
        {
            x = -1;
        }
        else
        {
            x = 1;
        }
        Speed = 10f;

        yield return new WaitForSeconds(0.3333333f);
        Rigid.simulated = true;
        isJumpDash = false;
        Speed = 5f;
        yield break;
    }

}
