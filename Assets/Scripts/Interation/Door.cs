using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    public bool isOut = true; // �ۿ��� ���� ������ �ȿ��� ������
    public float rotationSpeed = 4f;
    public Quaternion startRotation;
    public Transform inRoomTransform;

    [SerializeField] private Animator animator;

    private void Awake()
    {
        startRotation = transform.rotation;
    }

    public void OpenDoor()
    {
        // isOut ���¿� ���� �� ���� ���� ����
        Quaternion targetRotation = isOut ? startRotation * Quaternion.Euler(0f, 100f, 0f) : startRotation * Quaternion.Euler(0f, -100f, 0f);
        StartCoroutine(RotateDoorCoroutine(targetRotation));
        isOut = !isOut;
    }

    public void CloseDoor()
    {
        // �� ���� �� ���������� ȸ��
        Quaternion targetRotation = isOut ? startRotation * Quaternion.Euler(0f, 120f, 0f) : startRotation * Quaternion.Euler(0f, -120f, 0f);
        StartCoroutine(RotateDoorCoroutine(startRotation));
    }

    IEnumerator RotateDoorCoroutine(Quaternion targetRotation)
    {
        float elapsedTime = 0f;
        Quaternion currentRotation = transform.rotation;

        while (elapsedTime < 1.5f)
        {
            elapsedTime += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, elapsedTime / 1.5f);
            yield return null;
        }

        // ���� ���� ������Ʈ
        isOpen = !isOpen;

        // �ִϸ��̼� Ʈ���� ����
      /*  if (isOpen)
        {
            animator.SetTrigger("IsOpen");
        }*/
    }
}
