using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCTV : Item
{
    public GameObject phonePanel;

    private void Update()
    {
        if(ItemManager.Instance.isHaveItems.ContainsKey(name)&&Input.GetKeyDown(KeyCode.Z))
        {
          
            //TODO : �ٸ��� ȭ������ �Ѿ�� ����
        }
    }

}
