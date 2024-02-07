using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    public Transform surface;
    public NavMeshSurface nms;
    public Animator animator;
    public NavMeshAgent agent;
    public Player player;

    protected EnemyCameraDetection enemyCameraDetection;
    public bool watchedPlayer = false; // 적 카메라에 플레이어가 들어왔는지 
    public Door isFrontDoor = null;
    public Vector3 targetPos;
    protected bool isOpenAndMove = false;

    public float timer = 0f;
    protected float eventDelay = 20f;
    public bool isFollowingPlayer = false;
    public bool isMoving = false;

    public float runSpeed = 3.5f;
    public float walkSpeed = 2f;

    protected void Awake()
    {
        enemyCameraDetection = GetComponent<EnemyCameraDetection>();
        player = FindObjectOfType<Player>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        nms.BuildNavMesh();
    }
    protected void CheckDistance()
    {
        if (Vector3.Distance(this.transform.position, surface.position) > 5f || player.isOpened)
        {
            surface.position = this.transform.position;
            nms.BuildNavMesh();
            player.isOpened = false;
        }
    }

    protected void MoveToNextTransform()
    {
        isMoving = true;

        bool isFind = false;
        NavMeshHit hit;
        Vector3 randomPosition = Vector3.zero;

        while (!isFind)
        {
            // 랜덤한 위치 선택
            randomPosition = Random.insideUnitSphere * 30f; // 반경 안에서 랜덤한 위치 생성
            randomPosition += transform.position; // 적의 현재 위치에서 랜덤한 위치로 이동

            if (NavMesh.SamplePosition(randomPosition, out hit, 30f, NavMesh.AllAreas))
            {
                targetPos = hit.position;
                agent.speed = walkSpeed;
                isFind = true;
                print($"위치 : {hit.position}");
            }
        }

        agent.SetDestination(targetPos);


    }


    protected void EnemyEvent() //시간마다 플레이어에게 제일가까운 위치로 가게끔하는 이벤트
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 randomOffset = Random.insideUnitSphere * 5f; // 플레이어 주변
        randomOffset.y = 0f;

        Vector3 targetPosition = playerPosition + randomOffset; // 플레이어 주변 위치 계산
        targetPos = new Vector3(targetPosition.x, transform.position.y, targetPosition.z); // targetPos 초기화

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, 5.0f, NavMesh.AllAreas))
        {
            agent.speed = walkSpeed;
            agent.SetDestination(hit.position);
        }
        if (agent.remainingDistance <= agent.stoppingDistance)
            isFollowingPlayer = false;
    }

    protected IEnumerator OpenAndIntheRoom()
    {
        isOpenAndMove = true;
        animator.SetTrigger("IsOpen");
        print("문여는 애니메이션");
        yield return new WaitForSeconds(5f); //문열리는시간 대기
        isFrontDoor.OpenDoor();
        print("문염");
        yield return new WaitUntil(() => isFrontDoor.isOpen);
        /*nms.BuildNavMesh();*/
        print("방안으로 이동");
        agent.SetDestination(isFrontDoor.inRoomTransform.position);
        yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance);
        print("끝");
        isOpenAndMove = false;
        isFrontDoor = null;

    }
}
