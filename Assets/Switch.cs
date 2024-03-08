using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] private Light[] lights;
    private bool isOn = true;

    public void OnOffLights()
    {
        isOn = !isOn;
        int soundNum = isOn ? 12 : 13;
        GameManager.instance.settingsManager.PlayClip(soundNum);
        foreach (Light light in lights)
        {
            light.enabled = isOn;
        }
    }
}
