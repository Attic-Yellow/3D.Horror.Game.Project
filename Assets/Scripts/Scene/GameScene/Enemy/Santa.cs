using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class Santa : Enemy
{
    private void Update()
    {

        CheckAll();
        switch (state)
        {

            case State.Move:

              
                if (isFrontDoor == null )
                {
                    if (!isMoving)
                    {
                        if (timer >= eventDelay) // �÷��̾ �� �� �� eventDelay��ŭ ������ ��
                        {
                            print("�̺�Ʈ");
                            EnemyEvent();
                        }
                        else // �÷��̾ �������� ���ϰ� ���󰡴� ���� �ƴ� ��
                        {
                            print("�Ϲ����� �̵�");

                            MoveToNextTransform();

                        }
                    }
                }
                else 
                {
                    if (!isOpenAndMove)
                    {
                        if (Vector3.Distance(isFrontDoor.gameObject.transform.position, transform.position) >= 1f )
                        {
                            print("�������� �̵�");
                            agent.SetDestination(isFrontDoor.gameObject.transform.position);
                        }
                        else
                        {
                            state = State.OpenDoor;
                        }
                    }
                    else
                    {
                        if (Vector3.Distance(isFrontDoor.inRoomTransform.position, transform.position) >= 1f)
                        {
                            print("������� �̵�");
                            agent.SetDestination(isFrontDoor.inRoomTransform.position);
                        }
                        else
                        {
                            isOpenAndMove = false;
                            isFrontDoor = null;
                            state = State.LookingAround;
                        }
                      
                    }
                }

                break;
            case State.Follow:
                print("�÷��̾� ���󰡴���");
                timer = 0f;
                isFollowingPlayer = true;
                agent.speed = runSpeed;
                agent.SetDestination(player.transform.position);
                if (!watchedPlayer && !agent.hasPath)
                {
                    state = State.Move;
                    isFollowingPlayer = false;
                }
                break;
            case State.Crouch:

                break;

            case State.Stun:

                break;
            case State.OpenDoor:

                if (!isOpenAndMove)
                {
                    openDoorCoroutine = StartCoroutine(OpenAndIntheRoom());
                }
                break;
            case State.LookingAround:
                print("���ƺ�");
                agent.ResetPath();
                lookingAroundTimer += Time.deltaTime;
                if (lookingAroundTimer >= lookingAroundDuration)
                {
                    lookingAroundTimer = 0f; // Ÿ�̸� �ʱ�ȭ
                    state = State.Move;

                }
                break;

        }

        animator.SetInteger("State",(int) state);

    }
  

}
