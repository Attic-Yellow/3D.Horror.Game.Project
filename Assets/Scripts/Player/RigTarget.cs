using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigTarget : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        if (target == null)
            return;

        transform.position = target.position;
        transform.rotation = target.rotation;
    }

   
}
    