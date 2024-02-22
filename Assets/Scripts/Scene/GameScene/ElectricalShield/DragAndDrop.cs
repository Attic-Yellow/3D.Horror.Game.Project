using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public float minX = -0.1f;
    public float maxX = 0.04f;


    private void OnMouseDrag()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z));

        // 로컬 좌표계에서의 마우스 위치로 변환
        Vector3 localMousePos = transform.parent.InverseTransformPoint(mousePos);

        // x 값을 minX와 maxX 사이로 제한
        localMousePos.x = Mathf.Clamp(localMousePos.x, minX, maxX);

        // 오브젝트의 위치 설정
        transform.localPosition = new Vector3(localMousePos.x, transform.localPosition.y, transform.localPosition.z);


    }


}
