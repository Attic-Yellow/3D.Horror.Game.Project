using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;

public class Flashlight : Item
{
    public GameObject lightGO;
    private bool isOn = false;
    /*    public LayerMask rudolfLayer;
        private float maxDetectionDistance = 5f;
        private float fieldOfViewAngle = 60f;*/
    public GameObject lightColl; //적이 눈뽕맞는걸 체크하는 콜라이더
    void Start()
    {
        lightGO.SetActive(isOn);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && gameObject.activeSelf && ItemManager.Instance.isHaveItems.ContainsKey(name))
        {
            isOn = !isOn;
            lightGO.SetActive(isOn);
            lightColl.SetActive(isOn);
        }
    }

   
}
