using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
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

    public AudioSource[] souces;
    public AudioClip[] clips;

    private Door nearstDoor = null;
    private void Update()
    {
        CheckSturn();

        if(!isStunned)
       this.CheckAll();

        if(agent.hasPath)
        {
            if (state != State.Follow)
            {
                SoundPitchChange(0.5f);
                EnemySFX(0,clips[0]);
            }
            else
            {
                SoundPitchChange(0.5f * 1.5f);
                EnemySFX(0,clips[0]);
            }
        }

        switch (state)
        {

            case State.Move:
           
                if (isFrontDoor == null )
                {
                        if (timer >= eventDelay) // 플레이어를 못 본 지 eventDelay만큼 지났을 때
                        {
                            if (!isEvent)
                             {
                                  print("이벤트");
                                 EnemyEvent();
                             }
                              else
                               {
                            //이벤트가 활성화되어있는데 
                                }
                        }
                        else if(!agent.hasPath)// 플레이어를 감지하지 못하고 따라가는 중이 아닐 때
                        {
                            MoveToNextTransform();
                        }
                }
                else 
                {
                    if (!isOpenAndMove)
                    {

                        if (Vector3.Distance(isFrontDoor.gameObject.transform.position, transform.position) >= 2f)
                        {
                            if (!agent.hasPath)
                            {
                                print("문앞으로 이동");
                                agent.SetDestination(isFrontDoor.gameObject.transform.position);
                            }
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
                                if (!agent.hasPath)
                                {
                                print("방안으로 이동");
                                agent.SetDestination(isFrontDoor.inRoomTransform.position);
                                }
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
                print("플레이어 따라가는중");
                timer = 0f;
                agent.speed = runSpeed;
                agent.SetDestination(player.transform.position);
                if (!agent.hasPath)
                {
                    isFrontDoor = MostNearDoor();
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

    private void SoundPitchChange(float _num)
    {
        if (souces[0].pitch == _num)
            return;

            souces[0].pitch = _num;
    }
    public void EnemySFX(int souceNum, AudioClip _clip)
    {
        if (souces[souceNum].isPlaying)
        {
            return;
        }
        else
        {
            souces[souceNum].clip = _clip;
            souces[souceNum].Play();
        }
    }

    protected IEnumerator OpenAndIntheRoom()
    {
        agent.ResetPath();
        isOpenAndMove = true;
        print("문여는 애니메이션");
        yield return new WaitForSeconds(1f);
        EnemySFX(1,clips[1]);
        yield return new WaitForSeconds(5f); //문열리는시간 대기
        isFrontDoor.OpenDoor(gameObject);
        print("문염");
        yield return new WaitUntil(() => isFrontDoor.isOpen);
        /*nms.BuildNavMesh();*/
        state = State.Move;
        openDoorCoroutine = null;
    }

    protected override void CheckAll()
    {
        timer += Time.deltaTime;

        if (player.isOver)
        {
            agent.ResetPath();
            state = State.Over;         
        }
        else
        {
            watchedPlayer = enemyCameraDetection.IsPlayerVisible();

            if (state != State.Follow && (watchedPlayer || IsMovingCheck()))
            {
                if (openDoorCoroutine != null)
                {
                    StopCoroutine(openDoorCoroutine);
                    isOpenAndMove = false;
                }
                isEvent = false;
                state = State.Follow;
               
                return;
            }

            if (isEvent && !agent.hasPath)
            {
                isFrontDoor = MostNearDoor();
            }



            if (isFrontDoor == null)
            {
                isFrontDoor = enemyCameraDetection.DoorCheck();
            }
        }
    }

    private Door MostNearDoor() //쫒아가다가 놓쳤을떄 가장가까운 문을 열도록
    {     
        Collider[] colliders = Physics.OverlapSphere(transform.position, 20f);
        List<Door> doors = new();
        foreach(Collider collider in colliders)
        {
            if (collider.GetComponent<Door>() != null)
            {
                Door door = collider.GetComponent<Door>();
                if (!door.isLock && door.isRoomDoor &&(!door.isOpen || door.doorCoroutine != null))
                {                
                  doors.Add(door);
                }
            }
        }
        print(doors.Count);
        if(doors.Count == 1)
        {
            print("감지가능한 문이 한개밖에없음");
            return doors[0];
        }

        else if (doors.Count > 1)
        {
            Vector3 diffDis;
            float nearstDiff = 9999;

            if(state != State.Follow)
            {
                diffDis = transform.position;
                for (int i = 0; i < doors.Count; i++)
                {
                    print($"전체 {doors[i].name}");
                    if ( 2< Mathf.Abs(doors[i].transform.position.y - diffDis.y))
                    {
                        print($"y값의 차이가 많이 나는 놈{i}");
                        continue;
                    }
                    if (nearstDiff > Vector3.Distance(doors[i].transform.position, diffDis))
                    {
                        nearstDiff = Vector3.Distance(doors[i].transform.position, diffDis);
                        nearstDoor = doors[i];
                    }
                }
            }
            else
            {
                diffDis = player.transform.position - transform.position;
                for (int i = 0; i < doors.Count; i++)
                {
                    if (nearstDiff > Vector3.Distance(doors[i].transform.position, diffDis))
                    {
                        nearstDiff = Vector3.Distance(doors[i].transform.position, diffDis);
                        nearstDoor = doors[i];
                    }
                }
            }
          
        }
        else
        {
            return null;
        }
        Door thisDoor = nearstDoor;
        nearstDoor = null;
        return thisDoor;
    }
}
