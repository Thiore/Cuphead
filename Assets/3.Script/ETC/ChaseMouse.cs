using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseMouse : MonoBehaviour
{
    [SerializeField] private GameObject Main;
    public RectTransform canvasRectTransform; // ĵ������ RectTransform
    public RectTransform targetRectTransform; // ���콺 ��ġ�� ǥ���� ��� RectTransform
    Vector2 TexturePosition = new Vector2(20, -20);
    void Update()
    {
        Vector2 localPoint;
        Vector2 screenPoint = Input.mousePosition;
        

        // ScreenPoint�� Canvas�� RectTransform ���� ��ǥ�� ��ȯ
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, screenPoint, null, out localPoint))
        {
            // ��ȯ�� ���� ��ǥ�� target RectTransform�� ��ġ�� ����
            targetRectTransform.anchoredPosition = localPoint + TexturePosition;
        }
    }

   
}
