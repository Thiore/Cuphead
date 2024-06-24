using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private GameObject BulletPrefabs;

    private Animator Anim;
    private Coroutine Shoot_co = null;
    private Queue<PlayerBullet> Bullets = new Queue<PlayerBullet>();
    private float lastTime;
    
    private float delayTime;
    private bool isShoot = false;
    public bool Shoot { set { isShoot = value; } }
    private void Awake()
    {
        Anim = GetComponent<Animator>();
        Initialize(15);
        //gameObject.SetActive(false);
        delayTime = 0.5f;
        lastTime = Time.time;

    }
    private void Update()
    {
        if(isShoot)
        {
            Shooting();
        }
        




    }

    private PlayerBullet CreateNewBullet()
    {
        var newBullet = Instantiate(BulletPrefabs, transform).GetComponent<PlayerBullet>();
        newBullet.transform.SetParent(null);
        newBullet.GetComponent<PlayerBullet>().Initialize(this);
        newBullet.gameObject.SetActive(false);
        return newBullet;
    }

    private void Initialize(int count)
    {
        for(int i = 0; i < count; i++)
        {
            Bullets.Enqueue(CreateNewBullet());
        }    
    }

    private IEnumerator Attack_co()
    {
        while(true)
        {
            if(Bullets.Count>0)
            {
                var bullet = Bullets.Dequeue();
                bullet.transform.SetParent(null);
                bullet.gameObject.SetActive(true);
            }
            else
            {
                var newBullet = CreateNewBullet();
                newBullet.transform.SetParent(null);
                newBullet.gameObject.SetActive(true);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
    public void StartAttack_co()
    {
        if (Shoot_co != null)
            StopCoroutine("Attack_co");
        else
        {
            Shoot_co = StartCoroutine(Attack_co());
        }
            
    }
    public void StopAttack_co()
    {
        if(Shoot_co!= null)
            StopCoroutine(Attack_co());
    }

    public void EnqueueBullet(PlayerBullet bul)
    {
        if (Bullets.Count < 15)
        {
            bul.gameObject.SetActive(false);
            bul.transform.SetParent(transform);

            Bullets.Enqueue(bul);
        }
        else
            Destroy(bul);
        
    }

    private void Shooting()
    {
        if (Time.time > lastTime + delayTime)
        {
            lastTime = Time.time;
            Anim.SetTrigger("Shoot");
            if (Bullets.Count > 0)
            {
                var bullet = Bullets.Dequeue();

                bullet.transform.localRotation = transform.localRotation;
                bullet.transform.localPosition = transform.localPosition;
                bullet.gameObject.SetActive(true);
            }
            else
            {
                var newBullet = CreateNewBullet();

                newBullet.transform.localRotation = transform.localRotation;
                newBullet.transform.localPosition = transform.localPosition;
                newBullet.gameObject.SetActive(true);
            }
        }
    }
}
