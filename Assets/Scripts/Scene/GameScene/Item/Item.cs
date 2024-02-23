using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Item : MonoBehaviour
{
    //������ ���� Ÿ�ٵ�
    public Transform handTargetPos; //���� ��ġ
    public Transform hintPos;
    private Quaternion originalRotate;

    private void Awake()
    {
        originalRotate = transform.rotation;
    }

    public void SetTransform(Transform newPos)
    {
        transform.SetParent(newPos);
        transform.localPosition = Vector3.zero;
        transform.localRotation = originalRotate;
    }
}
