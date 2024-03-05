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
        
              
                break;

            case State.Follow:
                print("�÷��̾� ���󰡴���");
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
                print("���ƺ�");
                agent.ResetPath();
                lookingAroundTimer += Time.deltaTime;
                if (lookingAroundTimer >= lookingAroundDuration)
                {
                    lookingAroundTimer = 0f; // Ÿ�̸� �ʱ�ȭ
                    state = State.Move;

                }
                break;

            case State.Over:
                //������ ��������
                print("������");
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
