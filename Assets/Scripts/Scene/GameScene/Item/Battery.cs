using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    private float addTime = 10f;
    private Flashlight flashlight;
    private void Awake()
    {
         flashlight = FindObjectOfType<Flashlight>();
    }
    public void Use()
    {  
        flashlight.AddTime(addTime);
    }


}
