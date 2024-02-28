using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CurrentWorkList : Item
{
    private int openCount = 0;
    public GameObject Left;


    private void OnEnable()
    {
        if (openCount > 0)
        {
        /*    Vector3 targetDirection = Camera.main.transform.forward;
            transform.localRotation = Quaternion.LookRotation(targetDirection);*/
            Vector3 cameraPosition = Camera.main.transform.position;
            SetPosition(cameraPosition + Camera.main.transform.forward * 0.5f);
        }
    }

    private void OnDisable()
    {
        openCount++;
        if (openCount == 1)
        {
            OpenFile();
        }

    }
    public void OpenFile()
    {
        gameObject.transform.localRotation = Quaternion.Euler(-90,0,0);
        Left.transform.localRotation = Quaternion.Euler(0, 0, -170);
    }

    public void SetPosition(Vector3 vc)
    {
        gameObject.transform.position = vc;
    }

}
