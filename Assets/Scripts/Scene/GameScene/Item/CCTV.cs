using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV : Item
{
    private void Update()
    {
        if(Holder.Instance.isHaveItems.ContainsKey(name)&&Input.GetKeyDown(KeyCode.Z))
        {
          
            //TODO : �ٸ��� ȭ������ �Ѿ�� ����
        }
    }
}
