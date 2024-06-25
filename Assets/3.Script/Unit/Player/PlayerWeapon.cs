using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private GameObject BulletPrefabs;
    [SerializeField] private GameObject PlayerCol;
    private CapsuleCollider2D capCol;

    private Animator Anim;
    private AudioSource _audio;
    
    private Coroutine Shoot_co = null;
    private Queue<PlayerBullet> Bullets = new Queue<PlayerBullet>();
    private float lastTime;
    
    private float delayTime;
    private bool isShoot = false;
    public bool Shoot { set { isShoot = value; } }
    private void Awake()
    {
        Anim = GetComponent<Animator>();
        capCol = PlayerCol.GetComponent<CapsuleCollider2D>();
        _audio = GetComponent<AudioSource>();
        Initialize(15);
        //gameObject.SetActive(false);
        delayTime = 0.2f;
        lastTime = Time.time;

    }
    private void Update()
    {
        if (isShoot)
        {
            //Anim.SetTrigger("Shoot");
            Shooting();
            if (!_audio.isPlaying)
                _audio.Play();
        }
        else
            _audio.Stop();
        
    }

    private PlayerBullet CreateNewBullet()
    {
        
        var newBullet = Instantiate(BulletPrefabs, transform.position, Quaternion.identity).GetComponent<PlayerBullet>();
        //newBullet.transform.SetParent(null);
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
                //bullet.transform.SetParent(null);
                bullet.gameObject.SetActive(true);
            }
            else
            {
                var newBullet = CreateNewBullet();
                //newBullet.transform.SetParent(null);
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
            //bul.transform.SetParent(transform);

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
                bullet.gameObject.SetActive(true);
                bullet.transform.position = transform.position;
                Vector3 cappos = capCol.transform.position;
                cappos.y-= 0.02f;
                bullet.SetDir(transform.position - cappos);
                
            }
            else
            {
                var newBullet = CreateNewBullet();
                newBullet.gameObject.SetActive(true);
                newBullet.transform.position = transform.position;
                Vector3 cappos = capCol.transform.position;
                cappos.y -= 0.02f;
                newBullet.SetDir(transform.position - cappos);

                
            }
        }
        
    }
}
