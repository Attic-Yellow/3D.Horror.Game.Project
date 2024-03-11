using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;

public class PlayerLight : MonoBehaviour
{
    private float maxBatteryTime = 180f;
    private float currentBatteryTime;

    [SerializeField] FlashlightBattery flashlightBattery;
    [SerializeField] CapsuleCollider capsuleCollider;
    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        currentBatteryTime = maxBatteryTime;
        flashlightBattery.playerLight = this;
        flashlightBattery.batteyUI.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        if (flashlightBattery != null)
        {
            flashlightBattery.batteyUI.gameObject.SetActive(true);
        }
    }
    private void OnDisable()
    {
        if (flashlightBattery != null)
            flashlightBattery.batteyUI.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (flashlightBattery != null)
        {
            flashlightBattery.batteyUI.gameObject.SetActive(gameObject.activeSelf);
            capsuleCollider.enabled = gameObject.activeSelf;
        }
        if (currentBatteryTime > 0)
        {
            currentBatteryTime -= Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void AddTime(float time)
    {
        currentBatteryTime += time;
        if (currentBatteryTime > maxBatteryTime)
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
