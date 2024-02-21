using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostLight : MonoBehaviour
{
    private float detectionRadius = 0.8f;
    public GameObject lightObj;
    private bool isGhostDetected = false;

    private void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius);

        isGhostDetected = false;

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Ghost"))
            {
                isGhostDetected = true;
                break;
            }
        }

        lightObj.SetActive(isGhostDetected);
    }

}

