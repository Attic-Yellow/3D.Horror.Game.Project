using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    public bool isOut = true; // 밖에서 문을 여는지 안에서 여는지
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
        // isOut 상태에 따라 문 열기 방향 결정
        Quaternion targetRotation = isOut ? startRotation * Quaternion.Euler(0f, 100f, 0f) : startRotation * Quaternion.Euler(0f, -100f, 0f);
        StartCoroutine(RotateDoorCoroutine(targetRotation));
        isOut = !isOut;
    }

    public void CloseDoor()
    {
        // 문 닫을 때 역방향으로 회전
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

        // 문의 상태 업데이트
        isOpen = !isOpen;

        // 애니메이션 트리거 설정
      /*  if (isOpen)
        {
            animator.SetTrigger("IsOpen");
        }*/
    }
}
