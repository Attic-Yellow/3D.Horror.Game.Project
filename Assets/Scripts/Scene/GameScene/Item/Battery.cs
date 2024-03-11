using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    private float addTime = 120f;
    private PlayerLight playerLight;
    private void Start()
    {
        playerLight = FindObjectOfType<PlayerLight>();
    }
  
  public void Use()
    {  
        playerLight.AddTime(addTime);
    }


}
