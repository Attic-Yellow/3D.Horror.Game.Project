using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    public bool isOut;
    public float rotationSpeed = 5f;
    public Quaternion startRotation;
    public Transform inRoomTransform;

    [SerializeField] private Animator animator;

    private void Awake()
    {
        startRotation = transform.rotation;
    }


    public void OpenDoor() //안쪽에서 여는지
    {
        if(!isOut)
        {
            StartCoroutine(RotateDoorCoroutine(startRotation * Quaternion.Euler(0f, -90f, 0f)));
        }
        else
        {
            StartCoroutine(RotateDoorCoroutine(startRotation * Quaternion.Euler(0f, 90f, 0f)));
        }
    }

    public void CloseDoor()
    {
        StartCoroutine(RotateDoorCoroutine(startRotation));
    }

    IEnumerator RotateDoorCoroutine(Quaternion targetRotation)
    {
        float elapsedTime = 0f;
        Quaternion currentRotation = transform.rotation;

        while (elapsedTime < 1.5f)
        {
            elapsedTime += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, elapsedTime);
            animator.SetTrigger("IsOpen");
            yield return null;
        }

        isOpen = !isOpen;
    }
}
