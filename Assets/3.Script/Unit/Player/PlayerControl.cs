using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : Unit
{
    [SerializeField] private Key_Data keyData;
    [SerializeField] private float DashSpeed;
    [SerializeField] private GameObject WallCollider;
    private GameObject ClearPlatformObject;

    //private Vector2 LastVelocity;

    
    private float Speed = 5f;
 
    
    private bool isAnimTurn = false;
    private bool isZeroDuration = false;
    private bool isAim = false;
    private bool isUp = false; // Aim중 up이 true라면 아래키가 눌리지 않도록 하기 위해 사용
    private bool isDown = false; // Aim중 down이 true라면 윗키가 눌리지 않도록하기 위해 사용
    private bool isLeft = false;
    private bool isRight = false;
    private bool isRunDiagonal = false;
   
    private bool isDuck = false;
    private bool isAttack = false;
    private bool isDash = false;
    private bool isDashGround = false;//땅에서 대쉬했을때 collider충돌했다고 대쉬가 꺼지는 현상 방지
    //private bool isGrounded = false;
    private bool isClearPlatform = false;

    private bool[] isAimDir = { false, false, false };
    
    private float AnimDuration;

    private int AimDir = 0;
    private int StraightAim = 1;
    private int UpAim = 2;
    private int DownAim = 4;


    private readonly int Anim_iAim = Animator.StringToHash("Aim");
    //private readonly int Anim_tParry = Animator.StringToHash("Parry");
    private readonly int Anim_tTurn = Animator.StringToHash("Turn");
    private readonly int Anim_bDuck = Animator.StringToHash("isDuck");
    private readonly int Anim_bRunDiagonalOn = Animator.StringToHash("isRunDiagonalOn");
    readonly int Anim_bAttack = Animator.StringToHash("isAttack");
    private readonly int Anim_tDash = Animator.StringToHash("Dash");


    private readonly bool[] ClearAimDir = { false, false, false };

    private Coroutine dashCorutin = null;

    private AnimatorStateInfo StateInfo;

    

    public bool ISDir=>isLastDir;

    public bool ClearPlatform { set { isClearPlatform = value; } }

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

        if(!isDuck)
            Aim();

        if(!isAim)
            Duck();

        if (isAnimTurn)
        {
            Anim_Turn();
        }

        if(!isDash)
        {
            if (!isJump)
            {
                if (Input.GetKey(keyData.DownKey))
                {
                    if (Input.GetKeyDown(keyData.JumpKey))
                    {
                        if (isClearPlatform)
                        {
                            BoxCollider2D ClearBox = ClearPlatformObject.GetComponent<BoxCollider2D>();
                            float PlayerX = spriteRenderer.sprite.bounds.size.x;
                            float ClearBoxX = ClearBox.size.x*0.5f;
                            Debug.Log((transform.position.x));
                            Debug.Log((ClearBox.transform.position.x));
                            Debug.Log((PlayerX));
                            Debug.Log((ClearBoxX));
                            if (transform.position.x-PlayerX>ClearBox.transform.position.x-ClearBoxX
                                && transform.position.x + PlayerX < ClearBox.transform.position.x + ClearBoxX)
                            {
                                ClearPlatformObject.GetComponent<ClearPlatform>().CheckTrigger = true;
                                isJump = true;
                                Anim.SetBool(Anim_bJump, isJump);
                            }
                            
                        }
                        else
                        {
                            isJump = true;
                            Jump();
                        }
                    }
                }
                else
                {
                    if (Input.GetKeyDown(keyData.JumpKey))
                    {
                        isJump = true;
                        Jump();
                    }
                }
            }
            else
            {
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
            isAttack = true;
            Anim.SetBool(Anim_bAttack, isAttack);
            
        }
        if (Input.GetKeyUp(keyData.ShootKey))
        {
            isAttack = false;
            Anim.SetBool(Anim_bAttack, isAttack);
        }
        if (isRun && isAttack && Input.GetKey(keyData.UpKey))
        {
            isRunDiagonal = true;
            Anim.SetBool(Anim_bRunDiagonalOn, isRunDiagonal);

        }
        else
        {
            isRunDiagonal = false;
            Anim.SetBool(Anim_bRunDiagonalOn, isRunDiagonal);
        }

        Dash();

        if (!isCurrentDir.Equals(isLastDir) && !isAnimTurn)
            Flip(x);


        

        isLastDir = isCurrentDir;
        WallofCollider();
        move.MoveTo(new Vector3(x, y, 0), Speed);
    }

    private void LateUpdate()
    {

        //DontMoveScreen(0f,2f);
        ObstacleofRaycast();
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
            if (!isLastDir.Equals(isCurrentDir))
            {
                if ((StateInfo.IsName("RunShooting_DiagonalUp") || StateInfo.IsName("RunShooting_Straight")) && isAttack)
                {
                    isAnimTurn = true;
                    Anim.SetTrigger(Anim_tTurn);
                }
            }
                isRun = true;
        }
        else if (Input.GetKey(keyData.RightKey))
        {
            
            x = 1f;
               
            isCurrentDir = false;
            if (!isLastDir.Equals(isCurrentDir))
            {
                if ((StateInfo.IsName("RunShooting_DiagonalUp") || StateInfo.IsName("RunShooting_Straight")) && isAttack)
                {
                    isAnimTurn = true;
                    Anim.SetTrigger(Anim_tTurn);
                }
            }

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
        Rigid.gravityScale = 0f;
        Rigid.velocity = Vector2.zero;
        while (!isZeroDuration)
        {
            
            if (StateInfo.IsName("Dash_Ground"))
            {
                isZeroDuration = true;
                AnimDuration = StateInfo.length;

            }
            yield return null;
        }
        //Debug.Log(AnimDuration);
        //Debug.Log(AnimDuration - (AnimDuration * 0.1f));
        yield return new WaitForSeconds(AnimDuration - (AnimDuration * 0.1f));
        isDash = false;
        Rigid.gravityScale = GravityScale;
        Rigid.velocity = Vector2.zero;
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
        if((isDuck||(isDuck&&isAttack))&&!isAnimTurn)
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
    private void Aim()
    {
        if (Input.GetKeyDown(keyData.AimKey) && !isDuck && !isDash && !isJump)
        {
            isAim = true;

        }
        if (Input.GetKeyUp(keyData.AimKey))
        {
            isAimDir = ClearAimDir;
            isAim = false;
            AimDir = 0;
        }
        if (isAim)
        {
            SetAim();
            Anim.SetInteger(Anim_iAim, AimDir);
            x = 0;
        }
        else
            Anim.SetInteger(Anim_iAim, 0);
    }
    private void SetAim()
    {
        if ((Input.GetKeyDown(keyData.RightKey))&&!isLeft)
        {
            isAimDir[0] = true;
            isRight = true;
        }
        if (Input.GetKeyUp(keyData.RightKey) && isAimDir[0] && !isLeft && isRight)
        {
            isAimDir[0] = false;
            isRight = false;
            AimDir -= StraightAim;
        }

        if (Input.GetKeyDown(keyData.LeftKey) && !isRight)
        {
            isAimDir[0] = true;
            isLeft = true;
        }
        if (Input.GetKeyUp(keyData.LeftKey) && isAimDir[0] && !isRight && isLeft)
        {
            isAimDir[0] = false;
            isLeft = false;
            AimDir -= StraightAim;
        }

        if (Input.GetKeyDown(keyData.UpKey)&&!isDown)
        {
            isAimDir[1] = true;
            isUp = true;
            
        }
        if(Input.GetKeyUp(keyData.UpKey)&& isAimDir[1]&&!isDown&&isUp)
        {
            isAimDir[1] = false;
            isUp = false;
            AimDir -= UpAim;
        }

        if (Input.GetKeyDown(keyData.DownKey) && !isUp)
        {
            isAimDir[2] = true;
            isDown = true;
        }
        if (Input.GetKeyUp(keyData.DownKey) && isAimDir[2] && !isUp&& isDown)
        {
            isAimDir[2] = false;
            isDown = false;
            AimDir -= DownAim;
        }

        if (!isAimDir[0] && !isAimDir[1] && !isAimDir[2])
        {
            AimDir = StraightAim;
        }            
        else
        {
            AimDir = 0;
            for(int i = 0; i < isAimDir.Length;i++)
            {
                if(isAimDir[i])
                {
                    switch (i)
                    {
                        case 0:
                            AimDir += StraightAim;
                            break;
                        case 1:
                            AimDir += UpAim;
                            break;
                        case 2:
                            AimDir += DownAim;
                            break;
                    }
                }
                
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.contacts[0].normal.y);
        if (Mathf.Abs(collision.contacts[0].normal.y - 1f) < 0.1f)
        {
            isJump = false;
            Anim.SetBool(Anim_bJump, isJump);
            isDashGround = true;
        }

        if(collision.gameObject.layer.Equals(LayerMask.NameToLayer("ClearPlatform")))
        {
            ClearPlatformObject = collision.gameObject;
            isClearPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (Rigid.velocity.y<-1f&&!isDash && !isJump)
        {
            isJump = true;
            Anim.SetBool(Anim_bJump, isJump);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("ClearPlatform")))
        {
            isClearPlatform = false;
        }
    }
}
