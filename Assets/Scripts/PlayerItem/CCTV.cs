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
          
            //TODO : 다른적 화면으로 넘어가는 로직
        }
    }

}
