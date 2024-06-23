using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [SerializeField] protected Stage_Data stageData;
    [SerializeField] protected float Jump_Force;
    [SerializeField] protected float GravityScale;
    [SerializeField] protected GameObject jumpdustPrefab;

    [SerializeField] private LayerMask Obstacle;


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
    private float RayDistance;



    protected bool isDead = false;
    protected bool isRun = false;
    protected bool isJump = false;
    protected bool isCurrentDir;
    protected bool isLastDir = false;

    protected readonly int Anim_bRun = Animator.StringToHash("isRun");
    protected readonly int Anim_bJump = Animator.StringToHash("isJump");
    protected readonly int Anim_tDeath = Animator.StringToHash("Death");

    protected readonly float safetyFactor = 0.7f;

    public int MaxHP => _MaxHP;

    public void TakeDamage()
    {
        _CurrentHP -= 1;
        if(_CurrentHP<=0)
            isDead = true;
    }

    

    
    protected abstract void Flip(float x);

    protected void Run()
    {
        if (!Anim.GetBool(Anim_bRun).Equals(isRun))
            Anim.SetBool(Anim_bRun, isRun);
    }
    protected virtual void Jump()
    {
        Rigid.velocity = Vector2.zero;
        Rigid.AddForce(new Vector2(0, Jump_Force),ForceMode2D.Impulse);
        Anim.SetBool(Anim_bJump, isJump);
    }

    protected void WallofCollider()
    {
        Vector2 size = spriteRenderer.sprite.bounds.size;
        Vector2 pos = transform.position;
        if(size.x>size.y)
        {
            capCol.direction = CapsuleDirection2D.Horizontal;
        }
        else
        {
            capCol.direction = CapsuleDirection2D.Vertical;
        }
        capCol.size = size;
        capCol.transform.position = new Vector2(pos.x, pos.y + (size.y * 0.5f));
        //Debug.Log("spritesize"+ size);
    }
    protected void ObstacleofRaycast()
    {
        RayDistance = spriteRenderer.sprite.bounds.size.x * safetyFactor;
        //Debug.Log("Ray"+RayDistance);

        Vector3 posi = transform.position;
        posi.y = posi.y + 0.5f;
        RaycastHit2D hitLeft = Physics2D.Raycast(posi, Vector2.left, RayDistance, Obstacle);
        RaycastHit2D hitRight = Physics2D.Raycast(posi, Vector2.right, RayDistance, Obstacle);
        //Debug.Log("Player" + transform.position + " 콜라이더" + boxCol.transform.position);
        float minX = float.MinValue;
        float maxX = float.MaxValue;

        

        if (hitLeft.collider != null)
        {
            minX = hitLeft.point.x + RayDistance*0.7f;
            //Debug.Log(minX);
        }
        
        if(hitRight.collider != null)
        {
            maxX = hitRight.point.x - RayDistance*0.7f;
            //Debug.Log(maxX);
        }
        //Debug.Log(minX +" " + maxX);
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);

        transform.position = new Vector3(clampedX, transform.position.y, transform.position.z);
    }

    protected void DontMoveScreen(float LengthX, float LengthY)
    {
        //플레이어가 화면범위 바깥으로 나가지 못하도록 설정
        transform.position =
                new Vector3(
                Mathf.Clamp(transform.position.x, stageData.LimitMin.x - LengthX, stageData.LimitMax.x + LengthX),
                Mathf.Clamp(transform.position.y, stageData.LimitMin.y - LengthY, stageData.LimitMax.y - LengthY),
                0);
    }

    private void OnDrawGizmos()
    {
        Vector3 posi = transform.position;
        posi.y = posi.y + 0.3f;
        Debug.DrawRay(posi, Vector2.right * RayDistance, Color.red);
        Debug.DrawRay(posi, Vector2.left* RayDistance, Color.red);
    }

}
