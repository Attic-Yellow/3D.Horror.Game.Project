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
        print("������");
    }

    public void OnDrag(PointerEventData eventData)
    {
        print("�ű����");
            Vector3 mouseDelta = new Vector3(eventData.delta.x, 0, 0);

            // ���� ������ ���� ���콺 �̵����� ���մϴ�.
            Vector3 newPosition = dragStartPosition + mouseDelta;

            // X���� �ִ� �̵� ������ ������ ������ �ִ� �̵� ������ ������ �����մϴ�.
            if (newPosition.x < maxLeftPosition)
            {
                newPosition.x = maxLeftPosition;
            }
            // X���� �ִ� �̵� ������ ������ ũ�� �ִ� �̵� ������ ������ �����մϴ�.
            else if (newPosition.x > maxRightPosition)
            {
                newPosition.x = maxRightPosition;
            }

            // ���ο� �������� �����մϴ�.
            transform.position = newPosition;
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        print("����");
    }
}
