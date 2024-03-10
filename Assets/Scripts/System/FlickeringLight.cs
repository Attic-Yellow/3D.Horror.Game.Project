using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    public Light[] pointLight;
    public float minDelay = 0.1f;
    public float maxDelay = 0.5f;

    private void Start()
    {
        StartCoroutine(Flicker());
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            for (int i = 0; i < pointLight.Length; i++)
            {
                pointLight[i].enabled = !pointLight[i].enabled;
            }
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);
        }
    }
}
