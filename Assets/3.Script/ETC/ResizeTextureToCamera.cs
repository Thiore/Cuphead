using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeTextureToCamera : MonoBehaviour
{
    private Camera mainCamera;
    private SpriteRenderer textureRenderer;

    private void Awake()
    {
        mainCamera = Camera.main;
        textureRenderer = GetComponent<SpriteRenderer>();
        
        float cameraY = mainCamera.orthographicSize * 2f;
        float cameraX = cameraY * mainCamera.aspect;
        
        float textureY = textureRenderer.bounds.size.y;
        float textureX = textureRenderer.bounds.size.x;
        
        float scaleX = cameraX / textureX;
        float scaleY = cameraY / textureY;
        
        textureRenderer.transform.localScale = new Vector3(scaleX, scaleY, 1f);
    }
}
