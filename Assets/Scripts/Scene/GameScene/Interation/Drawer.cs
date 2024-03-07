using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawer : Container
{
    [SerializeField] private Vector3 initpos;
    private Coroutine drawerCoroutine = null;
    private void Awake()
    {
        initpos = transform.localPosition;
    }

    public void DrawerController()
    {
        if (drawerCoroutine == null)
        {
            drawerCoroutine = StartCoroutine(DrawerPosCoroutine());
        }
    }

    IEnumerator DrawerPosCoroutine()
    {
        float elapsedTime = 0f;
        float duration = 0.5f; // �� �̵��� �ð� ����
        Vector3 startPos = transform.localPosition;
        Vector3 endPos;

        if (isOpen)
        {
            // ������ �ݴ� ���, �ʱ� ��ġ�� �̵�
            endPos = initpos;
        }
        else
        {
            // ������ ���� ���, Z ��ġ�� �����Ͽ� ��ǥ ��ġ ����
            endPos = new Vector3(initpos.x, initpos.y, initpos.z + 0.3f);
        }

        GameManager.instance.settingsManager.PlayClip(0);

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            // ���� ������ ����Ͽ� ���� ��ġ ������Ʈ
            transform.localPosition = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
            yield return null;
        }

        drawerCoroutine = null;

        // ���� ���� ������Ʈ
        isOpen = !isOpen;

        // �ؽ�Ʈ ���� ������Ʈ
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
