using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCameraDetection : MonoBehaviour
{
    public Transform enemyForward; // 적의 카메라

    public LayerMask playerLayer; // 플레이어가 속한 레이어
    public float maxDetectionDistance = 40f; // 시야 최대 감지 거리
    public float doorCheckDistance = 10f;
    public float fieldOfViewAngle = 60f; // 시야각
 


    public bool IsPlayerVisible(Vector3 playerPos)
    {
        Vector3 cameraPos = enemyForward.transform.position;

        float halfFOV = fieldOfViewAngle * 0.5f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);

        // 시작 각도로부터 시야각 범위 내의 모든 방향으로 레이를 쏘아 충돌 결과를 배열에 저장
        RaycastHit[] hits = new RaycastHit[10];
        int hitCount = 0;

        hitCount += Physics.RaycastNonAlloc(cameraPos, leftRayRotation * enemyForward.transform.forward, hits, maxDetectionDistance, playerLayer);
        Debug.DrawRay(cameraPos, leftRayRotation * enemyForward.transform.forward * maxDetectionDistance, Color.red); 
        hitCount += Physics.RaycastNonAlloc(cameraPos, rightRayRotation * enemyForward.transform.forward, hits, maxDetectionDistance, playerLayer);
        Debug.DrawRay(cameraPos, rightRayRotation * enemyForward.transform.forward * maxDetectionDistance, Color.green);
        hitCount += Physics.RaycastNonAlloc(cameraPos, enemyForward.transform.forward,hits ,maxDetectionDistance, playerLayer);
        Debug.DrawRay(cameraPos, enemyForward.transform.forward * maxDetectionDistance, Color.blue);
        for (int i = 0; i < hitCount; i++)
            {
                if (hits[i].collider.CompareTag("Player"))
                {
                    return true;
                }
            }
     
        return false;
    }


    public Door DoorCheck()
    {
        RaycastHit hit;
        Debug.DrawRay(enemyForward.transform.position, enemyForward.transform.forward*doorCheckDistance,Color.black);
        if (Physics.Raycast(enemyForward.transform.position, enemyForward.transform.forward, out hit, doorCheckDistance))
        {
            if (hit.collider.gameObject.CompareTag("InDoor") || hit.collider.gameObject.CompareTag("OutDoor"))
            {
                Door door = hit.collider.gameObject.GetComponentInParent<Door>();
                if (hit.collider.gameObject.CompareTag("InDoor"))
                    door.isOut = false;
                if (hit.collider.gameObject.CompareTag("OutDoor"))
                    door.isOut = true;
                if (!door.isOpen)
                {
                    return door;
                }
            }
        }
        return null;
    }
}
