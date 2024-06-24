using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer childRenderer;
    private int Hp = 5;
    Color HitColor = new Color(103f, 131f, 255f);
    
    private Coroutine Hit_co = null;

    private void Start()
    {
        TryGetComponent(out spriteRenderer);
    }
    private void Update()
    {
        //Debug.Log(HitColor);
        spriteRenderer.color = HitColor;
        if (Hp<=0)
        {
            gameObject.SetActive(false);
        }
    }

    public void StartHit()
    {
        if(Hit_co == null)
            Hit_co = StartCoroutine(HitTarget());
        if(Hit_co != null)
        {
            StopCoroutine(HitTarget());
            Hit_co = null;
            spriteRenderer.color = Color.white;
        }

    }

    private IEnumerator HitTarget()
    {
        Hp -= 1;
        spriteRenderer.color = HitColor;
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.color = Color.white;
        Hit_co = null;

    }
}
