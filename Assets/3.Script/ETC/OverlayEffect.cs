using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
[RequireComponent(typeof(Camera))]
public class OverlayEffect : MonoBehaviour
{
    [SerializeField] private Material FXOverlayMaterial;
    [SerializeField] private Texture2D[] FXTex;
    [SerializeField] private Texture2D[] FX2Tex;
    public float animationSpeed = 10.0f;  // 프레임 변경 속도
    private float currentFrame;
    private int lastFrame;

    void Start()
    {
        currentFrame = 0;
        lastFrame = 0;
        FXOverlayMaterial.SetTexture("_DustTex", FXTex[0]);
        FXOverlayMaterial.SetTexture("_DustTex", FX2Tex[0]);
    }

    void Update()
    {
        currentFrame += animationSpeed * Time.deltaTime;

        if (currentFrame >= 127) 
            currentFrame = 0;

        

        if(!lastFrame.Equals(Mathf.FloorToInt(currentFrame)))
        {
            lastFrame = Mathf.FloorToInt(currentFrame);
            SetFXTex(lastFrame);
        }


        //FXOverlayMaterial.SetFloat("_DustFrame", currentFrame);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (FXOverlayMaterial != null)
        {
            Graphics.Blit(src, dest, FXOverlayMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }

    private void SetFXTex(int currentFrame)
    {
        FXOverlayMaterial.SetTexture("_DustTex", FXTex[currentFrame]);
        FXOverlayMaterial.SetTexture("_Dust2Tex", FX2Tex[currentFrame]);
    }    
}
