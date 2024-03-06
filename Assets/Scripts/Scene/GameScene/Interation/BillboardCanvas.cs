using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardCanvas : MonoBehaviour
{
    private void Update()
    {        
        // 카메라 방향으로의 벡터를 계산
        Vector3 direction = Camera.main.transform.position - transform.position;
        direction.y = 0; // y 축 변경을 무시하도록 y 값을 0으로 설정

        // 새 방향으로 회전시키기 위한 Quaternion을 계산
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // 오브젝트의 회전을 새로운 회전으로 설정
        transform.rotation = targetRotation;
    }
}
