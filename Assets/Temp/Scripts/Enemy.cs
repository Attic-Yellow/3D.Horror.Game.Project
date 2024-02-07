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
    public bool watchedPlayer = false; // �� ī�޶� �÷��̾ ���Դ��� 
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


    }


    protected void EnemyEvent() //�ð����� �÷��̾�� ���ϰ���� ��ġ�� ���Բ��ϴ� �̺�Ʈ
    {
        Vector3 playerPosition = player.transform.position;
        Vector3 randomOffset = Random.insideUnitSphere * 5f; // �÷��̾� �ֺ�
        randomOffset.y = 0f;

        Vector3 targetPosition = playerPosition + randomOffset; // �÷��̾� �ֺ� ��ġ ���
        targetPos = new Vector3(targetPosition.x, transform.position.y, targetPosition.z); // targetPos �ʱ�ȭ

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
        print("������ �ִϸ��̼�");
        yield return new WaitForSeconds(5f); //�������½ð� ���
        isFrontDoor.OpenDoor();
        print("����");
        yield return new WaitUntil(() => isFrontDoor.isOpen);
        /*nms.BuildNavMesh();*/
        print("������� �̵�");
        agent.SetDestination(isFrontDoor.inRoomTransform.position);
        yield return new WaitUntil(() => agent.remainingDistance <= agent.stoppingDistance);
        print("��");
        isOpenAndMove = false;
        isFrontDoor = null;

    }
}
