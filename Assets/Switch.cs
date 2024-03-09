using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Switch : MonoBehaviour
{
    [SerializeField] private Light[] lights;
    [SerializeField] private bool isOn = true;
    [SerializeField] private TextMeshProUGUI onText;
    [SerializeField] private TextMeshProUGUI offText;

    private void Start()
    {
        if (lights != null)
        {
            foreach (Light light in lights)
            {
                light.enabled = isOn;
                onText.gameObject.SetActive(!isOn);
                offText.gameObject.SetActive(isOn);
            }
        }
    }

    public void OnOffLights()
    {
        isOn = !isOn;
        int soundNum = isOn ? 12 : 13;
        GameManager.instance.settingsManager.PlayClip(soundNum);
        foreach (Light light in lights)
        {
            light.enabled = isOn;
            onText.gameObject.SetActive(!isOn);
            offText.gameObject.SetActive(isOn);
        }
    }
}
