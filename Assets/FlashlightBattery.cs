using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashlightBattery : MonoBehaviour
{
   public Image batteyUI;
  public  PlayerLight playerLight;


    private void Update()
    {
        if(playerLight != null)
        batteyUI.fillAmount = playerLight.GetBatteyTime();
    }
}
