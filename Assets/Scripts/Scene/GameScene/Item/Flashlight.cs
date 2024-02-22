using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;

public class Flashlight : Item
{
    public float maxBatteryTime = 10f;
    public float currentBatteryTime;
    public GameObject lightGO;
    private bool isOn = false;
    public GameObject lightColl; //적이 눈뽕맞는걸 체크하는 콜라이더

    void Start()
    {
        lightGO.SetActive(isOn);
        currentBatteryTime = maxBatteryTime;
    }

    private void OnDisable()
    {
        isOn = false;

    }

    void Update()
    {
        lightGO.SetActive(isOn);
        lightColl.SetActive(isOn);
        if (Input.GetKeyDown(KeyCode.X) && gameObject.activeSelf && Holder.Instance.isHaveItems.ContainsKey(name)&&currentBatteryTime > 0)
        {
            isOn = !isOn;         
        }
        if (isOn)
        {
            currentBatteryTime -= Time.deltaTime;
            if (currentBatteryTime <= 0)
            {
                isOn = false;
            }
        }
    }

    public void AddTime(float time)
    {
        currentBatteryTime+= time;
        if(currentBatteryTime > maxBatteryTime)
        {
            currentBatteryTime = maxBatteryTime;
        }    }
}
