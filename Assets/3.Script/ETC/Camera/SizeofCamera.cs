using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeofCamera : MonoBehaviour
{
    [SerializeField] private Camera renderCamera;
    [SerializeField] private bool isSetSize;
    private SpriteRenderer textureRenderer;
    public Vector3 BottomLeft;
    public Vector3 BottomRight;
    public Vector3 TopLeft;
    public Vector3 TopRight;
   


    private void Start()
    {
        BottomLeft = renderCamera.ViewportToWorldPoint(new Vector3(0, 0, renderCamera.nearClipPlane));
        BottomRight = renderCamera.ViewportToWorldPoint(new Vector3(1, 0, renderCamera.nearClipPlane));
        TopLeft = renderCamera.ViewportToWorldPoint(new Vector3(0, 1, renderCamera.nearClipPlane));
        TopRight = renderCamera.ViewportToWorldPoint(new Vector3(1, 1, renderCamera.nearClipPlane));
        if (isSetSize)
            ResizeTextureToCamera();
    }
    
    public Vector3 ClickPos(Vector3 MousePos)
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    // 마우스 위치를 스크린 좌표로 가져옴
        //    Vector3 MousePos = Input.mousePosition;
        // 카메라의 깊이를 지정하여 스크린 좌표를 월드 좌표로 변환
        Vector3 worldPosition = renderCamera.ScreenToWorldPoint(new Vector3(MousePos.x, MousePos.y, renderCamera.transform.position.z * -1));

        //}
        return worldPosition;
    }
    

    private void ResizeTextureToCamera()
    {
        if(renderCamera == null)
            renderCamera = Camera.main;

        textureRenderer = GetComponent<SpriteRenderer>();

        float cameraY = renderCamera.orthographicSize * 2f;
        float cameraX = cameraY * renderCamera.aspect;

        float textureY = textureRenderer.bounds.size.y;
        float textureX = textureRenderer.bounds.size.x;

        float scaleX = cameraX / textureX;
        float scaleY = cameraY / textureY;

        textureRenderer.transform.localScale = new Vector3(scaleX, scaleY, 1f);
    }



}
