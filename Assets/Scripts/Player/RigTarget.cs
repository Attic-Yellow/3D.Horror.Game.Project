using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigTarget : MonoBehaviour
{

    public void SetTransform(Transform newPos)
    {
        transform.position = newPos.position;
        transform.rotation = newPos.rotation;
    }
   
}
