using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    //아이템 리그 타겟들
    public Transform handTargetPos; //잡을 위치
    public string text; //상호작용 텍스트
   
    public void SetTransform(Transform newPos)
    {
        transform.SetParent(newPos);
        transform.localPosition = Vector3.zero;

    }
}
