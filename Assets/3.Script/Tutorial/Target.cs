using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer childRenderer;
    private int Hp = 15;
    Color HitColor = new Color(103f / 255f, 131f / 255f, 255f / 255f);

    private Coroutine Hit_co = null;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        childRenderer = GetComponentInChildren<SpriteRenderer>();
        Debug.Log(childRenderer.name);
    }
    private void Update()
    {
        if (Hp<=0)
        {
            gameObject.SetActive(false);
        }
    }

    public void StartHit()
    {
        
        if(Hit_co == null)
        {
            
            Hit_co = StartCoroutine(HitTarget());
        }
        else
        {
            Debug.Log("여기맞음?");
            StopCoroutine(HitTarget());
            Hit_co = null;
            spriteRenderer.color = Color.white;
            Hit_co = StartCoroutine(HitTarget());
        }

    }

    private IEnumerator HitTarget()
    {
        Hp -= 1;
        
        spriteRenderer.color = HitColor;
        childRenderer.color = HitColor;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
        childRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = HitColor;
        childRenderer.color = HitColor;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
        childRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        Hit_co = null;

    }
}
