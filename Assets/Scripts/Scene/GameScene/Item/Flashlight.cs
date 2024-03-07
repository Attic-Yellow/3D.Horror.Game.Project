using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;

public class Flashlight : Item
{
    private float maxBatteryTime = 10f;
    private float currentBatteryTime;
    [SerializeField] private GameObject lightGO;
   [SerializeField] private bool isOn = false;
    [SerializeField] private GameObject lightColl; //적이 눈뽕맞는걸 체크하는 콜라이더
    private FlashlightBattery flashlightBattery;
    private void Start()
    {
        lightGO.SetActive(isOn);
        currentBatteryTime = maxBatteryTime;
        flashlightBattery = FindObjectOfType<FlashlightBattery>();
        flashlightBattery.flashlight = this;
        flashlightBattery.batteyUI.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        if(flashlightBattery != null)
        {
            flashlightBattery.batteyUI.gameObject.SetActive(true);
        }
    }
    private void OnDisable()
    {
        isOn = false;
        if (flashlightBattery != null)
            flashlightBattery.batteyUI.gameObject.SetActive(false);
    }

    void Update()
    {
      
        if (Input.GetKeyDown(KeyCode.X) && gameObject.activeSelf && Holder.Instance.isHaveItems.ContainsKey(name))
        {
            if(currentBatteryTime > 0)
            isOn = !isOn;

            GameManager.instance.settingsManager.PlayClip(9);
        }
        if (isOn)
        {
            currentBatteryTime -= Time.deltaTime;
            if (currentBatteryTime <= 0)
            {
                isOn = false;
            }
        }
        lightGO.SetActive(isOn);
        lightColl.SetActive(isOn);
    }

    public void AddTime(float time)
    {
        currentBatteryTime+= time;
        if(currentBatteryTime > maxBatteryTime)
        {
            currentBatteryTime = maxBatteryTime;
        }   
    }

    public float GetBatteyTime()
    {
        float fillRatio = currentBatteryTime / maxBatteryTime;
        return fillRatio;
    }
}
