using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : Container
{
    [SerializeField] private Vector3 initpos;

    private void Awake()
    {
        initpos = transform.localPosition;
    }

    public void DrawerController()
    {
        StartCoroutine(DrawerPosCoroutine());
    }

    IEnumerator DrawerPosCoroutine()
    {
        float elapsedTime = 0f;
        float duration = 0.5f; // 총 이동할 시간 설정
        Vector3 startPos = transform.localPosition;
        Vector3 endPos;

        if (isOpen)
        {
            // 서랍을 닫는 경우, 초기 위치로 이동
            endPos = initpos;
        }
        else
        {
            // 서랍을 여는 경우, Z 위치를 조정하여 목표 위치 설정
            endPos = new Vector3(initpos.x, initpos.y, initpos.z + 0.3f);
        }

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            // 선형 보간을 사용하여 현재 위치 업데이트
            transform.localPosition = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            yield return null;
        }

        // 문의 상태 업데이트
        isOpen = !isOpen;

        // 텍스트 상태 업데이트
        for (int i = 0; i < openText.Length; i++)
        {
            if (openText != null)
            {
                openText[i].gameObject.SetActive(!isOpen);
            }

            if (closeText != null)
            {
                closeText[i].gameObject.SetActive(isOpen);
            }
        }
    }
}
