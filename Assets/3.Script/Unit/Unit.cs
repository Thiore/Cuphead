using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
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

    


    protected bool isDead = false;
    protected bool isRun = false;

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

    protected void Flip(float x)
    {
        if (x < 0)
            spriteRenderer.flipX = true;
        if (x > 0)
            spriteRenderer.flipX = false;
    }

    protected void Run()
    {
        if (isRun)
            Anim.SetBool("Run", true);
        else
            Anim.SetBool("Run", false);
    }
    protected void Jump()
    {
        Rigid.velocity = Vector2.zero;
        Rigid.AddForce(new Vector2(0, Jump_Force),ForceMode2D.Impulse);
        Anim.SetBool("Jump", true);
    }

}
