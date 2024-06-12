using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeTextureToCamera : MonoBehaviour
{
    [SerializeField] private Camera GetCamera;
    private Renderer textureRenderer;

    private void Awake()
    {
        if(GetCamera == null)
        {
            Debug.Log("1");
            GetCamera = GetComponent<Camera>();
        }
        Debug.Log("2");
        textureRenderer = GetComponent<Renderer>();
        Debug.Log("3");
        float cameraY = GetCamera.orthographicSize * 2f;
        float cameraX = cameraY * GetCamera.aspect;
        Debug.Log(cameraY + " ī�޶� " + cameraX);

        float textureY = textureRenderer.bounds.size.y;
        float textureX = textureRenderer.bounds.size.x;
        Debug.Log(textureY + " �ؽ��� " + textureX);

        float scaleX = cameraX / textureX;
        float scaleY = cameraY / textureY;
        Debug.Log(scaleX + " ������ " + scaleY);
        textureRenderer.transform.localScale = new Vector3(scaleX, scaleY, 1f);
    }
}
