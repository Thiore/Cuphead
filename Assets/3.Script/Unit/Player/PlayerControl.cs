using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : Unit
{
    [SerializeField] private Key_Data keyData;
    [SerializeField] private float DashSpeed;

    private Vector2 LastVelocity;

    
    private float Speed = 5f;

    private bool isJumpDash = false;
    private bool isCurrentDir;
    private bool isLastDir;
    private bool isAnimTurn = false;
    private bool isZeroDuration = false;
    private bool isCanDir = true;
    private bool isAim = false;
    private bool Up = false;
    private bool Down = false;
    private bool isDir = false;

    private float AnimDuration;

    private int AimDir = 0;

    readonly int Anim_iAim = Animator.StringToHash("Aim"); 
    readonly int Anim_tParry = Animator.StringToHash("Parry");
    readonly int Anim_tTurn = Animator.StringToHash("Turn");
    readonly int Anim_bDuck = Animator.StringToHash("isDuck");
    readonly int Anim_bRunDiagonalOn = Animator.StringToHash("isRunDiagonalOn");
    readonly int Anim_bAttack = Animator.StringToHash("isAttack");
    readonly int Anim_tDash = Animator.StringToHash("Dash");

    //private Coroutine dashCorutin = null;



    BoxCollider2D playerCol;
    private void Start()
    {
        move = GetComponent<Movement2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Anim = GetComponent<Animator>();
        Rigid = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<BoxCollider2D>();

        x = 0f;
        isZeroDuration = false;
        isLastDir = isLeft;
        isCurrentDir = isLastDir;
    }

    // Update is called once per frame
    void Update()
    {
        Run();


        float y = 0f;
        
        Duck();

        Move_Horizontal();

        if (!isCurrentDir.Equals(isLastDir) && !isAnimTurn)
            Flip(x);

        if (isAnimTurn)
        {
            Anim_Turn();
        }


        if (!Anim.GetBool(Anim_bJump) && Rigid.simulated)
        {

            if (Input.GetKeyDown(keyData.JumpKey))
            {
                isCanDir = true;
                Jump();
            }


        }
        else
        {
            Anim.SetInteger(Anim_iAim, 0);
            if (Input.GetKeyUp(keyData.JumpKey) && Rigid.velocity.y > 0)
            {
                Rigid.velocity = Rigid.velocity * 0.5f;
            }

            if (Input.GetKeyDown(keyData.JumpKey))
            {
                //parry
            }
        }

        if (Input.GetKeyDown(keyData.ShootKey))
        {
            Anim.SetBool(Anim_bAttack, true);
        }
        if (Input.GetKeyUp(keyData.ShootKey))
        {
            Anim.SetBool(Anim_bAttack, false);
        }

        if(Input.GetKeyDown(keyData.AimKey))
        {
            isAim = true;
        }
        Dash();
        isLastDir = isCurrentDir;

        if (!Anim.GetBool(Anim_bDuck)||!Anim.GetInteger(Anim_iAim).Equals(0))
        {
            
            move.MoveTo(new Vector3(x, y, 0), Speed);
        }
            
            

    }

    private void LateUpdate()
    {
        DontMoveScreen(0f,2f);
    }

    private void Move_Horizontal()
    {
        if (Input.GetKey(keyData.LeftKey) && Input.GetKey(keyData.RightKey))
        {
            x = 0;
            isRun = false;
;        }
        else if (Input.GetKey(keyData.LeftKey))
        {
            if (isCanDir)
            {
                x = -1f;
                isLeft = true;
            }
            isCurrentDir = true;
            
            isRun = true;
        }
        else if (Input.GetKey(keyData.RightKey))
        {
            if (isCanDir)
            {
                x = 1f;
                isLeft = false;
            }
            isCurrentDir = false;
            
            isDir = true;
        }
        else
        {
                x = 0;

            isRun = false;
        }
    }

   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.transform.name);
            Anim.SetBool(Anim_bJump, false);
        if (isJumpDash)
            isJumpDash = false;
        
    }
    #region 코루틴_대쉬
    private IEnumerator Dash_Jump()
    {

        if (isLeft)
        {
            x = -1;
        }
        else
        {
            x = 1;
        }
        Speed = 10f;

        while(!isZeroDuration)
        {
            AnimatorStateInfo dashStateInfo = Anim.GetCurrentAnimatorStateInfo(0);
            if (dashStateInfo.IsName("Dash_Air"))
            {
                isZeroDuration = true;
                AnimDuration = dashStateInfo.length;
                
            }
            yield return null;
        }
        
        yield return new WaitForSeconds(AnimDuration);
        isCanDir = true;
        Rigid.simulated = true;
        Rigid.velocity = Vector2.zero;
        Speed = 5f;
        isZeroDuration = false;
        yield break;
    }

    private IEnumerator Dash_Ground()
    {
        if (isLeft)
        {
            x = -1;
        }
        else
        {
            x = 1;
        }
        Speed = 10f;
        while (!isZeroDuration)
        {
            AnimatorStateInfo dashStateInfo = Anim.GetCurrentAnimatorStateInfo(0);
            if (dashStateInfo.IsName("Dash_Ground"))
            {
                isZeroDuration = true;
                AnimDuration = dashStateInfo.length;

            }
            yield return null;
        }
        yield return new WaitForSeconds(AnimDuration);
        Rigid.simulated = true;
        isCanDir = true;
        isJumpDash = false;
        Speed = 5f;
        isZeroDuration = false;
        yield break;
    }
    #endregion


    private void Duck()
    {
        if (Input.GetKeyDown(keyData.DownKey))
        {
            isCanDir = false;
            x = 0;
            Anim.SetBool(Anim_bDuck, true);
        }
        if (Input.GetKeyUp(keyData.DownKey))
        {
            isCanDir = true;
            Anim.SetBool(Anim_bDuck, false);
        }
        
    }

    private void Dash()
    {
        if (Input.GetKeyDown(keyData.DashKey) && !isJumpDash)
        {
            isJumpDash = true;
            Rigid.simulated = false;
            isCanDir = false;
            if (Anim.GetBool(Anim_bJump))
            {
                Anim.SetTrigger(Anim_tDash);
                _ = StartCoroutine("Dash_Jump");
            }
            else
            {
                Anim.SetTrigger(Anim_tDash);
                _ = StartCoroutine("Dash_Ground");
            }
        }
    }

    protected override void Flip(float x)
    {
        if(Anim.GetBool(Anim_bDuck)||(Anim.GetBool(Anim_bDuck)&&Anim.GetBool(Anim_bAttack))
            || (Anim.GetBool(Anim_bRun)&&Anim.GetBool(Anim_bAttack)))
        {
            isAnimTurn = true;
            Anim.SetTrigger(Anim_tTurn);
        }
        else
        {
            if (isLeft)
                spriteRenderer.flipX = true;
            else
                spriteRenderer.flipX = false;
        }
    }
    protected void Anim_Turn()
    {
        AnimatorStateInfo StateInfo = Anim.GetCurrentAnimatorStateInfo(0);
        if (StateInfo.IsName("DuckTurn")|| StateInfo.IsName("RunShooting_DiagonalUp_Turn")|| StateInfo.IsName("RunShooting_Straight_Turn"))
        {
            if(StateInfo.normalizedTime>0.45f)
            {
                if (isLeft)
                    spriteRenderer.flipX = true;
                else
                    spriteRenderer.flipX = false;
            }
            if(StateInfo.normalizedTime > 0.8f)
            {
                isAnimTurn = false;
            }
        }
    }

    //private void Aim()
    //{
    //    if(Input.GetKeyDown(keyData.LeftKey))
    //        if (Input.GetKeyDown(keyData.))
    //            if (Input.GetKeyDown(keyData.))
    //                if (Input.GetKeyDown(keyData.))
    //                    if (Input.GetKeyDown(keyData.))
    //                        if (Input.GetKeyDown(keyData.))
    //                            if (Input.GetKeyDown(keyData.))
    //}
}
