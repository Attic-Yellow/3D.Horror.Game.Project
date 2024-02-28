using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;


public class Enemy : MonoBehaviour
{
    protected Animator animator;
    protected NavMeshAgent agent;
    protected Player player;

    protected enum State
    {
        Move,
        Follow,
        Crouch,
        Stun,
        OpenDoor,
        LookingAround
    }
    protected State state;
    protected float lookingAroundDuration = 6f; // LookingAround �ִϸ��̼� �ð�
    protected float lookingAroundTimer = 0f; // LookingAround ���¿��� ����� �ð�

    protected Coroutine openDoorCoroutine;
    protected EnemyCameraDetection enemyCameraDetection;
    [SerializeField] protected bool watchedPlayer = false; // �� ī�޶� �÷��̾ ���Դ��� 
    [SerializeField] protected Door isFrontDoor = null;
    [SerializeField] protected Vector3 targetPos;
    protected bool isOpenAndMove = false;
    public Transform gameoverCamPos;
    public Transform enemySpine;

    public float timer = 0f;
    protected float eventDelay = 30f;
    public bool isMoving = false;

    public float runSpeed = 3.5f;
    public float walkSpeed = 2f;

    protected void Awake()
    {
        enemyCameraDetection = GetComponent<EnemyCameraDetection>();
        player = FindObjectOfType<Player>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    protected void Start()
    {
        state = State.Move;
    }
    protected void CheckAll()
    {
        timer += Time.deltaTime;

        if(player.isOver)
        {
            agent.ResetPath();
            return;
        }

        watchedPlayer = enemyCameraDetection.IsPlayerVisible();

        if ((watchedPlayer && state != State.Follow) && IsMovingCheck())
        {
            state = State.Follow;
            if (openDoorCoroutine != null)
            {
                StopCoroutine(openDoorCoroutine);
                isOpenAndMove = false;
            }
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

    protected void MoveToNextTransform()
    {
        isMoving = true;

        bool isFind = false;
        NavMeshHit hit;
        Vector3 randomPosition = Vector3.zero;

        while (!isFind)
        {
            // ������ ��ġ ����
            randomPosition = Random.insideUnitSphere * 30f; // �ݰ� �ȿ��� ������ ��ġ ����
            randomPosition += transform.position; // ���� ���� ��ġ���� ������ ��ġ�� �̵�

            if (NavMesh.SamplePosition(randomPosition, out hit, 30f, NavMesh.AllAreas))
            {
                targetPos = hit.position;
                agent.speed = walkSpeed;
                isFind = true;
                print($"��ġ : {hit.position}");
            }
        }

        agent.SetDestination(targetPos);
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && (!agent.hasPath || agent.velocity.sqrMagnitude == 0f))
        {

            state = State.LookingAround;

        }



    }


    protected void EnemyEvent() //�ð����� �÷��̾�� ���ϰ���� ��ġ�� ���Բ��ϴ� �̺�Ʈ
    {

        isMoving = true;
        bool isFind = false;
        NavMeshHit hit;

        while (!isFind)
        {
            Vector3 playerPosition = player.transform.position;
            Vector3 randomOffset = Random.insideUnitSphere * 5f; // �÷��̾� �ֺ�
            randomOffset.y = 0f;

            Vector3 targetPosition = playerPosition + randomOffset; // �÷��̾� �ֺ� ��ġ ���
            targetPos = new Vector3(targetPosition.x, transform.position.y, targetPosition.z); // targetPos �ʱ�ȭ
            if (NavMesh.SamplePosition(targetPos, out hit, 5.0f, NavMesh.AllAreas))
            {
                agent.speed = walkSpeed;
                targetPos = hit.position;

            }
        }
        agent.SetDestination(targetPos);

    }

    protected IEnumerator OpenAndIntheRoom()
    {
        agent.ResetPath();
        isOpenAndMove = true;
        print("������ �ִϸ��̼�");
        yield return new WaitForSeconds(5f); //�������½ð� ���
        isFrontDoor.OpenDoor();
        print("����");
        yield return new WaitUntil(() => isFrontDoor.isOpen);
        /*nms.BuildNavMesh();*/
        state = State.Move;
        openDoorCoroutine = null;
    }
   
    protected bool IsMovingCheck()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer <= 15)
        {
            PlayerMove playerMove = player.GetComponent<PlayerMove>();
            if (playerMove.isMoving)
            {
                agent.SetDestination(player.transform.position);
                print("���� �����̰� �ִ°� üũ��");
                return true;
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
