using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    private float plusTime = 10f;
    public  void Use()
    {
        Flashlight flashlight = FindObjectOfType<Flashlight>();
        flashlight.AddTime(plusTime);
    }


}
