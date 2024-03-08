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
   public AudioSource[] souces;
   public AudioClip[] clips;


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
                }
                else 
                {
                    if (!isOpenAndMove)
                    {
                        if (!agent.hasPath)
                        {
                            if (Vector3.Distance(isFrontDoor.gameObject.transform.position, transform.position) >= 2f)
                            {
                                print("문앞으로 이동");
                                print(isFrontDoor.gameObject.transform.position);
                                agent.SetDestination(isFrontDoor.gameObject.transform.position);
                            }
                            else
                            {
                                state = State.OpenDoor;
                            }
                        }
                    }
                    else
                    {
                        if (Vector3.Distance(isFrontDoor.inRoomTransform.position, transform.position) >= 1f)
                        {
                            print("방안으로 이동");
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
                print("OVER");
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
                print($"state{state != State.Follow}, watch {watchedPlayer}, move {IsMovingCheck()} ");
                if (openDoorCoroutine != null)
                {
                    StopCoroutine(openDoorCoroutine);
                    isOpenAndMove = false;
                }
                state = State.Follow;
                print("여기서 follow로");

                return;
            }

            if (isFrontDoor == null)
            {
                isFrontDoor = enemyCameraDetection.DoorCheck();

            }
            if (!agent.hasPath && isMoving)
            {
                isMoving = false;

            }
        }
    }

    private Door MostNearDoor()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10f);
        Door[] doors = new Door[2];
        foreach(Collider collider in colliders)
        {
            if (collider.GetComponent<Door>() != null)
            {
                Door door = collider.GetComponent<Door>();
                if (!door.isLock && door.isRoomDoor &&(!door.isOpen || door.doorCoroutine != null))
                {                
                    if (doors[0] != null && doors[1] == null)
                    {
                        doors[1] = door;
                        break;
                    }
                    if (doors[0] == null)
                        doors[0] = door;
                }
            }
        }
        Vector3 diffDis = player.transform.position - transform.position;
        float[] diff = new float[doors.Length];
       for(int i = 0; i < doors.Length; i++) 
        {
          diffDis[i] =  Vector3.Distance(doors[i].transform.position, diffDis);
        }

        bool nearstDoorIsFirst = false;
        if (diff[0] < diff[1])
        {
            nearstDoorIsFirst = true;
        }
        Door nearstDoor;
        print($"{doors[0].transform.position}, {doors[1].transform.position}");
        nearstDoor = nearstDoorIsFirst ? doors[0] : doors[1];
        return nearstDoor;

    }
}
