using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UIElements;

public class Rudolf : MovingEnemy
{
    [SerializeField] private Camera mainCamera; // ���� ī�޶�
    [SerializeField] private bool isRunAway = false;
    [SerializeField] private bool isPlayerCome = false;


    private void Update()
    {
        Ani();
        if (isPlayerCome)
        {
            Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);

            if (viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1) //ȭ�� ������ �����
            {
                timer += Time.deltaTime;
                Debug.Log("���� ȭ���� ������ϴ�!");

            }
            else //ȭ�� �ȿ�������
            {
                timer = 0f;
                Debug.Log("���� ȭ�� �ȿ� ����");

                if (isRunAway)
                    RunAway();
     

            }
            if (timer > 2f) //2�ʰ� �����
            {
                if (!agent.hasPath)
                {
                    isRunAway = true;
                    agent.SetDestination(player.transform.position);
                }
            }
        }
        else //�÷��̾ üũ�� ���� ��������
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 2);
            foreach(Collider collider in colliders)
            {
               if (collider.GetComponent<Player>()!=null)
                {
                    isPlayerCome = true;
                }
            }
        }
    }

    private void RunAway()
    {
        isRunAway = false;
        bool isFind = false;
        Vector3 randomDirection = Random.insideUnitSphere;
        randomDirection.y = 0;
        Vector3 targetPosition = player.transform.position + randomDirection.normalized * 10f; 

        while (!isFind)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPosition, out hit, 10f, NavMesh.AllAreas))
            {
                targetPos = hit.position;
                agent.speed = walkSpeed;
                isFind = true;
            }
        }

        agent.SetDestination(targetPos);
        if (agent.remainingDistance <= 0.5f && !agent.hasPath)
        {
            gameObject.SetActive(false);
        }
    }

    private void Ani()
    {
        animator.SetBool("PlayerCheck", isPlayerCome&&isRunAway);
        animator.SetBool("IsRunAway",isPlayerCome &&!isRunAway);
    }
}
