using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Door
{
    [SerializeField] private GameObject target;

    private new void Start()
    {
        for (int i = 0; i < openText.Length; i++)
        {
            if (openText != null)
            {
                openText[i].gameObject.SetActive(!isOpen);
            }
        }
    }

    public void OpenShield()
    {
        Quaternion targetRotation = startRotation * Quaternion.Euler(0f, 90f, 0f);
        StartCoroutine(RotateDoorCoroutine(targetRotation));
    }

    public void CloseShield()
    {
        target.transform.localRotation = startRotation * Quaternion.Euler(0f, 180f, 90f);

        Quaternion targetRotation = startRotation * Quaternion.Euler(-90f, 90f, 0f);
        StartCoroutine(RotateDoorCoroutine(targetRotation));
    }
}
