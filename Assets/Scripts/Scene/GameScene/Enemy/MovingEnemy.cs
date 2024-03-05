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
    protected float lookingAroundDuration = 6f; // LookingAround �ִϸ��̼� �ð�
    protected float lookingAroundTimer = 0f; // LookingAround ���¿��� ����� �ð�

    protected Coroutine openDoorCoroutine;
    protected EnemyCameraDetection enemyCameraDetection;
    [SerializeField] protected bool watchedPlayer = false; // �� ī�޶� �÷��̾ ���Դ��� 
    [SerializeField] protected Door isFrontDoor = null;
    [SerializeField] protected Vector3 targetPos;
    protected bool isOpenAndMove = false;

    public float timer = 0f;
    protected float eventDelay = 30f;
    public bool isMoving = false;

    public float runSpeed = 3.5f;

    private new void Awake()
    {
        base.Awake();
        enemyCameraDetection = GetComponent<EnemyCameraDetection>();
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
            return;
        }

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

    protected void MoveToNextTransform()
    {
        isMoving = true;

        bool isFind = false;
        Vector3 randomPosition = Vector3.zero;

        while (!isFind)
        {
            randomPosition = Random.insideUnitSphere * 30f; // �ݰ� �ȿ��� ������ ��ġ ����
            randomPosition += transform.position; // ���� ���� ��ġ���� ������ ��ġ�� �̵�

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


    protected void EnemyEvent() //�ð����� �÷��̾�� ���ϰ���� ��ġ�� ���Բ��ϴ� �̺�Ʈ
    {

        isMoving = true;
        bool isFind = false;

        while (!isFind)
        {
            Vector3 playerPosition = player.transform.position;
            Vector3 randomOffset = Random.insideUnitSphere * 5f; // �÷��̾� �ֺ�
            randomOffset.y = 0f;

            Vector3 targetPosition = playerPosition + randomOffset; // �÷��̾� �ֺ� ��ġ ���
            targetPos = new Vector3(targetPosition.x, transform.position.y, targetPosition.z); // targetPos �ʱ�ȭ
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
