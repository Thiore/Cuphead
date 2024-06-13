using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overlay : MonoBehaviour
{
    [SerializeField] private Camera Overlay_Camera;

    private RenderTexture renderTexture;
    private SpriteRenderer spriteRenderer;
    private Texture2D texture;

    float cameraY;
    float cameraX;
    float textureY;
    float textureX;
    float scaleX;
    float scaleY;

    void Start()
    {
        cameraY = Overlay_Camera.orthographicSize * 2f;
        cameraX = cameraY * Overlay_Camera.aspect;

        spriteRenderer = GetComponent<SpriteRenderer>();

        scaleX = cameraX / spriteRenderer.bounds.size.x;
        scaleY = cameraY / spriteRenderer.bounds.size.y;

        renderTexture = new RenderTexture((int)cameraX, (int)cameraY,0);
        
        


    }

    void Update()
    {
        if (renderTexture != null)
        {
           

            Texture2D tex = new Texture2D((int)cameraX, (int)cameraY, TextureFormat.RGB24, false);
            Debug.Log((int)cameraX + " " + (int)cameraY);
            RenderTexture.active = renderTexture;
            tex.ReadPixels(new Rect(0, 0, (int)cameraX, (int)cameraY), 0, 0);
            tex.Apply();
            RenderTexture.active = null;

            Color[] pixels = tex.GetPixels();

            

            spriteRenderer.transform.localScale = new Vector3(scaleX, scaleY, 1f);


            texture = spriteRenderer.sprite.texture;
            Color[] FXpixels = texture.GetPixels();

            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] *= FXpixels[i];
            }
            tex.SetPixels(pixels);
            tex.Apply();

            Overlay_Camera.targetTexture = null;
            Graphics.Blit(tex, renderTexture);
            Overlay_Camera.targetTexture = renderTexture;


        }
    }

}
