using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen;
    public bool isOut = true; // �ۿ��� ���� ������ �ȿ��� ������
    public float rotationSpeed = 4f;
    public Quaternion startRotation;
    public Transform inRoomTransform;

    [SerializeField] private Animator animator;

    [SerializeField] private TextMeshProUGUI[] openText;
    [SerializeField] private TextMeshProUGUI[] closeText;

    private void Awake()
    {
        startRotation = transform.rotation;
    }

    private void Start()
    {
        for (int i = 0; i < 2; i++)
        {
            openText[i].gameObject.SetActive(!isOpen);
            closeText[i].gameObject.SetActive(isOpen);
        }
    }

    public void OpenDoor()
    {
        // isOut ���¿� ���� �� ���� ���� ����
        Quaternion targetRotation = isOut ? startRotation * Quaternion.Euler(0f, 130f, 0f) : startRotation * Quaternion.Euler(0f, -130f, 0f);
        StartCoroutine(RotateDoorCoroutine(targetRotation));
    }

    public void CloseDoor()
    {
        StartCoroutine(RotateDoorCoroutine(startRotation));
    }

    IEnumerator RotateDoorCoroutine(Quaternion targetRotation)
    {
        float elapsedTime = 0f;
        Quaternion currentRotation = transform.rotation;

        while (elapsedTime < 2.5f)
        {
            elapsedTime += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, elapsedTime / 2.5f);
            yield return null;
        }

        // ���� ���� ������Ʈ
        isOpen = !isOpen;

        for (int i = 0; i < 2; i++)
        {
            openText[i].gameObject.SetActive(!isOpen);
            closeText[i].gameObject.SetActive(isOpen);
        }
    }
}
