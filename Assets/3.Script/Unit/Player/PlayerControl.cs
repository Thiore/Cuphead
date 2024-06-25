using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : Unit
{
    [SerializeField] private Key_Data keyData;
    [SerializeField] private float DashSpeed;
    [SerializeField] private GameObject WallCollider;
    [SerializeField] private GameObject Weapon;
    [SerializeField] private GameObject dustPrefabs_1;
    [SerializeField] private GameObject dustPrefabs_2;
    [SerializeField] private AudioClip DashClip;
    [SerializeField] private AudioClip JumpClip;
    [SerializeField] private AudioClip LandClip;
    [SerializeField] private AudioClip[] WalkClip;
    private AudioSource _audio;


    private GameObject[] DustPrefabs = new GameObject[4];
    private GameObject ClearPlatformObject;
    private PlayerWeapon playerWeapon;
    private SpriteRenderer weaponSprite;
    //private Vector2 LastVelocity;

    
    private float Speed = 5f;

    private bool iswalk = false;
    private bool isAnimTurn = false;
    private bool isZeroDuration = false;
    private bool isAim = false;
    private bool isUp = false; // Aim중 up이 true라면 아래키가 눌리지 않도록 하기 위해 사용
    private bool isDown = false; // Aim중 down이 true라면 윗키가 눌리지 않도록하기 위해 사용
    private bool isLeft = false;
    private bool isRight = false;
    private bool isRunDiagonal = false;
    private bool isParry = false;
    private bool isCountParry = false;

    
   
    private bool isDuck = false;
    private bool isAttack = false;
    private bool isDash = false;
    private bool isDashGround = false;//땅에서 대쉬했을때 collider충돌했다고 대쉬가 꺼지는 현상 방지
    //private bool isGrounded = false;
    private bool isClearPlatform = false;

    private bool[] isAimDir = { false, false, false };
    
    private float AnimDuration;
    private float WalkLastTime = 0f;
    private float WalkDelayTime = 0.1f;

    private int AimDir = 0;
    private int StraightAim = 1;
    private int UpAim = 2;
    private int DownAim = 4;


    private readonly int Anim_iAim = Animator.StringToHash("Aim");
    private readonly int Anim_bParry = Animator.StringToHash("isParry");
    private readonly int Anim_tTurn = Animator.StringToHash("Turn");
    private readonly int Anim_bDuck = Animator.StringToHash("isDuck");
    private readonly int Anim_bRunDiagonalOn = Animator.StringToHash("isRunDiagonalOn");
    readonly int Anim_bAttack = Animator.StringToHash("isAttack");
    private readonly int Anim_tDash = Animator.StringToHash("Dash");



    private Coroutine dashCorutine = null;
    private Coroutine parryCorutine = null;

    private AnimatorStateInfo StateInfo;

    


    public bool ClearPlatform { set { isClearPlatform = value; } }

    BoxCollider2D playerCol;
    private void Start()
    {
        move = GetComponent<Movement2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //boxCol = WallCollider.GetComponent<BoxCollider2D>();
        capCol = WallCollider.GetComponent<CapsuleCollider2D>();
        Anim = GetComponent<Animator>();
        Rigid = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<BoxCollider2D>();
        weaponSprite = Weapon.GetComponent<SpriteRenderer>();
        playerWeapon = Weapon.GetComponent<PlayerWeapon>();
        _audio = GetComponent<AudioSource>();
        isCurrentDir = isLastDir;


        DustPrefabs[0] = Instantiate(dustPrefabs_1, transform.position, Quaternion.identity);
        
        DustPrefabs[1] = Instantiate(dustPrefabs_2, transform.position, Quaternion.identity);
        
        DustPrefabs[2] = Instantiate(dustPrefabs_1, transform.position, Quaternion.identity);
        
        DustPrefabs[3] = Instantiate(dustPrefabs_2, transform.position, Quaternion.identity);
        
    }

    private void FixedUpdate()
    {
        if(Game_Manager.Instance.isStartGame)
        if (!isDash)
            Move_Horizontal();
    }

    private void Update()
    {
        weaponSprite.flipX = spriteRenderer.flipX;
        //MyPosition = transform.position;
        StateInfo = Anim.GetCurrentAnimatorStateInfo(0);

        Reset();

        
            Run();

        if(!isDuck&&!isJump)
            Aim();

        if(!isAim)
            Duck();

        Jump();

        
        

        if (Input.GetKeyDown(keyData.ShootKey)&&!isDash)
        {
            isAttack = true;
            Anim.SetBool(Anim_bAttack, isAttack);
        }
        
        if (Input.GetKeyUp(keyData.ShootKey))
        {
            isAttack = false;
            Anim.SetBool(Anim_bAttack, isAttack);
            playerWeapon.Shoot = false;
            Weapon.transform.rotation = Quaternion.identity;
            Weapon.transform.position = capCol.transform.position;
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
        if(!isAnimTurn)
            Dash();

        if (!isCurrentDir.Equals(isLastDir) && !isAnimTurn)
            Flip(x);

        if (isAnimTurn)
        {
            Anim_Turn();
        }

        if (isAttack)
        {
            
            if (StateInfo.IsName("Jump"))
            {
                
                JumpShoot();
            }
            else
            {
                SetShootDir();
            }
               
                //WeaponAnim.SetBool("Shoot", true);
           
        }
        

        isLastDir = isCurrentDir;
        WallofCollider();
        if((StateInfo.IsName("Run")|| StateInfo.IsName("RunShooting_Straight") || StateInfo.IsName("RunShooting_DiagonalUp"))
        &&Time.time>WalkLastTime+WalkDelayTime)
        {
        WalkLastTime = Time.time;
            if(!_audio.isPlaying)
            {
                int walkcount;
                iswalk = !iswalk;
                if (iswalk)
                    walkcount = 0;
                else
                    walkcount = 1;
                _audio.clip = WalkClip[walkcount];
                _audio.Play();
            }

        }
        else
        {
            if(_audio.clip == WalkClip[0]|| _audio.clip == WalkClip[1])
             _audio.Stop();
        }


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

            isRun = false;
            if (!isAim && !isDuck && !isJump)
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

            isRun = false;
            if (!isAim && !isDuck && !isJump)
                isRun = true;
        }
        else
        {
            x = 0;

            isRun = false;
        }
    }


    protected override void Jump()
    {
        if (!isDash)
        {
            if (!isJump)
            {
                if (Input.GetKey(keyData.DownKey))
                {
                    if (Input.GetKeyDown(keyData.JumpKey))
                    {
                        Anim.SetInteger(Anim_iAim, 0);
                        if (isClearPlatform)
                        {
                            BoxCollider2D ClearBox = ClearPlatformObject.GetComponent<BoxCollider2D>();
                            float PlayerX = spriteRenderer.sprite.bounds.size.x;
                            float ClearBoxX = ClearBox.size.x * 0.5f;
                            if (transform.position.x - PlayerX > ClearBox.transform.position.x - ClearBoxX
                                && transform.position.x + PlayerX < ClearBox.transform.position.x + ClearBoxX)
                            {
                                ClearPlatformObject.GetComponent<ClearPlatform>().CheckTrigger = true;
                                isJump = true;
                                Anim.SetBool(Anim_bJump, isJump);
                            }

                        }
                        else
                        {
                            _audio.Stop();
                            _audio.clip = JumpClip;
                            _audio.Play();
                            isJump = true;
                            base.Jump();
                        }
                    }
                }
                else
                {
                    if (Input.GetKeyDown(keyData.JumpKey))
                    {
                        Anim.SetInteger(Anim_iAim, 0);
                        isJump = true;
                        base.Jump();
                    }
                }
            }
            else
            {
                if(StateInfo.IsName("Jump"))
                {
                    if (Input.GetKeyUp(keyData.JumpKey) && Rigid.velocity.y > 0)
                    {
                        Rigid.velocity = Rigid.velocity * 0.5f;
                    }

                    

                    if (Input.GetKeyDown(keyData.JumpKey)&&!isCountParry)
                    {
                        isParry = true;
                        isCountParry = true;
                        Anim.SetBool(Anim_bParry, isParry);
                        StartCoroutine(Parry_co());
                    }
                }
            }
        }
    }

    private IEnumerator Parry_co()
    {
        
        while (!isZeroDuration)
        {
            if (StateInfo.IsName("Parry"))
            {
                isZeroDuration = true;
                AnimDuration = StateInfo.length;

            }
            yield return null;
        }

        isZeroDuration = false;
        yield return new WaitForSeconds(AnimDuration - (AnimDuration * 0.1f));
        isParry = false;
        parryCorutine = null;
        Anim.SetBool(Anim_bParry, isParry);
        yield break;
    }


    #region 대쉬
    private void Dash()
    {
        if (dashCorutine == null && isDashGround)
        {
            if (Input.GetKeyDown(keyData.DashKey))
            {
                if(StateInfo.IsName("Parry"))
                {
                    isZeroDuration = false;
                    isParry = false;
                    Anim.SetBool(Anim_bParry, isParry);
                    isCountParry = false;
                    StopCoroutine(Parry_co());
                    parryCorutine = null;
                }
                isDash = true;
                isAttack = false;
                //playerWeapon.StopAttack_co();
                Anim.SetBool(Anim_bAttack, isAttack);
                //Rigid.simulated = false;
                isLastDir = isCurrentDir;
                _audio.Stop();
                _audio.clip = DashClip;
                _audio.Play();
                if (Anim.GetBool(Anim_bJump))
                {
                    Anim.SetTrigger(Anim_tDash);
                    dashCorutine = StartCoroutine(Dash_Jump_co());
                }
                else
                {
                    Anim.SetTrigger(Anim_tDash);
                    dashCorutine = StartCoroutine(Dash_Ground_co());
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
    private IEnumerator Dash_Jump_co()
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
        _audio.Stop();
        dashCorutine = null;
        yield break;
    }

    private IEnumerator Dash_Ground_co()
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
        _audio.Stop();
        isZeroDuration = false;
        dashCorutine = null;
        yield break;
    }
    #endregion


    private void Duck()
    {
        if (Input.GetKey(keyData.DownKey))
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
        if (Input.GetKey(keyData.AimKey))
        {
            isAim = true;
            
        }
        if (Input.GetKeyUp(keyData.AimKey))
        {
            AimDir = 0;
            isAim = false;
            
        }
        if (isAim)
        {
            SetAim();
            
            x = 0;
            
        }
            Anim.SetInteger(Anim_iAim, AimDir);
    }
    private void SetAim()
    {
        if ((Input.GetKey(keyData.RightKey))&&!isLeft)
        {
            isAimDir[0] = true;
            isRight = true;
        }
        if (Input.GetKeyUp(keyData.RightKey) && !isLeft && isRight)
        {
            isAimDir[0] = false;
            isRight = false;
            
        }

        if (Input.GetKey(keyData.LeftKey) && !isRight)
        {
            isAimDir[0] = true;
            isLeft = true;
        }
        if (Input.GetKeyUp(keyData.LeftKey) && !isRight && isLeft)
        {
            isAimDir[0] = false;
            isLeft = false;
            
        }

        if (Input.GetKey(keyData.UpKey)&&!isDown)
        {
            isAimDir[1] = true;
            isUp = true;
            
        }
        if(Input.GetKeyUp(keyData.UpKey)&& !isDown&&isUp)
        {
            isAimDir[1] = false;
            isUp = false;
           
        }

        if (Input.GetKey(keyData.DownKey) && !isUp)
        {
            isAimDir[2] = true;
            isDown = true;
        }
        if (Input.GetKeyUp(keyData.DownKey) && !isUp&& isDown)
        {
            isAimDir[2] = false;
            isDown = false;
           
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

    private void SetShootDir()
    {
        if (StateInfo.IsName("DuckShoot")|| StateInfo.IsName("RunShooting_Straight") || StateInfo.IsName("Shooting_Straight"))
        {
            SetWeaponPos();
            if (spriteRenderer.flipX)// 왼쪽
            {
                Weapon.transform.position = capCol.transform.position + Vector3.left * capCol.size.x * 0.6f;
            }
            else
                Weapon.transform.position = capCol.transform.position + Vector3.right*capCol.size.x * 0.6f;
        }
        else if(StateInfo.IsName("RunShooting_DiagonalUp")||StateInfo.IsName("Shooting_DiagonalUp"))
        {
            SetWeaponPos();
            if (spriteRenderer.flipX)// 왼쪽
            {
                Weapon.transform.position = capCol.transform.position + Vector3.left * capCol.size.x * 0.5f + Vector3.up * capCol.size.y*0.3f;
                Weapon.transform.rotation = Quaternion.Euler(0, 0, -40f);
            }
            else
            {
                Weapon.transform.position = capCol.transform.position + Vector3.right * capCol.size.x * 0.5f + Vector3.up * capCol.size.y * 0.3f;
                Weapon.transform.rotation = Quaternion.Euler(0, 0, 40f);
                
            }
            
        }
        else if(StateInfo.IsName("Shooting_Up"))
        {
            SetWeaponPos();
           
            if (spriteRenderer.flipX)// 왼쪽
            {
                
                Weapon.transform.position = capCol.transform.position + Vector3.left * capCol.size.x * 0.2f+ Vector3.up * capCol.size.y * 0.5f;
            }
            else
            {
                Weapon.transform.position = capCol.transform.position + Vector3.right * capCol.size.x * 0.2f + Vector3.up * capCol.size.y * 0.5f;
            }
            Weapon.transform.rotation = Quaternion.Euler(0, 0, 90f);
        }
        else if(StateInfo.IsName("Shooting_Down"))
        {
            SetWeaponPos();
            
            if (spriteRenderer.flipX)// 왼쪽
            {
                Weapon.transform.position = capCol.transform.position + Vector3.left * capCol.size.x * 0.2f + Vector3.down * capCol.size.y * 0.5f;
            }
            else
            {
                Weapon.transform.position = capCol.transform.position + Vector3.right * capCol.size.x * 0.2f + Vector3.down * capCol.size.y * 0.5f;
            }
            Weapon.transform.rotation = Quaternion.Euler(0, 0, -90f);
        }
        else if(StateInfo.IsName("Shooting_DiagonalDown"))
        {
            SetWeaponPos();
            if (spriteRenderer.flipX)// 왼쪽
            {
                Weapon.transform.position = capCol.transform.position + Vector3.left * capCol.size.x * 0.55f + Vector3.down * capCol.size.y * 0.3f;
                Weapon.transform.rotation = Quaternion.Euler(0, 0, 50f);
            }
            else
            {
                Weapon.transform.position = capCol.transform.position + Vector3.right * capCol.size.x * 0.55f + Vector3.down * capCol.size.y * 0.3f;
                Weapon.transform.rotation = Quaternion.Euler(0, 0, -50f);

            }
        }
        else
        {
            playerWeapon.Shoot = false;
            Weapon.transform.rotation = Quaternion.identity;
            Weapon.transform.position = capCol.transform.position;
        }
          
        
    }

    private void SetWeaponPos()
    {

        playerWeapon.Shoot = true;
       
        //Weapon.transform.rotation = Quaternion.identity;
        //Weapon.transform.position = capCol.transform.position;
       
        

        
    }

    private void JumpShoot()
    {
        SetWeaponPos();


        if ((Input.GetKey(keyData.RightKey)) && !isLeft)
        {
            isAimDir[0] = true;
            isRight = true;
        }
        if (Input.GetKeyUp(keyData.RightKey) && !isLeft && isRight)
        {
            isAimDir[0] = false;
            isRight = false;
            
        }

        if (Input.GetKey(keyData.LeftKey) && !isRight)
        {
            isAimDir[0] = true;
            isLeft = true;
        }
        if (Input.GetKeyUp(keyData.LeftKey) && !isRight && isLeft)
        {
            isAimDir[0] = false;
            isLeft = false;
            
        }

        if (Input.GetKey(keyData.UpKey) && !isDown)
        {
            isAimDir[1] = true;
            isUp = true;

        }
        if (Input.GetKeyUp(keyData.UpKey) && !isDown && isUp)
        {
            isAimDir[1] = false;
            isUp = false;
           
        }

        if (Input.GetKey(keyData.DownKey) && !isUp)
        {
            isAimDir[2] = true;
            isDown = true;
            
        }
        if (Input.GetKeyUp(keyData.DownKey) && !isUp && isDown)
        {
            isAimDir[2] = false;
            isDown = false;
           
        }

        if (!isAimDir[0] && !isAimDir[1] && !isAimDir[2])
        {
            AimDir = StraightAim;
        }
        else
        {
            AimDir = 0;
            for (int i = 0; i < isAimDir.Length; i++)
            {
                if (isAimDir[i])
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

        
        switch(AimDir)
        {
            
            case 1:
                if (spriteRenderer.flipX)// 왼쪽
                {
                    Weapon.transform.position = capCol.transform.position + Vector3.left * capCol.size.x * 0.6f;
                    Weapon.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
                }
                else
                    Weapon.transform.position = capCol.transform.position + Vector3.right * capCol.size.x * 0.6f;
                break;
            case 2:
                Weapon.transform.position = capCol.transform.position + Vector3.up * capCol.size.x * 0.6f;
                Weapon.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                break;
            case 3:
                if (spriteRenderer.flipX)// 왼쪽
                {
                    Weapon.transform.position = capCol.transform.position + Vector3.left * capCol.size.x * 0.4f+ Vector3.up *capCol.size.y*0.4f;
                    Weapon.transform.rotation = Quaternion.Euler(0f, 0f, -45f);
                }
                else
                {
                    Weapon.transform.position = capCol.transform.position + Vector3.right * capCol.size.x * 0.4f + Vector3.up * capCol.size.y * 0.4f;
                    Weapon.transform.rotation = Quaternion.Euler(0f, 0f, 45f);
                }
                break;
            case 4:
                Weapon.transform.position = capCol.transform.position + Vector3.down * capCol.size.x * 0.6f;
                Weapon.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
                break;
            case 5:
                if (spriteRenderer.flipX)// 왼쪽
                {
                    Weapon.transform.position = capCol.transform.position + Vector3.left * capCol.size.x * 0.4f + Vector3.down * capCol.size.y * 0.4f;
                    Weapon.transform.rotation = Quaternion.Euler(0f, 0f, 45f);
                }
                else
                {
                    Weapon.transform.position = capCol.transform.position+Vector3.right * capCol.size.x * 0.4f + Vector3.down * capCol.size.y * 0.4f;
                    Weapon.transform.rotation = Quaternion.Euler(0f, 0f, -45f);
                }
                break;
        }
        

    }

    private void Reset()
    {
        Array.Clear(isAimDir, 0, isAimDir.Length);
        isRight = false;
        isLeft = false;
        isUp = false;
        isDown = false;
        AimDir = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.contacts[0].normal.y);
        if (Mathf.Abs(collision.contacts[0].normal.y - 1f) < 0.1f)
        {
            _audio.clip = LandClip;
            _audio.Play();
            isJump = false;
            Anim.SetBool(Anim_bJump, isJump);
            isDashGround = true;
            isCountParry = false;


            Weapon.transform.localRotation = Quaternion.identity;
            Weapon.transform.position = capCol.transform.position;
            playerWeapon.Shoot = false;

            if(DustPrefabs[0].activeSelf)
            {
                DustPrefabs[2].SetActive(true);
                DustPrefabs[2].transform.position = transform.position;
                DustPrefabs[3].SetActive(true);
                DustPrefabs[3].transform.position = transform.position;
                Debug.Log("2,3");
            }
            else
            {
                DustPrefabs[0].SetActive(true);
                DustPrefabs[0].transform.position = transform.position;
                DustPrefabs[1].SetActive(true);
                DustPrefabs[1].transform.position = transform.position;
                Debug.Log("1,2");
            }
            
            
           
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Parry")) && isParry)
        {
            if(collision.gameObject.GetComponent<CircleCollider2D>().isTrigger)
            {
                collision.gameObject.GetComponent<ParrySphere>().isParry = false;
                Rigid.velocity = Vector2.zero;
                Rigid.AddForce(new Vector2(0, Jump_Force * 0.8f), ForceMode2D.Impulse);
                if (parryCorutine != null)
                {
                    isZeroDuration = false;
                    isParry = false;
                    isCountParry = false;
                    StopCoroutine(Parry_co());
                    parryCorutine = null;
                    Anim.SetBool(Anim_bParry, isParry);
                }
            }

           
        }
        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Interaction")))
        {
            if(!Game_Manager.Instance.isFinish)
            {
                if (Input.GetKey(keyData.ShootKey))
                {
                    Game_Manager.Instance.isStartGame = false;
                    Game_Manager.Instance.isFinish = true;

                }
            }
            
        }
    }
}
