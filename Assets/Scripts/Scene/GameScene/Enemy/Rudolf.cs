using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rudolf : Enemy
{
 /*   public bool isStunned = false;
    private new void Awake()
    {
        base.Awake();
    }


    private void Update()
    {
        CheckSturn();
        if (isStunned)
            return;

        *//*CheckDistance();*//*
        watchedPlayer = enemyCameraDetection.IsPlayerVisible(player.transform.position);
        animator.SetBool("IsWatched", watchedPlayer);



        if (isFrontDoor == null)
        {
            isFrontDoor = enemyCameraDetection.DoorCheck();
            timer += Time.deltaTime;
            if (watchedPlayer) // �߰ߵ��� �� �÷��̾ ���󰡴�
            {
                timer = 0f;
                print($"{player.transform.position}�÷��̾� ���󰡴���");
                isFollowingPlayer = true;
                agent.speed = runSpeed;
                agent.SetDestination(player.transform.position);
            }
            else
            {
                isFollowingPlayer = false;
            }

            if (!agent.hasPath)
            {
                isMoving = false;
            }
            if (timer >= eventDelay) // �÷��̾ �� �� �� eventDelay��ŭ ������ ��
            {
                print("�̺�Ʈ");
                isFollowingPlayer = true;
                EnemyEvent();
            }

            if (!watchedPlayer && !isFollowingPlayer && !isMoving) // �÷��̾ �������� ���ϰ� ���󰡴� ���� �ƴ� ��
            {
                print("�Ϲ����� �̵�");

                MoveToNextTransform();

            }
        }
        else
        {
            if (Vector3.Distance(isFrontDoor.transform.position, transform.position) >= 1f)
            {
                agent.SetDestination(isFrontDoor.transform.position);
            }
            else
            {
                if (!isOpenAndMove)
                    StartCoroutine(OpenAndIntheRoom());
            }

        }
    }

    public void Sturn()
    {
        if (!isStunned)
        {
            agent.ResetPath();
            isStunned = true;
            animator.SetTrigger("isSturn");
            StartCoroutine(RecoverFromStun());
        }
    }

    IEnumerator RecoverFromStun()
    {
        yield return new WaitForSeconds(3f);
        isStunned = false;
    }

    public void CheckSturn()
    {


        float halfFOV = 45 * 0.5f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);

        // ���� �����κ��� �þ߰� ���� ���� ��� �������� ���̸� ��� �浹 ����� �迭�� ����
        RaycastHit[] hits = new RaycastHit[10];
        int hitCount = 0;

        hitCount += Physics.RaycastNonAlloc(transform.position, leftRayRotation * transform.forward, hits, 5f);
        Debug.DrawRay(transform.position, leftRayRotation * transform.forward * 5f, Color.red);
        hitCount += Physics.RaycastNonAlloc(transform.position, rightRayRotation * transform.forward, hits, 5f);
        Debug.DrawRay(transform.position, rightRayRotation * transform.forward   * 5f, Color.green);
        hitCount += Physics.RaycastNonAlloc(transform.position, transform.forward, hits, 5f);
        Debug.DrawRay(transform.position, transform.forward * 5f, Color.blue);
        for (int i = 0; i < hitCount; i++)
        {
            if (hits[i].collider.CompareTag("Light"))
            {
                Sturn();
            }
        }

    }*/

}
