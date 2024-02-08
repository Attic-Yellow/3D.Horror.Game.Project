using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class Santa : Enemy
{
    private new void Awake()
    {
        base.Awake();
    }


    private void Update()
    {
      
       /*CheckDistance();*/
        watchedPlayer = enemyCameraDetection.IsPlayerVisible(player.transform.position);
        animator.SetBool("IsWatched", watchedPlayer);
        


        if (isFrontDoor==null)
        {
            isFrontDoor = enemyCameraDetection.DoorCheck();
            timer += Time.deltaTime;
            if (watchedPlayer) // �߰ߵ��� �� �÷��̾ ���󰡴�
            {
                timer = 0f;
                print($"{player.transform.position}�÷��̾� ���󰡴���");
                isFollowingPlayer = true;
                agent.speed = runSpeed;
                agent.SetDestination(player.transform.position);
            }
            else
            {
                isFollowingPlayer = false;
            }

            if (!agent.hasPath)
            {
                isMoving = false;
            }
            if (timer >= eventDelay) // �÷��̾ �� �� �� eventDelay��ŭ ������ ��
            {
                print("�̺�Ʈ");
                isFollowingPlayer = true;
                EnemyEvent();
            }

            if (!watchedPlayer && !isFollowingPlayer && !isMoving) // �÷��̾ �������� ���ϰ� ���󰡴� ���� �ƴ� ��
            {
                print("�Ϲ����� �̵�");

                MoveToNextTransform();

            }
        }
        else
        {
            if (Vector3.Distance(isFrontDoor.transform.position, transform.position) >= 1f)
            {
                agent.SetDestination(isFrontDoor.transform.position);
            }
            else
            {
                if(!isOpenAndMove)
                StartCoroutine(OpenAndIntheRoom());
            }

        }
    }


}
