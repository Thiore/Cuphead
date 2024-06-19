using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : Unit
{
    [SerializeField] private Key_Data keyData;
    [SerializeField] private float DashSpeed;
    [SerializeField] private GameObject WallCollider;

    //private Vector2 LastVelocity;

    
    private float Speed = 5f;
 
    
    private bool isAnimTurn = false;
    private bool isZeroDuration = false;
    private bool isAim = false;
    //private bool isUp = false; // Aim중 up이 true라면 아래키가 눌리지 않도록 하기 위해 사용
    //private bool isDown = false; // Aim중 down이 true라면 윗키가 눌리지 않도록하기 위해 사용
   
    private bool isDuck = false;
    private bool isAttack = false;
    private bool isDash = false;
    private bool isDashGround = false;//땅에서 대쉬했을때 collider충돌했다고 대쉬가 꺼지는 현상 방지

    private float AnimDuration;

    //private int AimDir = 0;

    readonly int Anim_iAim = Animator.StringToHash("Aim"); 
    //readonly int Anim_tParry = Animator.StringToHash("Parry");
    readonly int Anim_tTurn = Animator.StringToHash("Turn");
    readonly int Anim_bDuck = Animator.StringToHash("isDuck");
    //readonly int Anim_bRunDiagonalOn = Animator.StringToHash("isRunDiagonalOn");
    readonly int Anim_bAttack = Animator.StringToHash("isAttack");
    readonly int Anim_tDash = Animator.StringToHash("Dash");

    private Coroutine dashCorutin = null;

    private AnimatorStateInfo StateInfo;

    public bool ISDir=>isLastDir;

    BoxCollider2D playerCol;
    private void Start()
    {
        move = GetComponent<Movement2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCol = WallCollider.GetComponent<BoxCollider2D>();
        capCol = GetComponent<CapsuleCollider2D>();
        Anim = GetComponent<Animator>();
        Rigid = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<BoxCollider2D>();

        isCurrentDir = isLastDir;
    }

    private void FixedUpdate()
    {
        if (!isDash)
            Move_Horizontal();
    }

    private void Update()
    {
        
        //MyPosition = transform.position;
        StateInfo = Anim.GetCurrentAnimatorStateInfo(0);
        
        
        Run();

       

        Duck();

        if (isAnimTurn)
        {
            Anim_Turn();
        }

        if(!isDash)
        {
            if (!isJump)
            {
                if (Input.GetKeyDown(keyData.JumpKey))
                {
                    isJump = true;
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

        if (!isCurrentDir.Equals(isLastDir) && !isAnimTurn)
            Flip(x);


        

        isLastDir = isCurrentDir;
        WallofCollider();
        //x = 0;
        move.MoveTo(new Vector3(x, y, 0), Speed);
    }

    private void LateUpdate()
    {
        
        //DontMoveScreen(0f,2f);
        //WallofCollider();
        //DontMoveWall();
    }

    private void Move_Horizontal()
    {
        if (Input.GetKey(keyData.LeftKey) && Input.GetKey(keyData.RightKey))
        {
            x = 0;
            isRun = false;
            
        }
        else if (Input.GetKey(keyData.LeftKey))
        {
            
            x = -1f;
            isCurrentDir = true;
            
            isRun = true;
        }
        else if (Input.GetKey(keyData.RightKey))
        {
            
            x = 1f;
               
            isCurrentDir = false;

            isRun = true;
        }
        else
        {
            x = 0;

            isRun = false;
        }
    }

   

    
    #region 대쉬
    private void Dash()
    {
        if (dashCorutin == null && isDashGround)
        {
            if (Input.GetKeyDown(keyData.DashKey))
            {
                isDash = true;

                //Rigid.simulated = false;
                isLastDir = isCurrentDir;

                if (Anim.GetBool(Anim_bJump))
                {
                    Anim.SetTrigger(Anim_tDash);
                    dashCorutin = StartCoroutine("Dash_Jump");
                }
                else
                {
                    Anim.SetTrigger(Anim_tDash);
                    dashCorutin = StartCoroutine("Dash_Ground");
                }

                Speed = 10f;
            }
        }
        if(isDash)
        {
            isCurrentDir = isLastDir;
            if (isCurrentDir)
            {
                x = -1;
            }
            else
            {
                x = 1;
            }
        }

    }
    private IEnumerator Dash_Jump()
    {
        Rigid.gravityScale = 0f;
        Rigid.velocity = Vector2.zero;
        isDashGround = false;
        while(!isZeroDuration)
        {
            if (StateInfo.IsName("Dash_Air"))
            {
                isZeroDuration = true;
                AnimDuration = StateInfo.length;
                
            }
            yield return null;
        }
        Debug.Log(AnimDuration);
        Debug.Log(AnimDuration - (AnimDuration * 0.1f));
        yield return new WaitForSeconds(AnimDuration-(AnimDuration*0.1f));
        isDash = false;
        Rigid.gravityScale = GravityScale;
        Rigid.velocity = Vector2.zero;
        Speed = 5f;
        isZeroDuration = false;
        dashCorutin = null;
        yield break;
    }

    private IEnumerator Dash_Ground()
    {
        while (!isZeroDuration)
        {
            
            if (StateInfo.IsName("Dash_Ground"))
            {
                isZeroDuration = true;
                AnimDuration = StateInfo.length;

            }
            yield return null;
        }
        Debug.Log(AnimDuration);
        Debug.Log(AnimDuration - (AnimDuration * 0.1f));
        yield return new WaitForSeconds(AnimDuration - (AnimDuration * 0.1f));
        isDash = false;
        Rigid.simulated = true;        
        Speed = 5f;
        isZeroDuration = false;
        dashCorutin = null;
        yield break;
    }
    #endregion


    private void Duck()
    {
        if (Input.GetKeyDown(keyData.DownKey))
        {
            isDuck = true;
            Anim.SetBool(Anim_bDuck, isDuck);
        }
        if (Input.GetKeyUp(keyData.DownKey))
        {
            isDuck = false;
            Anim.SetBool(Anim_bDuck, isDuck);
        }

        if(isDuck)
        {
            x = 0;
        }
        
    }

    

    protected override void Flip(float x)
    {
        if(isDuck||(Anim.GetBool("isRun")&&isAttack) || (Anim.GetBool("isRun") && isAttack))
        {
            isAnimTurn = true;
            Anim.SetTrigger(Anim_tTurn);
        }
        else
        {
            spriteRenderer.flipX = isCurrentDir;
        }
    }
    protected void Anim_Turn()
    {
        if (StateInfo.IsName("DuckTurn")|| StateInfo.IsName("RunShooting_DiagonalUp_Turn")|| StateInfo.IsName("RunShooting_Straight_Turn"))
        {
            if(StateInfo.normalizedTime>0.45f)
            {
                spriteRenderer.flipX = isCurrentDir;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJump = false;
        Anim.SetBool(Anim_bJump, isJump);
        isDashGround = true;

        Debug.Log(collision.gameObject.name);

        //CollisionX = collision.contacts[0].point.x;
        //isCollisionX = true;


    }
}
