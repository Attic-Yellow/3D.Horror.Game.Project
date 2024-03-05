using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rudolf : MovingEnemy
{
 


    private void Update()
    {

        CheckAll();

        switch (state)
        {

            case State.Move:

              
                    if (!isMoving)
                    {
                        if (timer >= eventDelay) // 플레이어를 못 본 지 eventDelay만큼 지났을 때
                        {
                            print("이벤트");
                            EnemyEvent();
                        }
                        else // 플레이어를 감지하지 못하고 따라가는 중이 아닐 때
                        {
                            print("일반적인 이동");

                            MoveToNextTransform();

                        }
                    }
        
              
                break;

            case State.Follow:
                print("플레이어 따라가는중");
                timer = 0f;
                agent.speed = runSpeed;
                agent.SetDestination(player.transform.position);
                if (!watchedPlayer && !agent.hasPath)
                {
                    state = State.Move;
                }
                break;
            case State.Crouch:

                break;

            case State.Stun:

                break;

            case State.OpenDoor:

 
                break;

            case State.LookingAround:
                print("돌아봐");
                agent.ResetPath();
                lookingAroundTimer += Time.deltaTime;
                if (lookingAroundTimer >= lookingAroundDuration)
                {
                    lookingAroundTimer = 0f; // 타이머 초기화
                    state = State.Move;

                }
                break;

            case State.Over:
                //끝나면 게임종료
                print("오버야");
                break;
        }
        animator.SetInteger("State", (int)state);
     

    }

    protected override void CheckAll()
    {
        timer += Time.deltaTime;

        if (player.isOver && state != State.Over)
        {
            agent.ResetPath();
            state = State.Over;

        }
        else
        {
            watchedPlayer = enemyCameraDetection.IsPlayerVisible();

            if (state != State.Follow && IsMovingCheck())
            {
                state = State.Follow;

                return;
            }

            if (!agent.hasPath && isMoving)
            {
                isMoving = false;

            }
        }
    }

}
