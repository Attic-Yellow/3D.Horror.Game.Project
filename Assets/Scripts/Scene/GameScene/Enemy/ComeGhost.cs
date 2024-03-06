using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class ComeGhost : Enemy
{
    public float detectionRadius = 10f; // �÷��̾ �����ϴ� �ݰ�
    public bool isSee = false;
    public GameObject[] childObj;

    void Update()
    {
        if (!player.isOver)
        {
            // ���� ���� ���� �ִ� ��� �÷��̾ ����
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
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
