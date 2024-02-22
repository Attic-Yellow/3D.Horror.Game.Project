using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
public class ElectricalShield : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Vector3 dragStartPosition;
    private float maxLeftPosition = 0.04f;
    private float maxRightPosition = -0.1f;

    private void Update()
    {
       
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        dragStartPosition = transform.position;
        print("눌렀어");
    }

    public void OnDrag(PointerEventData eventData)
    {
        print("옮기는중");
            Vector3 mouseDelta = new Vector3(eventData.delta.x, 0, 0);

            // 현재 포지션 값에 마우스 이동량을 더합니다.
            Vector3 newPosition = dragStartPosition + mouseDelta;

            // X값이 최대 이동 가능한 값보다 작으면 최대 이동 가능한 값으로 설정합니다.
            if (newPosition.x < maxLeftPosition)
            {
                newPosition.x = maxLeftPosition;
            }
            // X값이 최대 이동 가능한 값보다 크면 최대 이동 가능한 값으로 설정합니다.
            else if (newPosition.x > maxRightPosition)
            {
                newPosition.x = maxRightPosition;
            }

            // 새로운 포지션을 적용합니다.
            transform.position = newPosition;
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        print("놨어");
    }
}
