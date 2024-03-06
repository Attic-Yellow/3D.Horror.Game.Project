using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardCanvas : MonoBehaviour
{
    private void Update()
    {        
        // ī�޶� ���������� ���͸� ���
        Vector3 direction = Camera.main.transform.position - transform.position;
        direction.y = 0; // y �� ������ �����ϵ��� y ���� 0���� ����

        // �� �������� ȸ����Ű�� ���� Quaternion�� ���
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // ������Ʈ�� ȸ���� ���ο� ȸ������ ����
        transform.rotation = targetRotation;
    }
}
