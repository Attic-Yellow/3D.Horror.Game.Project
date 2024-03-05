using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;

public class EnemyCameraDetection : MonoBehaviour
{
   [SerializeField] private Transform enemyForward; // ���� ī�޶�
    [SerializeField] private LayerMask playerLayer; // �÷��̾ ���� ���̾�
    private float maxDetectionDistance = 40f; // �þ� �ִ� ���� �Ÿ�
    private float doorCheckDistance = 10f;
    private float fieldOfViewAngle = 60f; // �þ߰�
 


    public bool IsPlayerVisible()
    {
        Vector3 cameraPos = enemyForward.transform.position;

        float halfFOV = fieldOfViewAngle * 0.5f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);

        // ���� �������� ����ĳ��Ʈ
        RaycastHit leftHit;
        bool leftHitSuccess = Physics.Raycast(cameraPos, leftRayRotation * enemyForward.transform.forward, out leftHit, maxDetectionDistance, playerLayer);
        Debug.DrawRay(cameraPos, leftRayRotation * enemyForward.transform.forward * maxDetectionDistance, Color.red);

        // ������ �������� ����ĳ��Ʈ
        RaycastHit rightHit;
        bool rightHitSuccess = Physics.Raycast(cameraPos, rightRayRotation * enemyForward.transform.forward, out rightHit, maxDetectionDistance, playerLayer);
        Debug.DrawRay(cameraPos, rightRayRotation * enemyForward.transform.forward * maxDetectionDistance, Color.green);

        // ���� �������� ����ĳ��Ʈ
        RaycastHit forwardHit;
        bool forwardHitSuccess = Physics.Raycast(cameraPos, enemyForward.transform.forward, out forwardHit, maxDetectionDistance, playerLayer);
        Debug.DrawRay(cameraPos, enemyForward.transform.forward * maxDetectionDistance, Color.blue);


        if(leftHitSuccess ||  rightHitSuccess || forwardHitSuccess)
        {
            return true;
        }

        return false;
    }


    public Door DoorCheck()
    {
     
        RaycastHit hit;
        Debug.DrawRay(enemyForward.transform.position, enemyForward.transform.forward * doorCheckDistance, Color.black);
        if (Physics.Raycast(enemyForward.transform.position, enemyForward.transform.forward, out hit, doorCheckDistance))
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
