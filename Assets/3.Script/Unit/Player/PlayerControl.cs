using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : Unit
{
    [SerializeField] private Key_Data keyData;

    float x;

    BoxCollider2D playerCol;
    private void Start()
    {
        move = GetComponent<Movement2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Anim = GetComponent<Animator>();
        Rigid = GetComponent<Rigidbody2D>();

        Jump_Force = 400f;
        x = 0f;
    }

    // Update is called once per frame
    void Update()
    {
       
        float y = 0f;

        Move_Horizontal();
        
        if(!Anim.GetBool("Jump"))
        {
            if (Input.GetKeyDown(keyData.JumpKey))
            {
                Jump();
            }
        }
        else
        {
            if (Input.GetKeyDown(keyData.JumpKey))
            {
                //parry
            }
            
        }
        
        
            if(Input.GetKeyDown(keyData.DashKey))
            {
                Anim.SetTrigger("Dash");
                if(Anim.GetBool("Jump"))
                {
                    Rigid.velocity = Vector2.zero;
                    Rigid.gravityScale = 0f;
                }
            }

        //if(Anim.GetCurrentAnimatorStateInfo(1))
        //{
        //    //AnimatorStateInfo stateInfo = Anim.GetCurrentAnimatorStateInfo(8);
        //}

        /*
         gpt답변
        유니티에서 2D 애니메이션의 현재 상태를 조건문에 사용하려면 Animator의 현재 상태 정보를 가져와야 합니다. 이를 위해 AnimatorStateInfo를 사용하고, 상태 이름이나 태그를 조건문에 넣을 수 있습니다.

아래는 애니메이션 상태를 조건문에 사용하는 예시입니다.

애니메이션 상태 이름을 조건문에 사용하기
csharp
코드 복사
using UnityEngine;

public class AnimationCondition : MonoBehaviour
{
private Animator animator;

void Start()
{
    animator = GetComponent<Animator>();
}

void Update()
{
    // 기본 레이어 (레이어 0)의 현재 애니메이션 상태 정보 가져오기
    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

    // 현재 상태의 이름을 가져오기
    string currentStateName = stateInfo.IsName("YourStateName") ? "YourStateName" : "";

    // 현재 애니메이션 상태를 조건문으로 확인
    if (stateInfo.IsName("YourStateName"))
    {
        Debug.Log("현재 애니메이션 상태는 YourStateName 입니다.");
        // YourStateName 상태일 때 수행할 동작
    }
    else if (stateInfo.IsName("AnotherStateName"))
    {
        Debug.Log("현재 애니메이션 상태는 AnotherStateName 입니다.");
        // AnotherStateName 상태일 때 수행할 동작
    }
}
}
애니메이션 태그를 조건문에 사용하기
태그를 사용하면 여러 상태를 그룹화하여 조건문에서 쉽게 체크할 수 있습니다.

csharp
코드 복사
using UnityEngine;

public class AnimationCondition : MonoBehaviour
{
private Animator animator;

void Start()
{
    animator = GetComponent<Animator>();
}

void Update()
{
    // 기본 레이어 (레이어 0)의 현재 애니메이션 상태 정보 가져오기
    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

    // 현재 상태의 태그를 가져오기
    if (stateInfo.IsTag("YourTag"))
    {
        Debug.Log("현재 애니메이션 태그는 YourTag 입니다.");
        // YourTag 태그가 붙은 상태일 때 수행할 동작
    }
    else if (stateInfo.IsTag("AnotherTag"))
    {
        Debug.Log("현재 애니메이션 태그는 AnotherTag 입니다.");
        // AnotherTag 태그가 붙은 상태일 때 수행할 동작
    }
}
}
상태 이름 또는 태그 사용 예시
상태 이름: 정확히 하나의 특정 애니메이션 상태를 참조하고 싶을 때 유용합니다.
태그: 여러 상태를 그룹화하여 조건문에서 쉽게 사용할 수 있습니다. 예를 들어, 
        "공격 상태"를 여러 개의 다른 애니메이션으로 정의해놓고 이를 한 번에 체크하고 싶을 때 사용합니다.
위 예시에서는 IsName과 IsTag 메서드를 사용하여 현재 상태가 특정 상태인지 또는 특정 태그가 붙어 있는지를 확인하고, 
        이에 따라 조건문 내에서 필요한 동작을 수행합니다. 이를 통해 현재 애니메이션 상태에 따라 다른 로직을 실행할 수 있습니다.

         */


        Flip(x);

        if (isJump)
            Anim.SetBool("Jump", true);

        Run();
            
        move.MoveTo(new Vector3(x, y, 0),5f);

    }

    private void LateUpdate()
    {
        DontMoveLine(0f,2f);
    }

    private void Dash()
    {

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
            isRun = true;
        }
        else if (Input.GetKey(keyData.RightKey))
        {
            x = 1f;
            isRun = true;
        }
        else
        {
            x = 0;
            isRun = false;
        }
    }

   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.transform.name);
            isJump = false;
            Anim.SetBool("Jump", false);
        
    }


}
