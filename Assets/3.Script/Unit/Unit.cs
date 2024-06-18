using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [SerializeField] protected Stage_Data stageData;
    [SerializeField] protected float Jump_Force;
    [SerializeField] protected float GravityScale;


    protected Movement2D move;
    protected SpriteRenderer spriteRenderer;
    protected Animator Anim;
    protected AudioSource audioSource;
    protected AudioClip audioClip;
    protected Rigidbody2D Rigid;

    protected int _CurrentHP;
    protected int _MaxHP;

    protected float x;

    protected bool isDead = false;
    protected bool isRun = false;

    protected bool isLeft = false;

    protected readonly int Anim_bRun = Animator.StringToHash("isRun");
    protected readonly int Anim_bJump = Animator.StringToHash("isJump");
    protected readonly int Anim_tDeath = Animator.StringToHash("Death");

    public int MaxHP => _MaxHP;

    public void TakeDamage()
    {
        _CurrentHP -= 1;
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

    protected abstract void Flip(float x);

    protected void Run()
    {
        if (isRun)
            Anim.SetBool(Anim_bRun,true);
        else
            Anim.SetBool(Anim_bRun, false);
    }
    protected void Jump()
    {
        Rigid.velocity = Vector2.zero;
        Rigid.AddForce(new Vector2(0, Jump_Force),ForceMode2D.Impulse);
        Anim.SetBool(Anim_bJump, true);
    }

}
