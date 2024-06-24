using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDust : MonoBehaviour
{
    private Animator Dust_Anim;
    private bool isAwake = false;

    public bool isAwakePrefabs => isAwake;

    private void Awake()
    {
        Dust_Anim = GetComponent<Animator>();
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        
        Dust_Anim.SetTrigger("Dust");
        isAwake = true;

    }
    private void Update()
    {
        AnimatorStateInfo StateInfo = Dust_Anim.GetCurrentAnimatorStateInfo(0);
        if (StateInfo.IsName("JumpDust"))
        {
            if (StateInfo.normalizedTime >= StateInfo.length*0.95f)
                gameObject.SetActive(false);
        }
    }
}
