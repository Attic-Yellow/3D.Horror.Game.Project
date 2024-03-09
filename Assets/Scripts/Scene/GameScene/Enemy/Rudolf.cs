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
    [SerializeField] private bool isGoToPlayer = false;
    [SerializeField] private bool isPlayerCome = false;
    [SerializeField] private bool isRunAway = false;

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

                if(isRunAway)
                gameObject.SetActive(false);
            }
            else //ȭ�� �ȿ�������
            {
                timer = 0f;
                Debug.Log("���� ȭ�� �ȿ� ����");

                if (isGoToPlayer)
                    RunAway();
     

            }
            if (timer > 2f) //2�ʰ� �����
            {
                if (!agent.hasPath)
                {
                    isGoToPlayer = true;
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
        isGoToPlayer = false;
        isRunAway = true;
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
    
    }

    private void Ani()
    {
        animator.SetBool("PlayerCheck", isPlayerCome&&isGoToPlayer);
        animator.SetBool("IsRunAway",isPlayerCome &&!isGoToPlayer);
    }
}
