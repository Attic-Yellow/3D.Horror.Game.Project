using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : UseItem
{

    public override void Use()
    {
        Flashlight flashlight = FindObjectOfType<Flashlight>();
        flashlight.currentBatteryTime = flashlight.maxBatteryTime;
    }


}
