using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    // private CircleCollider2D cirCol;
    private Movement2D move;
    
    private Animator Anim;

    private PlayerWeapon weapon;
    
    
    private bool Hit = false;

    private void Awake()
    {
        //cirCol = GetComponent<CircleCollider2D>();
        Anim = GetComponent<Animator>();
        move = GetComponent<Movement2D>();



    }
    private void Update()
    {
        move.MoveTo(transform.right, 24f);
        if (Hit)
        {
            AnimatorStateInfo info = Anim.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("HitBullet"))
            {
                if (info.normalizedTime < 1)
                    return;
                else
                    weapon.EnqueueBullet(this);
            }
        }
    }

    public void Initialize(PlayerWeapon weapon)
    {
        this.weapon = weapon;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Hit = true;
        Anim.SetTrigger("Hit");

        //if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
        //{

        //}
    }

}
