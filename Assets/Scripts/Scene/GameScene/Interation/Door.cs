using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Door : Container
{
    public bool isOut = true; // 밖에서 문을 여는지 안에서 여는지
    public bool isLock = false;
    private Coroutine textcoroutine = null;
    [SerializeField] protected float rotationSpeed = 4f;
    [SerializeField] protected Quaternion startRotation;
    [SerializeField] protected float openAngle;
    private Coroutine doorCoroutine = null;
    /*private OcclusionPortal occlusionPortal;*/

    // [SerializeField] private Animator animator;

    protected void Awake()
    {
        startRotation = Quaternion.identity;
        /*occlusionPortal = GetComponent<OcclusionPortal>();*/
    }

    public void OpenDoor()
    {
        print("문열어");
        if(isLock)
        {
            if (Holder.Instance.isHaveItems["Key"])
            {
                print("키가있어");
                isLock = false;
            }
            else
            {
                if (textcoroutine == null)
                {
                    textcoroutine = StartCoroutine(ScreenTextOverlayOff()); 
                }
                return;
            }   
        }
        if (doorCoroutine == null)
        {
            GameManager.instance.settingsManager.PlayClip(5);
            Quaternion targetRotation = isOut ? startRotation * Quaternion.Euler(0f, openAngle, 0f) : startRotation * Quaternion.Euler(0f, -openAngle, 0f);
            doorCoroutine = StartCoroutine(RotateDoorCoroutine(targetRotation));
        }
        /*occlusionPortal.open = true;*/
    }

    public void CloseDoor()
    {
        if (doorCoroutine == null)
        {
            GameManager.instance.settingsManager.PlayClip(3);
            doorCoroutine = StartCoroutine(RotateDoorCoroutine(startRotation));
        }
    }

    protected IEnumerator RotateDoorCoroutine(Quaternion targetRotation)
    {
        float elapsedTime = 0f;
        Quaternion currentRotation = transform.localRotation;

        while (elapsedTime < 5f)
        {
            elapsedTime += Time.deltaTime * rotationSpeed;
            transform.localRotation = Quaternion.Slerp(currentRotation, targetRotation, elapsedTime / 5f);
            yield return null;
        }
        doorCoroutine = null;
        // 문의 상태 업데이트
        isOpen = !isOpen;

        for (int i = 0; i < openText.Length; i++)
        {
            if (openText != null)
            {
                openText[i].gameObject.SetActive(!isOpen);
            }

            if (closeText.Length != 0)
            {
                closeText[i].gameObject.SetActive(isOpen);
            }
        }
    }

    protected IEnumerator ScreenTextOverlayOff()
    {
        GameManager.instance.overlayManager.ScrenTextOverlayController();
        print("텍스트 나오고");
        yield return new WaitForSeconds(1.5f);
        GameManager.instance.overlayManager.ScrenTextOverlayController();
        textcoroutine = null;
        print("텍스트 꺼지고");
    }
}
