using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class ComeGhost : Enemy
{
    public float boxXDistance = 10f; 
    public float boxZDistance = 2f;
    public bool isSee = false;
    public GameObject[] childObj;
    private Vector3 detectionBoxSize; // 감지 박스의 크기

    private new void Awake()
    {
        base.Awake();
        detectionBoxSize = new Vector3(boxXDistance, 1f, boxZDistance);
    }
    void Update()
    {
        if (!player.isOver)
        {
            // 일정 범위 내에 있는 모든 플레이어를 검출
            Collider[] colliders = Physics.OverlapBox(transform.position + transform.forward * boxXDistance * 0.5f, detectionBoxSize * 0.5f);
            bool playerDetected = false; // 플레이어를 발견했는지 여부를 나타내는 변수

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    // 플레이어를 발견함
                    playerDetected = true;
                    Debug.Log("귀신이 플레이어를 감지했습니다!");
                    break; // 더 이상 검사하지 않고 루프를 빠져나감
                }
            }
           


            if (playerDetected)
            {
                transform.LookAt(player.transform);

                if (isSee)
                {
                    foreach (GameObject obj in childObj)
                    {
                        obj.SetActive(true);
                    }
                }
                else
                {
                    foreach (GameObject obj in childObj)
                    {
                        obj.SetActive(false);
                    }
                    // 플레이어 방향으로 귀신을 이동시킴
                    Vector3 direction = (player.transform.position - transform.position).normalized;
                    direction.y = 0; // y축 이동 방향 제거
                    transform.Translate(direction * walkSpeed * Time.deltaTime, Space.World);
                }
            }
            else
            {
                SetActiveTrue();
            }
        }
    }

    public void SetActiveTrue()
    {
        foreach (GameObject obj in childObj)
        {
            obj.SetActive(true);
        }
    }
}
