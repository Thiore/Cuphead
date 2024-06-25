using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ready : MonoBehaviour
{
    [SerializeField] private GameObject ReadyObject;
    [SerializeField] private AudioClip End;
    private AudioSource _audio;
    private Animator Anim;
    private Animator ReadyAnim;
    private bool size = false;

    private void Start()
    {
        Anim = GetComponent<Animator>();
        ReadyAnim = ReadyObject.GetComponent<Animator>();
        _audio = ReadyObject.GetComponent<AudioSource>();
        size = false;
    }
    private void Update()
    {
        AnimatorStateInfo ReadyAnimInfo = ReadyAnim.GetCurrentAnimatorStateInfo(0);
        if(ReadyAnimInfo.IsName("Ready"))
        {
            if(ReadyAnimInfo.normalizedTime>=1f)
            {
                Anim.SetTrigger("Fade");
                ReadyAnim.SetTrigger("Go");
            }
        }
        if (ReadyAnimInfo.IsName("Allow"))
        {
            if (ReadyAnimInfo.normalizedTime >= 1f)
            {
                Game_Manager.Instance.isStartGame = true;
            }
        }
        if(Game_Manager.Instance.isFinish)
        {
            
            ReadyAnim.SetTrigger("Go");
           
            if (ReadyAnimInfo.IsName("Bravo"))
            {
                ReadyObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
                if (!size)
                {
                    _audio.clip = End;
                    _audio.Play();
                    ReadyObject.transform.localScale *= 0.42f;
                    size = true;
                }
                if (ReadyAnimInfo.normalizedTime >= 1f)
                {

                    AnimatorStateInfo FadeInfo = Anim.GetCurrentAnimatorStateInfo(0);
                    Anim.SetTrigger("Fade");
                    if(FadeInfo.IsName("FadeOut"))
                    {
                        if(FadeInfo.normalizedTime>=1f)
                        {
                            Game_Manager.Instance.isFinish = false;
                            Scene_Manager.Instance.SetScene(eScene.Menu);
                        }
                    }
                    
                }
            }
        }
        



    }
    


}
