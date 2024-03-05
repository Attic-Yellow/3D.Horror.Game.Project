using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashlightBattery : MonoBehaviour
{
   public Image batteyUI;
  public  Flashlight flashlight;


    private void Update()
    {
        batteyUI.fillAmount = flashlight.GetBatteyTime();
    }
}
