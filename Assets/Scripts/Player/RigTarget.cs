using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigTarget : MonoBehaviour
{
    public Transform Target;

    private void Update()
    {
        if (Target == null)
            return;

        transform.position = Target.position;
        transform.rotation = Target.rotation;
    }
    public void SetTransform(Transform newPos)
    {
        transform.position = newPos.position;
        transform.rotation = newPos.rotation;
    }
   
}
