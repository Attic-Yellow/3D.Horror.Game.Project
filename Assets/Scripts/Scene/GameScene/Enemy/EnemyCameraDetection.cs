using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class EnemyCameraDetection : MonoBehaviour
{
   [SerializeField] private Transform enemyForward; // 적의 카메라
    private float maxDetectionDistance = 40f; // 시야 최대 감지 거리
    private float doorCheckDistance = 10f;
    private float fieldOfViewAngle = 60f; // 시야각
 


    public bool IsPlayerVisible()
    {
        Vector3 cameraPos = enemyForward.transform.position;

        float halfFOV = fieldOfViewAngle * 0.5f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);

        // 왼쪽 방향으로 레이캐스트
        RaycastHit leftHit;
        if( Physics.Raycast(cameraPos, leftRayRotation * enemyForward.transform.forward, out leftHit, maxDetectionDistance))
        {
            if(leftHit.collider.gameObject.CompareTag("Player"))
            {
                return true;
            }
        }
        Debug.DrawRay(cameraPos, leftRayRotation * enemyForward.transform.forward * maxDetectionDistance, Color.red);

        // 오른쪽 방향으로 레이캐스트
        RaycastHit rightHit;
        if(Physics.Raycast(cameraPos, rightRayRotation * enemyForward.transform.forward, out rightHit, maxDetectionDistance))
        {
            if(rightHit.collider.gameObject.CompareTag("Player"))
            {
                return true;
            }
        }

        Debug.DrawRay(cameraPos, rightRayRotation * enemyForward.transform.forward * maxDetectionDistance, Color.green);

        // 정면 방향으로 레이캐스트
        RaycastHit forwardHit;
        if(Physics.Raycast(cameraPos, enemyForward.transform.forward, out forwardHit, maxDetectionDistance))
        {
            if(forwardHit.collider.gameObject.CompareTag("Player"))
            {
                return true;    
            }
        }
        
        Debug.DrawRay(cameraPos, enemyForward.transform.forward * maxDetectionDistance, Color.blue);

        return false;
    }


    public Door DoorCheck()
    {
     
        RaycastHit hit;
        Debug.DrawRay(enemyForward.transform.position, enemyForward.transform.forward * doorCheckDistance, Color.black);
        if (Physics.Raycast(enemyForward.transform.position, enemyForward.transform.forward, out hit, doorCheckDistance, 1 <<6))
        {
            if (hit.collider.gameObject.CompareTag("InDoor") || hit.collider.gameObject.CompareTag("OutDoor"))
            {
                Door door = hit.collider.GetComponentInParent<Door>();
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
