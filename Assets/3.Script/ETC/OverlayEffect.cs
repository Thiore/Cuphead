using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OverlayEffect : MonoBehaviour
{
    [SerializeField] private Material FXOverlayMaterial;
    [SerializeField] private RenderTexture renderTexture;
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
        FXOverlayMaterial.SetTexture("_Dust2Tex", FX2Tex[0]);
    }

    void Update()
    {
        

        currentFrame += animationSpeed * Time.deltaTime;

        if (currentFrame >= FXTex.Length) 
            currentFrame = 0;

        int newFrame = Mathf.FloorToInt(currentFrame);

        if (!lastFrame.Equals(newFrame))
        {
            lastFrame = newFrame;
            SetFXTex(lastFrame);
        }
        
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        FXOverlayMaterial.SetTexture("_MainTex", renderTexture);
        
        Graphics.Blit(renderTexture, dest, FXOverlayMaterial);
        
    }

    private void SetFXTex(int currentFrame)
    {
        FXOverlayMaterial.SetTexture("_DustTex", FXTex[currentFrame]);
        FXOverlayMaterial.SetTexture("_Dust2Tex", FX2Tex[currentFrame]);
    }    
}
