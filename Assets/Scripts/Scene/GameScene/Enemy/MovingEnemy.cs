using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovingEnemy : Enemy
{
    protected enum State
    {
        Move,
        Follow,
        Crouch,
        Stun,
        OpenDoor,
        LookingAround,
        Over
    }
   [SerializeField] protected State state;
    protected float lookingAroundDuration = 6f; // LookingAround 애니메이션 시간
    protected float lookingAroundTimer = 0f; // LookingAround 상태에서 경과한 시간

    protected Coroutine openDoorCoroutine;
    protected EnemyCameraDetection enemyCameraDetection;
    [SerializeField] protected bool watchedPlayer = false; // 적 카메라에 플레이어가 들어왔는지 
    [SerializeField] protected Door isFrontDoor = null;
    [SerializeField] protected Vector3 targetPos;
    protected bool isOpenAndMove = false;

    [SerializeField] protected float timer = 0f;
    [SerializeField] protected float eventDelay = 30f;
    [SerializeField] protected bool isMoving = false;

    public float runSpeed;

    private new void Awake()
    {
        base.Awake();
        enemyCameraDetection = GetComponent<EnemyCameraDetection>();
        runSpeed = walkSpeed * 1.5f;
    }

    protected void Start()
    {
        state = State.Move;
    }
    protected virtual void CheckAll()
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

            if (state != State.Follow && IsMovingCheck())
            {

                if (openDoorCoroutine != null)
                {
                    StopCoroutine(openDoorCoroutine);
                    isOpenAndMove = false;
                }
                state = State.Follow;

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

    protected void MoveToNextTransform()
    {
        isMoving = true;

        bool isFind = false;
        Vector3 randomPosition = Vector3.zero;

        while (!isFind)
        {
            randomPosition = Random.insideUnitSphere * 30f; // 반경 안에서 랜덤한 위치 생성
            randomPosition += transform.position; // 적의 현재 위치에서 랜덤한 위치로 이동

            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 30f, NavMesh.AllAreas))
            {
                targetPos = hit.position;
                agent.speed = walkSpeed;
                isFind = true;
                print($"hitPos : {hit.position}");
            }
        }

        agent.SetDestination(targetPos);
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f))
        {

            state = State.LookingAround;

        }



    }


    protected void EnemyEvent() //시간마다 플레이어에게 제일가까운 위치로 가게끔하는 이벤트
    {

        isMoving = true;
        bool isFind = false;

        while (!isFind)
        {
            Vector3 playerPosition = player.transform.position;
            Vector3 randomOffset = Random.insideUnitSphere * 5f; // 플레이어 주변
            randomOffset.y = 0f;

            Vector3 targetPosition = playerPosition + randomOffset; // 플레이어 주변 위치 계산
            targetPos = targetPosition;
            if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, 5.0f, NavMesh.AllAreas))
            {
                agent.speed = walkSpeed;
                targetPos = hit.position;
                timer = 0f;
                isFind = true;

            }
        }
        agent.SetDestination(targetPos);

    }

   
    protected bool IsMovingCheck()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer <= 10)
        {
            PlayerMove playerMove = player.GetComponent<PlayerMove>();
            if (playerMove.isMoving)
            {
                agent.SetDestination(player.transform.position);
                print("적이 움직이고 있는거 체크함");
                return true;
            }
            else
            {
                GameManager.instance.settingsManager.PlayClip(10);
            }
        }

        return false;


    }



    protected IEnumerator Moving(Vector3 pos)
    {
        isMoving = true;
        agent.SetDestination(pos);
        yield return new WaitUntil(() => agent.remainingDistance >= agent.stoppingDistance);
        isMoving = false;

    }


}
