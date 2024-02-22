using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 offset;
    public float minX = -0.1f;
    public float maxX = 0.04f;

    private void OnMouseDrag()
    {
        float distance = Camera.main.WorldToScreenPoint(transform.localPosition).z;

        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePos);

        objPos.y = transform.localPosition.y;
        objPos.x = transform.localPosition.x;
        transform.localPosition = objPos;


    }


}
