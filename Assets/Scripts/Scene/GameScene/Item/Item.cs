using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    //������ ���� Ÿ�ٵ�
    public Transform handTargetPos; //���� ��ġ
   
    public void SetTransform(Transform newPos)
    {
        transform.SetParent(newPos);
        transform.localPosition = Vector3.zero;
        transform.localPosition = Vector3.zero;

    }
}
