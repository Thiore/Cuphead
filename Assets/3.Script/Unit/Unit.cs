using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [SerializeField] protected Stage_Data stageData;
    [SerializeField] protected float Jump_Force;
    [SerializeField] protected float GravityScale;
    [SerializeField] protected GameObject jumpdustPrefab;


    
    protected Movement2D move;
    protected SpriteRenderer spriteRenderer;
    protected CapsuleCollider2D capCol;
    protected BoxCollider2D boxCol;
    protected Animator Anim;
    
    protected AudioSource audioSource;
    protected AudioClip audioClip;
    protected Rigidbody2D Rigid;

    
    protected Vector2 size;
    protected Vector2 MyPosition;

    protected int _CurrentHP;
    protected int _MaxHP;

    protected float x = 0;
    protected float y = 0;


    protected bool isDead = false;
    protected bool isRun = false;
    protected bool isJump = false;
    protected bool isCurrentDir;
    protected bool isLastDir = false;


    

    protected readonly int Anim_bRun = Animator.StringToHash("isRun");
    protected readonly int Anim_bJump = Animator.StringToHash("isJump");
    protected readonly int Anim_tDeath = Animator.StringToHash("Death");

    public int MaxHP => _MaxHP;

    public void TakeDamage()
    {
        _CurrentHP -= 1;
        if(_CurrentHP<=0)
            isDead = true;
    }

    protected void DontMoveScreen(float LengthX, float LengthY)
    {
        //플레이어가 화면범위 바깥으로 나가지 못하도록 설정
        transform.position =
                new Vector3(
                Mathf.Clamp(transform.position.x, stageData.LimitMin.x-LengthX, stageData.LimitMax.x+LengthX),
                Mathf.Clamp(transform.position.y, stageData.LimitMin.y-LengthY, stageData.LimitMax.y-LengthY),
                0);
    }

    protected void DontMoveWall()
    {
        MyPosition = transform.position;
         
        if(!isLastDir)
        {
            Vector2 leftRayOrigin = new Vector2(MyPosition.x - size.x * 0.5f, MyPosition.y + 0.1f);
            RaycastHit2D hit = Physics2D.Raycast(leftRayOrigin, Vector2.left, size.x * 0.5f);
            
            if (hit.collider != null)
            {
                Debug.Log(isLastDir);
                Debug.Log(hit.collider.name);
                MyPosition.x = hit.collider.bounds.center.x - (hit.collider.bounds.size.x * 0.5f) + (size.x * 0.5f);
                
               

                transform.position = MyPosition;
               
            }
        }
        else
        {

            Vector2 RightRayOrigin = new Vector2(MyPosition.x + size.x * 0.5f, MyPosition.y + 0.1f);
            RaycastHit2D hit = Physics2D.Raycast(RightRayOrigin, Vector2.right, size.x * 0.5f);
            if (hit.collider != null)
            {
                Debug.Log(isLastDir);
                Debug.Log(hit.collider.name);
                MyPosition.x = hit.collider.bounds.center.x + (hit.collider.bounds.size.x * 0.5f) - (size.x * 0.5f);



                transform.position = MyPosition;

            }
            
        }

        

        

        
        
        //Debug.DrawRay(!isLastDir ? (Vector3)leftRayOrigin : (Vector3)RightRayOrigin, (Vector3)((!isLastDir ? Vector2.left : Vector2.right) * (size.x * 0.5f)), Color.red);

        
       
    }


    protected abstract void Flip(float x);

    protected void Run()
    {
        if (!Anim.GetBool(Anim_bRun).Equals(isRun))
            Anim.SetBool(Anim_bRun, isRun);
    }
    protected void Jump()
    {
        Rigid.velocity = Vector2.zero;
        Rigid.AddForce(new Vector2(0, Jump_Force),ForceMode2D.Impulse);
        Anim.SetBool(Anim_bJump, isJump);
    }

    protected void WallofCollider()
    {
        Vector2 size = spriteRenderer.sprite.bounds.size;
        Vector2 pos = transform.position;
        boxCol.size = size;
        boxCol.transform.position = new Vector2(pos.x, pos.y + (size.y * 0.5f));
    }

    

}
