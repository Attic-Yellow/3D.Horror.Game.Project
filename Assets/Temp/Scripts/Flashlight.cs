using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;

public class Flashlight : Item
{
    public GameObject lightGO;
    private bool isOn = false;
    public LayerMask rudolfLayer;
    private float maxDetectionDistance = 5f;
    private float fieldOfViewAngle = 60f;
    void Start()
    {
        lightGO.SetActive(isOn);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && gameObject.activeSelf && ItemManager.Instance.isHaveItems.ContainsKey(name))
        {
            isOn = !isOn;
            lightGO.SetActive(isOn);
        }


        RayCheck();

    }

    private void RayCheck()
    {
        if (isOn)
        {
            Vector3 cameraPos = lightGO.transform.position;

            float halfFOV = fieldOfViewAngle * 0.5f;
            Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
            Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);

            // 시작 각도로부터 시야각 범위 내의 모든 방향으로 레이를 쏘아 충돌 결과를 배열에 저장
            RaycastHit[] hits = new RaycastHit[10];
            int hitCount = 0;

            hitCount += Physics.RaycastNonAlloc(cameraPos, leftRayRotation * lightGO.transform.forward, hits, maxDetectionDistance, rudolfLayer);
            Debug.DrawRay(cameraPos, leftRayRotation * lightGO.transform.forward * maxDetectionDistance, Color.red);
            hitCount += Physics.RaycastNonAlloc(cameraPos, rightRayRotation * lightGO.transform.forward, hits, maxDetectionDistance, rudolfLayer);
            Debug.DrawRay(cameraPos, rightRayRotation * lightGO.transform.forward * maxDetectionDistance, Color.green);
            hitCount += Physics.RaycastNonAlloc(cameraPos, lightGO.transform.forward, hits, maxDetectionDistance, rudolfLayer);
            Debug.DrawRay(cameraPos, lightGO.transform.forward * maxDetectionDistance, Color.blue);
            for (int i = 0; i < hitCount; i++)
            {
                if (hits[i].collider.CompareTag("Rudolf"))
                {
                    Rudolf rudolf = hits[i].collider.gameObject.GetComponent<Rudolf>();
                    rudolf.Sturn();
                }
            }
        }
    }
}
