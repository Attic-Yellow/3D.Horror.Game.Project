using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class Santa : MovingEnemy
{
  [SerializeField] private bool isStunned = false;
    [SerializeField] Transform enemyForward;
    private float stunTime = 3.26f;
    private float stunTimer = 0f;
   [SerializeField]private AudioSource[] souce;
   [SerializeField]private AudioClip[] clips;


    private void Update()
    {

        CheckSturn();

        if(!isStunned)
        CheckAll();

        if(isMoving)
        {
           
        }

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
                        if (Vector3.Distance(isFrontDoor.gameObject.transform.position, transform.position) >= 1.5f )
                        {
                            print("�������� �̵�");
                            print(Vector3.Distance(isFrontDoor.gameObject.transform.position, transform.position));
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
                stunTimer += Time.deltaTime;
                if(stunTimer >= stunTime)
                {
                    stunTimer = 0f;
                    isStunned = false;
                    state = State.Move;
                  
                }
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
            case State.Over:

                EnemySFX(0);
                break;
        }

        animator.SetInteger("State",(int) state);

    }


    public void Finsih()
    {
        isStunned = false;
        state = State.Move;
    }

    private void CheckSturn()
    {
       float halfFOV = 45 * 0.5f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);


        RaycastHit lefthit;
        bool leftHitSuccess = Physics.Raycast(enemyForward.position,leftRayRotation * transform.forward,out lefthit,5f,1 << 14);
        Debug.DrawRay(enemyForward.position, leftRayRotation * transform.forward * 5f, Color.red);

        RaycastHit rightHit;
        bool rightHitSuccess = Physics.Raycast(enemyForward.position, rightRayRotation * transform.forward, out rightHit, 5f, 1 << 14);
        Debug.DrawRay(enemyForward.position, rightRayRotation * transform.forward * 5f, Color.green);

        RaycastHit fowardHit;
        bool fowardHitSuccess = Physics.Raycast(enemyForward.position, transform.forward, out fowardHit, 5f, 1 << 14);
        Debug.DrawRay(enemyForward.position, transform.forward * 5f, Color.blue);

        if (leftHitSuccess || rightHitSuccess || fowardHitSuccess)
        {
                agent.ResetPath();
                isStunned = true;
                state = State.Stun;
         }
     

    }

    private void EnemyMoveSound(int _num) //������ �ϴ� ������ҽ�
    {
        if (souce[0].clip == clips[_num])
        {
            return;  
        }
        else
        {
                souce[0].clip = clips[_num];
                souce[0].Play();
        }
    }
    private void EnemySFX(int _num)
    {
        if (souce[1].isPlaying)
        {
            return;
        }
        else
        {
            souce[1].clip = clips[_num];
            souce[1].Play();
        }
    }

}
