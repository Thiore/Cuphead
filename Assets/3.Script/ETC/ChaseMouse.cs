using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseMouse : MonoBehaviour
{
    [SerializeField] private GameObject Main;
    public RectTransform canvasRectTransform; // 캔버스의 RectTransform
    public RectTransform targetRectTransform; // 마우스 위치를 표시할 대상 RectTransform
    Vector2 TexturePosition = new Vector2(20, -20);
    void Update()
    {
        Vector2 localPoint;
        Vector2 screenPoint = Input.mousePosition;
        

        // ScreenPoint를 Canvas의 RectTransform 로컬 좌표로 변환
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPoint, null, out localPoint))
        {
            // 변환된 로컬 좌표를 target RectTransform의 위치로 설정
            targetRectTransform.anchoredPosition = localPoint + TexturePosition;
        }
    }

   
}
