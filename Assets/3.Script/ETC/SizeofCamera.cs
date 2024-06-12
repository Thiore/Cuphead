using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeofCamera : MonoBehaviour
{
    [SerializeField] private Camera MainCamera;
    public Vector3 BottomLeft;
    public Vector3 BottomRight;
    public Vector3 TopLeft;
    public Vector3 TopRight;
   


    private void Start()
    {
        
        BottomLeft = MainCamera.ViewportToWorldPoint(new Vector3(0, 0, MainCamera.nearClipPlane));
        BottomRight = MainCamera.ViewportToWorldPoint(new Vector3(1, 0, MainCamera.nearClipPlane));
        TopLeft = MainCamera.ViewportToWorldPoint(new Vector3(0, 1, MainCamera.nearClipPlane));
        TopRight = MainCamera.ViewportToWorldPoint(new Vector3(1, 1, MainCamera.nearClipPlane));
    }
    
    public Vector3 ClickPos(Vector3 MousePos)
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    // 마우스 위치를 스크린 좌표로 가져옴
        //    Vector3 MousePos = Input.mousePosition;
        // 카메라의 깊이를 지정하여 스크린 좌표를 월드 좌표로 변환
        Vector3 worldPosition = MainCamera.ScreenToWorldPoint(new Vector3(MousePos.x, MousePos.y, MainCamera.transform.position.z * -1));

        //}
        return worldPosition;
    }


}
