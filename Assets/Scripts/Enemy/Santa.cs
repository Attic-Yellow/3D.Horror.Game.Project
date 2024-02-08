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
            if (watchedPlayer) // 발견됐을 때 플레이어를 따라가는
            {
                timer = 0f;
                print($"{player.transform.position}플레이어 따라가는중");
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
            if (timer >= eventDelay) // 플레이어를 못 본 지 eventDelay만큼 지났을 때
            {
                print("이벤트");
                isFollowingPlayer = true;
                EnemyEvent();
            }

            if (!watchedPlayer && !isFollowingPlayer && !isMoving) // 플레이어를 감지하지 못하고 따라가는 중이 아닐 때
            {
                print("일반적인 이동");

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
