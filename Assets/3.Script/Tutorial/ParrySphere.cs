using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrySphere : MonoBehaviour
{
    [SerializeField] private Sprite OffParry;

    [SerializeField] private Sprite OnParry;

    [SerializeField] private float DelayTime;

    [SerializeField] private bool isParryOn;

    private SpriteRenderer spriteRenderer;

    private bool isLastParry;

    public bool isParry
    {
        get
        {
            return isParryOn;
        }
        set
        {
            isParryOn = value;
        }
    }
    
    private float LastTime;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        LastTime = Time.time;
        isLastParry = isParryOn;
    }

    private void Update()
    {
        if(Time.time > LastTime+DelayTime)
        {
            isParryOn = !isParryOn;
        }

        if(!isLastParry.Equals(isParryOn))
        {
            LastTime = Time.time;
            isLastParry = isParryOn;
            if (isParryOn)
            {
                spriteRenderer.sprite = OnParry;
            }
            else
                spriteRenderer.sprite = OffParry;
        }

        
    }



}
