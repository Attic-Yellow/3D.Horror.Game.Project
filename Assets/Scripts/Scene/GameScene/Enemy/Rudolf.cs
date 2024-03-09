using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UIElements;

public class Rudolf : MovingEnemy
{
    [SerializeField] private Camera mainCamera; // 메인 카메라
    [SerializeField] private bool isRunAway = false;
    [SerializeField] private bool isPlayerCome = false;


    private void Update()
    {
        Ani();
        if (isPlayerCome)
        {
            Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);

            if (viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1) //화면 밖으로 벗어나면
            {
                timer += Time.deltaTime;
                Debug.Log("적이 화면을 벗어났습니다!");

            }
            else //화면 안에있으면
            {
                timer = 0f;
                Debug.Log("적이 화면 안에 감지");

                if (isRunAway)
                    RunAway();
     

            }
            if (timer > 2f) //2초간 벗어나면
            {
                if (!agent.hasPath)
                {
                    isRunAway = true;
                    agent.SetDestination(player.transform.position);
                }
            }
        }
        else //플레이어를 체크를 아직 못했으면
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
