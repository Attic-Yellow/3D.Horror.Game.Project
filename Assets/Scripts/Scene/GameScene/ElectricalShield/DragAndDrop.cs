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

        // ���� ��ǥ�迡���� ���콺 ��ġ�� ��ȯ
        Vector3 localMousePos = transform.parent.InverseTransformPoint(mousePos);

        // x ���� minX�� maxX ���̷� ����
        localMousePos.x = Mathf.Clamp(localMousePos.x, minX, maxX);

        // ������Ʈ�� ��ġ ����
        transform.localPosition = new Vector3(localMousePos.x, transform.localPosition.y, transform.localPosition.z);


    }


}
