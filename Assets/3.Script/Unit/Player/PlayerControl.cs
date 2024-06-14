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
         gpt�亯
        ����Ƽ���� 2D �ִϸ��̼��� ���� ���¸� ���ǹ��� ����Ϸ��� Animator�� ���� ���� ������ �����;� �մϴ�. �̸� ���� AnimatorStateInfo�� ����ϰ�, ���� �̸��̳� �±׸� ���ǹ��� ���� �� �ֽ��ϴ�.

�Ʒ��� �ִϸ��̼� ���¸� ���ǹ��� ����ϴ� �����Դϴ�.

�ִϸ��̼� ���� �̸��� ���ǹ��� ����ϱ�
csharp
�ڵ� ����
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
    // �⺻ ���̾� (���̾� 0)�� ���� �ִϸ��̼� ���� ���� ��������
    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

    // ���� ������ �̸��� ��������
    string currentStateName = stateInfo.IsName("YourStateName") ? "YourStateName" : "";

    // ���� �ִϸ��̼� ���¸� ���ǹ����� Ȯ��
    if (stateInfo.IsName("YourStateName"))
    {
        Debug.Log("���� �ִϸ��̼� ���´� YourStateName �Դϴ�.");
        // YourStateName ������ �� ������ ����
    }
    else if (stateInfo.IsName("AnotherStateName"))
    {
        Debug.Log("���� �ִϸ��̼� ���´� AnotherStateName �Դϴ�.");
        // AnotherStateName ������ �� ������ ����
    }
}
}
�ִϸ��̼� �±׸� ���ǹ��� ����ϱ�
�±׸� ����ϸ� ���� ���¸� �׷�ȭ�Ͽ� ���ǹ����� ���� üũ�� �� �ֽ��ϴ�.

csharp
�ڵ� ����
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
    // �⺻ ���̾� (���̾� 0)�� ���� �ִϸ��̼� ���� ���� ��������
    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

    // ���� ������ �±׸� ��������
    if (stateInfo.IsTag("YourTag"))
    {
        Debug.Log("���� �ִϸ��̼� �±״� YourTag �Դϴ�.");
        // YourTag �±װ� ���� ������ �� ������ ����
    }
    else if (stateInfo.IsTag("AnotherTag"))
    {
        Debug.Log("���� �ִϸ��̼� �±״� AnotherTag �Դϴ�.");
        // AnotherTag �±װ� ���� ������ �� ������ ����
    }
}
}
���� �̸� �Ǵ� �±� ��� ����
���� �̸�: ��Ȯ�� �ϳ��� Ư�� �ִϸ��̼� ���¸� �����ϰ� ���� �� �����մϴ�.
�±�: ���� ���¸� �׷�ȭ�Ͽ� ���ǹ����� ���� ����� �� �ֽ��ϴ�. ���� ���, 
        "���� ����"�� ���� ���� �ٸ� �ִϸ��̼����� �����س��� �̸� �� ���� üũ�ϰ� ���� �� ����մϴ�.
�� ���ÿ����� IsName�� IsTag �޼��带 ����Ͽ� ���� ���°� Ư�� �������� �Ǵ� Ư�� �±װ� �پ� �ִ����� Ȯ���ϰ�, 
        �̿� ���� ���ǹ� ������ �ʿ��� ������ �����մϴ�. �̸� ���� ���� �ִϸ��̼� ���¿� ���� �ٸ� ������ ������ �� �ֽ��ϴ�.

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
