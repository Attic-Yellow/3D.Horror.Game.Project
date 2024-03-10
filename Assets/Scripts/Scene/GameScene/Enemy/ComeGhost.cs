using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class ComeGhost : Enemy
{
    public float boxXDistance = 10f; 
    public float boxZDistance = 2f;
    public bool isSee = false;
    public GameObject[] childObj;
    private Vector3 detectionBoxSize; // ���� �ڽ��� ũ��

    private new void Awake()
    {
        base.Awake();
        detectionBoxSize = new Vector3(boxXDistance, 1f, boxZDistance);
    }
    void Update()
    {
        if (!player.isOver)
        {
            // ���� ���� ���� �ִ� ��� �÷��̾ ����
            Collider[] colliders = Physics.OverlapBox(transform.position + transform.forward * boxXDistance * 0.5f, detectionBoxSize * 0.5f);
            bool playerDetected = false; // �÷��̾ �߰��ߴ��� ���θ� ��Ÿ���� ����

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    // �÷��̾ �߰���
                    playerDetected = true;
                    Debug.Log("�ͽ��� �÷��̾ �����߽��ϴ�!");
                    break; // �� �̻� �˻����� �ʰ� ������ ��������
                }
            }
           


            if (playerDetected)
            {
                transform.LookAt(player.transform);

                if (isSee)
                {
                    foreach (GameObject obj in childObj)
                    {
                        obj.SetActive(true);
                    }
                }
                else
                {
                    foreach (GameObject obj in childObj)
                    {
                        obj.SetActive(false);
                    }
                    // �÷��̾� �������� �ͽ��� �̵���Ŵ
                    Vector3 direction = (player.transform.position - transform.position).normalized;
                    direction.y = 0; // y�� �̵� ���� ����
                    transform.Translate(direction * walkSpeed * Time.deltaTime, Space.World);
                }
            }
            else
            {
                SetActiveTrue();
            }
        }
    }

    public void SetActiveTrue()
    {
        foreach (GameObject obj in childObj)
        {
            obj.SetActive(true);
        }
    }
}
