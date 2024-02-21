using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class ComeGhost : MonoBehaviour
{
    public float detectionRadius = 10f; // �÷��̾ �����ϴ� �ݰ�
    public bool isSee = false;
    public GameObject[] childObj;
    private Transform player; // �÷��̾��� ��ġ
    public float moveSpeed = 1f; // �ͽ��� �̵� �ӵ�

    void Start()
    {
        // �÷��̾ ã��
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
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
                Vector3 direction = (player.position - transform.position).normalized;
                transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
            }
        }
    }
}
