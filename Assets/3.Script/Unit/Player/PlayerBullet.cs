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
    Vector3 Dir;

    private void Awake()
    {
        //cirCol = GetComponent<CircleCollider2D>();
        Anim = GetComponent<Animator>();
        move = GetComponent<Movement2D>();
        


    }
    private void Update()
    {
        if(!Hit)
            move.MoveTo(Dir, 12f);
        Debug.Log(Dir);

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

    public void SetDir(Vector3 dir)
    {
        this.Dir = dir;
        float angle = Mathf.Atan2(Dir.y, Dir.x) * Mathf.Rad2Deg;
        
        transform.localRotation = Quaternion.identity;
        
        transform.localRotation = Quaternion.Euler(0, 0, angle);
       


    }

    public void Initialize(PlayerWeapon weapon)
    {
        this.weapon = weapon;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("왜 안맞음?");

        if (collision.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy")))
        {
            Hit = true;
            Anim.SetTrigger("Hit");
            collision.gameObject.GetComponent<Target>().StartHit();

        }
        if(collision.gameObject.layer.Equals(LayerMask.NameToLayer("Obstacle"))
            || collision.gameObject.layer.Equals(LayerMask.NameToLayer("Ground"))
            || collision.gameObject.layer.Equals(LayerMask.NameToLayer("ClearPlatform")))
        {
            Hit = true;
            Anim.SetTrigger("Hit");
        }
    }

    

}
