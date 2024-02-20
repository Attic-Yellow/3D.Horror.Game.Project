using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorControl : MonoBehaviour
{
    [SerializeField] private Material monitor;
    [SerializeField] private bool isOn = false;
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private float maxGlowSize = 2f;

    public void OnAndOff()
    {
        isOn = !isOn;
        monitor.SetFloat("_isOn", isOn ? 1f : 0f);

        if (isOn)
        {
            StartCoroutine(TurnOnEffect());
        }
        else
        {
            StartCoroutine(TurnOffEffect());
        }
    }

    private IEnumerator TurnOnEffect()
    {
        float elapsedTime = 0f;

        yield return new WaitForSeconds(1.15f);

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * turnSpeed;
            float currentGlowSize = Mathf.Lerp(0, maxGlowSize, elapsedTime);
            monitor.SetFloat("_GlowSize", currentGlowSize);
            yield return null;
        }
    }

    private IEnumerator TurnOffEffect()
    {
        float currentGlowSize = monitor.GetFloat("_GlowSize");
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * 4;
            monitor.SetFloat("_GlowSize", Mathf.Lerp(currentGlowSize, 0, elapsedTime));
            yield return null;
        }
    }
}
